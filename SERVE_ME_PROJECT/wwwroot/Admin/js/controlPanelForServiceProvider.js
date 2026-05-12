
function filterRequests() {
    const filter = document.getElementById("filter-status").value;
    const rows = document.querySelectorAll("#requests-body tr");

    rows.forEach(row => {
        if (filter === "all" || row.getAttribute("data-status") === filter) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}

function updateStatus(button, status) {
    const row = button.closest("tr");
    const statusText = row.querySelector(".status-text");

    // Update the status text and style
    if (status === "success") {
        statusText.textContent = "ناجحة";
        statusText.className = "status-text success";
    } else if (status === "waiting") {
        statusText.textContent = "في الانتظار";
        statusText.className = "status-text waiting";
    } else if (status === "fail") {
        statusText.textContent = "فاشلة";
        statusText.className = "status-text fail";
    }

    // Update the data-status attribute for filtering
    row.setAttribute("data-status", status);
}

    rows.forEach(row => {
        if (filter === "all" || row.getAttribute("data-status") === filter) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });


function filterComments() {
    const filter = document.getElementById("filter-comments").value;
    const rows = document.querySelectorAll("#comments-body tr");

    rows.forEach(row => {
        if (filter === "all" || row.getAttribute("data-type") === filter) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}