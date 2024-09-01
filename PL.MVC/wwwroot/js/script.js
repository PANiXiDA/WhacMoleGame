$(document).ready(function () {
    $('input[name="phone"]').mask("+7 (999) 999-99-99", {
        placeholder: " ",
        clearIfNotMatch: true
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const navigationItems = document.querySelectorAll('.account__navigation-item');
    const accountInfos = document.querySelectorAll('.account__info');

    if (navigationItems) {
        navigationItems.forEach(item => {
            item.addEventListener('click', function () {
                const tabId = this.getAttribute('data-acnavTab');

                navigationItems.forEach(nav => nav.classList.remove('active'));
                this.classList.add('active');

                accountInfos.forEach(info => {
                    if (info.getAttribute('data-infnavTab') === tabId) {
                        info.classList.add('account__info--display');
                    } else {
                        info.classList.remove('account__info--display');
                    }
                });
            });
        });
    }
});


