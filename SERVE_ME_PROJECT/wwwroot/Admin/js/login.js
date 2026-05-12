const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});

// Form Validation Functionality
const forms = document.querySelectorAll('form');

forms.forEach(form => {
    form.addEventListener('submit', function(event) {
        event.preventDefault();
        const inputs = form.querySelectorAll('input, select, textarea');
        let valid = true;

        inputs.forEach(input => {
            const errorSpan = input.parentElement.querySelector('.error-message');
            if (!input.value.trim()) {
                errorSpan.textContent = 'This field is required.';
                input.parentElement.classList.add('error');
                valid = false;
            } else {
                errorSpan.textContent = '';
                input.parentElement.classList.remove('error');
            }

            // Additional validation for email
            if (input.type === 'email') {
                const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailPattern.test(input.value.trim())) {
                    errorSpan.textContent = 'Please enter a valid email address.';
                    input.parentElement.classList.add('error');
                    valid = false;
                }
            }

            // Additional validation for phone
            if (input.type === 'tel') {
                const phonePattern = /^[0-9]{10,15}$/;
                if (!phonePattern.test(input.value.trim())) {
                    errorSpan.textContent = 'Please enter a valid phone number (10-15 digits).';
                    input.parentElement.classList.add('error');
                    valid = false;
                }
            }
        });

        if (valid) {
            alert("Form submitted successfully!");
            form.reset();
        } else {
            alert("Please fill in all fields correctly.");
        }
    });
});