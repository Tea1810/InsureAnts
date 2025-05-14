window.registerFilter = function () {
    document.addEventListener("keydown", handleFilterFocus);
}

window.unregisterFilter = function () {
    document.removeEventListener("keydown", handleFilterFocus);
}

window.handleFilterFocus = function (ev) {
    if (ev.shiftKey && ev.ctrlKey && ev.key === "F") {
        document.getElementById("filter").focus()
    }
};
