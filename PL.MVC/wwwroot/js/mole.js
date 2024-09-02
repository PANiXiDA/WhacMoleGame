new Vue({
    el: '#game',
    data: {
        tiles: [],
        players: [],
        gameOver: false,
        playerLogin: '',
        mole: {
            image: "/img/monty-mole.png",
            tileId: 0,
            playerId: 0
        },
        plant: {
            image: "/img/piranha-plant.png",
            tileId: 0
        }
    },
    mounted() {
        this.setGame();
        setInterval(this.updateGameState, 1000);
    },
    methods: {
        async setGame() {
            this.playerLogin = document.getElementById('game').dataset.login;
            for (let i = 0; i < 64; i++) {
                this.tiles.push({ id: i, mole: null, plant: null });
            }
        },
        async updateGameState() {
            const response = await fetch("/Game/GetGameState");
            const gameState = await response.json();

            this.mole.tileId = gameState.molePosition;
            this.plant.tileId = gameState.plantPosition;

            this.tiles.forEach((tile) => {
                tile.mole = tile.id === this.mole.tileId ? this.mole : null;
                tile.plant = tile.id === this.plant.tileId ? this.plant : null;
            });

            this.players = Object.keys(gameState.playerScores).map((playerLogin) => ({
                login: playerLogin,
                score: gameState.playerScores[playerLogin],
            }));

            this.gameOver = gameState.gameOver;
        },
        async selectTile(playerLogin, tileId) {
            if (this.gameOver) return;

            await fetch("/Game/PlayerMove", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ playerLogin, tileId }),
            });
        },
    },
});

