new Vue({
    el: '#game',
    data: {
        tiles: [],
        players: [],
        gameOver: false,
        playerLogin: '',
        moles: [],
        plants: [],
        gameId: parseInt(document.getElementById('game').getAttribute('data-game-id'), 10),
        intervalId: null
    },
    mounted() {
        this.setGame();
        this.intervalId = setInterval(this.updateGameState, 500);
    },
    methods: {
        async setGame() {
            this.playerLogin = document.getElementById('game').dataset.login;
            for (let i = 0; i < 64; i++) {
                this.tiles.push({ id: i, mole: null, plant: null });
            }
        },
        async updateGameState() {
            const response = await fetch(`/Game/GetGameState?gameId=${this.gameId}`);
            const gameState = await response.json();

            this.moles = gameState.molePositions.map(tileId => ({
                image: "/img/monty-mole.png",
                tileId
            }));

            this.plants = gameState.plantPositions.map(tileId => ({
                image: "/img/piranha-plant.png",
                tileId
            }));

            this.tiles.forEach((tile) => {
                tile.moles = this.moles.filter(mole => mole.tileId === tile.id);
                tile.plants = this.plants.filter(plant => plant.tileId === tile.id);
            });

            this.players = Object.keys(gameState.playerScores).map((playerLogin) => ({
                login: playerLogin,
                score: gameState.playerScores[playerLogin],
            }));

            this.gameOver = gameState.gameOver;

            if (this.gameOver) {
                this.gameEnd();
            }
        },
        async selectTile(playerLogin, tileId) {
            if (this.gameOver) return;

            await fetch("/Game/PlayerMove", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ playerLogin, tileId, gameId: this.gameId }),
            });
        },
        async gameEnd() {
            await fetch(`/Game/GameOver?gameId=${this.gameId}`, {
                method: "POST",
            });
            clearInterval(this.intervalId);
        }
    },
});

