using System.Collections;

namespace InsureAnts.Application.Logging;

public class LogScopes : IDictionary<string, object?>, IFormattable
{
    private readonly Dictionary<string, object?> _state;

    public LogScopes() => _state = new Dictionary<string, object?>(StringComparer.Ordinal);

    public LogScopes(int capacity) => _state = new Dictionary<string, object?>(capacity, StringComparer.Ordinal);

    public LogScopes Add(string key, object? value)
    {
        _state[key] = value;
        return this;
    }

    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);

    void IDictionary<string, object?>.Add(string key, object? value) => _state[key] = value;

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _state.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_state).GetEnumerator();

    void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item) => _state[item.Key] = item.Value;

    void ICollection<KeyValuePair<string, object?>>.Clear() => _state.Clear();

    bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item) => ((ICollection<KeyValuePair<string, object?>>)_state).Contains(item);

    void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, object?>>)_state).CopyTo(array, arrayIndex);

    bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item) => ((ICollection<KeyValuePair<string, object?>>)_state).Remove(item);

    public int Count => _state.Count;

    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => ((ICollection<KeyValuePair<string, object?>>)_state).IsReadOnly;

    public bool ContainsKey(string key) => _state.ContainsKey(key);

    public bool Remove(string key) => _state.Remove(key);

    public bool TryGetValue(string key, out object? value) => _state.TryGetValue(key, out value);

    public object? this[string key]
    {
        get => _state[key];
        set => _state[key] = value;
    }

    public ICollection<string> Keys => _state.Keys;

    public ICollection<object?> Values => _state.Values;
}