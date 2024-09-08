var registrationForm = document.getElementById('RegistrationForm');

registrationForm.addEventListener('submit', function (event) {
    event.preventDefault();

    var formData = {
        Login: document.getElementById('Login').value,
        Password: document.getElementById('Password').value,
        RepeatPassword: document.getElementById('RepeatPassword').value,
        Email: document.getElementById('Email').value,
        PhoneNumber: document.getElementById('PhoneNumber').value
    };

    fetch('/Account/Registration', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    }).then(response => response.json())
        .then(data => {
            if (data.isSuccess) {
                confirmationModal.style.display = 'block';
            }
            else {
                Toastify({
                    text: data.textError,
                    duration: 3000
                }).showToast();
            }
        });
});

var forgotPassword = document.getElementById('AccountConfirmationForm');
forgotPassword.addEventListener('submit', function (event) {
    event.preventDefault();

    var formData = {
        Code: document.getElementById('CodeConfirmation').value,
    };

    fetch('/Account/VerifyCodeConfirmation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    }).then(response => response.json())
        .then(data => {
            if (data.isSuccess) {
                window.location.href = '/Account/Index';
            }
            else {
                Toastify({
                    text: data.textError,
                    duration: 3000
                }).showToast();
            }
        });
});

document.querySelector('.close').addEventListener('click', function () {
    document.getElementById('confirmationModal').style.display = 'none';
});

function applyMask(input) {
    let value = input.value.replace(/\D/g, '');
    let maskedValue = '';

    for (let i = 0; i < value.length && i < 4; i++) {
        if (i > 0) maskedValue += '-';
        maskedValue += value[i];
    }

    input.value = maskedValue;
}
