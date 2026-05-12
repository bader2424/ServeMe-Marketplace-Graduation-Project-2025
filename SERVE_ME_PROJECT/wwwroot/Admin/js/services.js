let currentSlide = 0;
const slides = document.querySelectorAll('.slides img');
const indicators = document.querySelectorAll('.indicators div');

// Show the first slide by default
slides[currentSlide].classList.add('active');
indicators[currentSlide].classList.add('active');

// Function to show the previous slide
function prevSlide() {
    changeSlide(currentSlide - 1);
}

// Function to show the next slide
function nextSlide() {
    changeSlide(currentSlide + 1);
}

// Function to set a specific slide
function setSlide(index) {
    changeSlide(index);
}

// Function to change slides and update indicators
function changeSlide(index) {
    slides[currentSlide].classList.remove('active');
    indicators[currentSlide].classList.remove('active');

    currentSlide = (index + slides.length) % slides.length;

    slides[currentSlide].classList.add('active');
    indicators[currentSlide].classList.add('active');
}

// Complete Sale Action
function completeSale() {
    alert("تم إتمام عملية البيع بنجاح!");
}

// Complete Rental Action
function completeRental() {
    alert("تم إتمام عملية الإيجار بنجاح!");
}





function addComment() {
    const name = document.getElementById("commenterName").value;
    const rating = document.getElementById("commenterRating").value;
    const text = document.getElementById("commentText").value;

    if (name && rating && text) {
        const commentsSection = document.querySelector(".comments-section");

        // Create a new comment element
        const commentDiv = document.createElement("div");
        commentDiv.className = "comment";

        const commentHeader = document.createElement("div");
        commentHeader.className = "comment-header";
        
        const authorSpan = document.createElement("span");
        authorSpan.className = "comment-author";
        authorSpan.textContent = name;

        const ratingSpan = document.createElement("span");
        ratingSpan.className = "comment-rating";
        ratingSpan.textContent = "⭐".repeat(rating);

        commentHeader.appendChild(authorSpan);
        commentHeader.appendChild(ratingSpan);

        const commentBody = document.createElement("div");
        commentBody.className = "comment-body";
        commentBody.textContent = text;

        commentDiv.appendChild(commentHeader);
        commentDiv.appendChild(commentBody);

        // Add the new comment to the comments section
        commentsSection.appendChild(commentDiv);

        // Reset form
        document.getElementById("addCommentForm").reset();
    } else {
        alert("يرجى ملء جميع الحقول وإدخال تقييم بين 1 و 5.");
    }
}