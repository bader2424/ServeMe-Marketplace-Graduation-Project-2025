function validateForm() {
    let isValid = true;

    const title = document.getElementById('postTitle').value.trim();
    if (title === '') {
        showError('titleGroup', 'titleError', 'يرجى إدخال عنوان المنشور.');
        isValid = false;
    } else {
        hideError('titleGroup', 'titleError');
    }

    const content = document.getElementById('postContent').value.trim();
    if (content === '') {
        showError('contentGroup', 'contentError', 'يرجى إدخال محتوى المنشور.');
        isValid = false;
    } else {
        hideError('contentGroup', 'contentError');
    }

    const date = document.getElementById('publishDate').value;
    if (date === '') {
        showError('dateGroup', 'dateError', 'يرجى اختيار تاريخ النشر.');
        isValid = false;
    } else {
        hideError('dateGroup', 'dateError');
    }

    const category = document.getElementById('postCategory').value;
    if (category === '') {
        showError('categoryGroup', 'categoryError', 'يرجى اختيار القسم.');
        isValid = false;
    } else {
        hideError('categoryGroup', 'categoryError');
    }


    return isValid;
}

function showError(groupId, errorId, message) {
    document.getElementById(groupId).classList.add('error');
    document.getElementById(errorId).textContent = message;
}

function hideError(groupId, errorId) {
    document.getElementById(groupId).classList.remove('error');
    document.getElementById(errorId).textContent = '';
}
