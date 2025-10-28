(function () {
    const btn = document.getElementById("navToggle");
    const links = document.getElementById("navLinks");
    if (!btn || !links) return;

    btn.addEventListener("click", () => {
        const open = links.classList.toggle("is-open");
        btn.setAttribute("aria-expanded", open ? "true" : "false");
    });

    links.addEventListener("click", (e) => {
        const a = e.target.closest("a");
        if (!a) return;
        links.classList.remove("is-open");
        btn.setAttribute("aria-expanded", "false");
    });
})();
