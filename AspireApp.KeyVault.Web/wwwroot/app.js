const toggleInputType = (element, type) => {
    if (element) {
        element.setAttribute('type', type);
    }
};

// Coalescing assignment, if non-existent create window.app.
window.app = Object.assign({}, window.app, {
    toggleInputType
});