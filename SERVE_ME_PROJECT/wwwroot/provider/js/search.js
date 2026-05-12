 const posts = [
    {
        title: 'استراحة ',
        description: 'وصف مختصر....',
        image: './images/geto.png',
    },
    {
        title: 'استراحة ',
        description: 'وصف مختصر....',
        image: './images/geto.png',
    },
    {
        title: 'استراحة ',
        description: 'وصف مختصر....',
        image: './images/geto.png',
    },
    {
        title: 'استراحة ',
        description: 'وصف مختصر....',
        image: './images/geto.png',
    }
];

// Function to search posts and display results
function searchPosts() {
    const query = document.getElementById('searchQuery').value.trim().toLowerCase();
    const resultsContainer = document.getElementById('resultsContainer');
    const noResults = document.getElementById('noResults');

    // Clear previous results
    resultsContainer.innerHTML = '';

    // Filter posts based on the search query
    const filteredPosts = posts.filter(post => post.title.toLowerCase().includes(query));

    // Show or hide "no results" message
    if (filteredPosts.length === 0) {
        noResults.style.display = 'block';
    } else {
        noResults.style.display = 'none';
    }

    // Display the filtered posts
    filteredPosts.forEach(post => {
        const postElement = document.createElement('div');
        postElement.classList.add('post');

        postElement.innerHTML = `
            <div class="post-image">
                <img src="${post.image}" alt="${post.title}">
            </div>
            <div class="post-content">
                <h3 class="post-title">${post.title}</h3>
                <p class="post-description">${post.description}</p>
            </div>
        `;

        resultsContainer.appendChild(postElement);
    });
}