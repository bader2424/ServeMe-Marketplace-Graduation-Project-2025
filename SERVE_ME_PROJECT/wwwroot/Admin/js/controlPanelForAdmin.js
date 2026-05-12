function filterAccounts(type) {
    const rows = document.querySelectorAll("#accountsTable tbody tr");
    rows.forEach(row => {
        const accountType = row.cells[2].textContent.trim();
        row.style.display = (type === "all" || accountType === type) ? "" : "none";
    });
}