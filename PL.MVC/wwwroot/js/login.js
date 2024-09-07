var authorizationForm = document.getElementById('LoginForm');

authorizationForm.addEventListener('submit', function (event) {
    event.preventDefault();

    var formData = {
        Login: document.getElementById('Login').value,
        Password: document.getElementById('Password').value,
        Remember: document.getElementById('Remember').checked
    };

    fetch('/Account/Login', {
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

var forgotPassword = document.getElementById('ForgotPasswordForm');
forgotPassword.addEventListener('submit', function (event) {
    event.preventDefault();

    var formData = {
        Email: document.getElementById('EmailForgotPasswordForm').value,
    };

    fetch('/Account/RecoveryPassword', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    }).then(response => response.json())
        .then(data => {
            if (data.isSuccess) {
                Toastify({
                    text: 'The message has been sent to your email',
                    duration: 3000,
                    style: {
                        background: "green",
                        color: "white"
                    }
                }).showToast();
            }
            else {
                Toastify({
                    text: data.textError,
                    duration: 3000
                }).showToast();
            }
        });
});

document.getElementById('forgotPasswordBtn').addEventListener('click', function () {
    document.getElementById('forgotPasswordModal').style.display = 'block';
});

document.querySelector('.close').addEventListener('click', function () {
    document.getElementById('forgotPasswordModal').style.display = 'none';
});

window.onclick = function (event) {
    if (event.target == document.getElementById('forgotPasswordModal')) {
        document.getElementById('forgotPasswordModal').style.display = 'none';
    }
};

