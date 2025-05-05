using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation;
using Mediator;

namespace InsureAnts.Application.Features.Abstractions;

internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly ICollection<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = (ICollection<IValidator<TRequest>>)validators;

    private static string BuildErrorMessage(IEnumerable<ValidationFailure> errors)
    {
        return string.Join(Environment.NewLine, errors.Select(vf => !string.IsNullOrEmpty(vf.PropertyName) ? $"{vf.PropertyName}: {vf.ErrorMessage}" : vf.ErrorMessage));
    }

    protected virtual void SetupContext(ValidationContext<TRequest> context) { }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (_validators.Count > 0)
        {
            var context = new ValidationContext<TRequest>(message);

            SetupContext(context);

            var failures = new List<ValidationFailure>();

            foreach (var validator in _validators)
            {
                var validationResult = await validator.ValidateAsync(context, cancellationToken);
                failures.AddRange(validationResult.Errors);
            }

            if (failures.Count > 0)
            {
                throw new ValidationException(BuildErrorMessage(failures));
            }
        }

        return await next(message, cancellationToken);
    }
}