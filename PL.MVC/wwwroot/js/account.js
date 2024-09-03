new Vue({
    el: '#gameHistory',
    data: {
        games: [],
        gamesFilter: 0,
        currentPage: 1,
        totalPages: 0
    },
    mounted() {
        this.updateGames();
    },
    methods: {
        async updateGames() {
            this.games = [];
            const response = await fetch(`/Account/GetGames?filter=${this.gamesFilter}&page=${this.currentPage}`, {
                method: "GET",
            });

            const data = await response.json();
            this.totalPages = data.totalPages;

            data.games.forEach(game => {
                game.gameStartTime = this.formatDate(game.gameStartTime);
                game.gameEndTime = this.formatDate(game.gameEndTime);
                this.games.push(game);
            });
        },
        formatDate(dateString) {
            if (!dateString) return '';
            const date = new Date(dateString);
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');

            return `${day}.${month}.${year} ${hours}:${minutes}`;
        },
        goToPage(page) {
            if (page >= 1 && page <= this.totalPages) {
                this.currentPage = page;
                this.updateGames();
            }
        },
        getPaginationRange() {
            const range = [];
            const totalPages = this.totalPages;
            const currentPage = this.currentPage;
            const maxPagesToShow = 5;

            if (totalPages <= maxPagesToShow) {
                for (let i = 1; i <= totalPages; i++) {
                    range.push(i);
                }
            } else {
                if (currentPage <= 3) {
                    range.push(1, 2, 3, '...', totalPages);
                } else if (currentPage >= totalPages - 2) {
                    range.push(1, '...', totalPages - 2, totalPages - 1, totalPages);
                } else {
                    range.push(1, '...', currentPage - 1, currentPage, currentPage + 1, '...', totalPages);
                }
            }
            return range;
        }
    }
});

new Vue({
    el: '#adminUsers',
    data: {
        users: [],
        currentPage: 1,
        totalPages: 0
    },
    mounted() {
        this.updateUsers();
    },
    methods: {
        async updateUsers() {
            this.users = [];
            const response = await fetch(`/Account/GetUsers?page=${this.currentPage}`, {
                method: "GET",
            });

            const data = await response.json();
            this.totalPages = data.totalPages;

            data.users.forEach(user => {
                user.registrationDate = this.formatDate(user.registrationDate);
                this.users.push(user);
            });
        },
        formatDate(dateString) {
            if (!dateString) return '';
            const date = new Date(dateString);
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');

            return `${day}.${month}.${year} ${hours}:${minutes}`;
        },
        goToPage(page) {
            if (page >= 1 && page <= this.totalPages) {
                this.currentPage = page;
                this.updateUsers();
            }
        },
        getPaginationRange() {
            const range = [];
            const totalPages = this.totalPages;
            const currentPage = this.currentPage;
            const maxPagesToShow = 5;

            if (totalPages <= maxPagesToShow) {
                for (let i = 1; i <= totalPages; i++) {
                    range.push(i);
                }
            } else {
                if (currentPage <= 3) {
                    range.push(1, 2, 3, '...', totalPages);
                } else if (currentPage >= totalPages - 2) {
                    range.push(1, '...', totalPages - 2, totalPages - 1, totalPages);
                } else {
                    range.push(1, '...', currentPage - 1, currentPage, currentPage + 1, '...', totalPages);
                }
            }
            return range;
        },
        async Block(id) {
            const response = await fetch(`/Account/BlockAndUnblockUser?id=${id}&block=true`, {
                method: "POST",
            });

            if (response.ok) {
                Toastify({
                    text: 'The user is blocked',
                    duration: 3000,
                }).showToast();
            }

            this.updateUsers();
        },
        async Unblock(id) {
            const response = await fetch(`/Account/BlockAndUnblockUser?id=${id}&block=false`, {
                method: "POST",
            });

            if (response.ok) {
                Toastify({
                    text: 'The user is unblocked',
                    duration: 3000,
                    style: {
                        background: "green",
                        color: "white"
                    }
                }).showToast();
            }

            this.updateUsers();
        }
    }
});

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
                    text: 'The data has been successfully updated',
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

