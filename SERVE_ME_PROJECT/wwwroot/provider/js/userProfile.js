function showProfile() {
    document.querySelector('.profile-container').style.display = 'block';
    document.querySelector('.change-password-container').style.display = 'none';
    document.querySelector('.orders-container').style.display = 'none';

}

function showChangePassword() {
    document.querySelector('.profile-container').style.display = 'none';
    document.querySelector('.change-password-container').style.display = 'block';
    document.querySelector('.orders-container').style.display = 'none';

}

function showOrders() {
    document.querySelector('.profile-container').style.display = 'none';
    document.querySelector('.change-password-container').style.display = 'none';
    document.querySelector('.orders-container').style.display = 'block';
}

