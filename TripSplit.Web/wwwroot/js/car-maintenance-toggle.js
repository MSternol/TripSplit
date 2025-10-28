// wwwroot/js/car-maintenance-toggle.js
(function () {
    function setSectionState(checkbox, container) {
        if (!container) return;
        const enabled = !!(checkbox && checkbox.checked);

        container.querySelectorAll('input,select,textarea,button').forEach(el => {
            if (el === checkbox) return;
            if (el.type !== 'hidden') el.disabled = !enabled;
        });

        container.style.opacity = enabled ? '1' : '0.6';
        if (container.tagName === 'DETAILS') container.open = enabled;
    }

    function wire(toggleSelector, containerSelector) {
        const checkbox = document.querySelector(toggleSelector);
        const container = document.querySelector(containerSelector);
        if (!container) return;

        const summary = container.querySelector('summary');
        if (summary) {
            summary.addEventListener('click', (e) => {
                if (e.target !== checkbox) { e.preventDefault(); checkbox && checkbox.click(); }
            });
        }

        setSectionState(checkbox, container);
        checkbox && checkbox.addEventListener('change', () => setSectionState(checkbox, container));
    }

    document.addEventListener('DOMContentLoaded', function () {
        wire('.js-toggle-insurance', '.js-insurance-fields');
        wire('.js-toggle-inspection', '.js-inspection-fields');
    });
})();
