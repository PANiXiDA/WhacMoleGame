profileForm.addEventListener('submit', function (event) {
    event.preventDefault();

    var formData = {
        Login: document.getElementById('Login').value,
        Password: document.getElementById('Password').value,
        Email: document.getElementById('Email').value,
        PhoneNumber: document.getElementById('PhoneNumber').value
    };

    fetch('/Account/UpdateProfile', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    }).then(response => response.json())
        .then(data => {
            if (data.isSuccess) {
                Toastify({
                    text: 'Данные успешно обновлены',
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
        }).catch(error => console.log('Ошибка: ' + error.message));
});

document.getElementById('Password').addEventListener('focus', function () {
    this.removeAttribute('readonly');
    this.placeholder = '';
    this.type = 'Password';
});

document.getElementById('Password').addEventListener('blur', function () {
    if (this.value === '') {
        this.setAttribute('readonly', 'true');
        this.placeholder = '********';
        this.type = 'text';
    }
});