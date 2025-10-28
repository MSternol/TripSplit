// Prosty przełącznik motywu: zapis do localStorage + a11y ikonka
(function () {
    const root = document.documentElement;
    const btn = document.getElementById("themeToggle");
    const icon = document.getElementById("themeIcon");

    const KEY = "ts-theme";
    const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
    const saved = localStorage.getItem(KEY);
    const start = saved ?? (prefersDark ? "dark" : "light");

    function apply(mode) {
        if (mode === "dark") root.setAttribute("data-theme", "dark");
        else root.setAttribute("data-theme", "light");
        if (icon) icon.textContent = mode === "dark" ? "🌙" : "☀️";
        if (btn) btn.setAttribute("aria-label", `Motyw: ${mode === "dark" ? "ciemny" : "jasny"}`);
    }

    apply(start);

    if (btn) {
        btn.addEventListener("click", () => {
            const next = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
            apply(next);
            localStorage.setItem(KEY, next);
        });
    }
})();
