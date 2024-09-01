document.addEventListener('DOMContentLoaded', function () {
    new Vue({
        el: '#game',
        data() {
            return {
                tiles: [],
                players: [],
                gameOver: false,
            };
        },
        created() {
            this.setGame();
            setInterval(this.updateGameState, 1000);
        },
        methods: {
            async setGame() {
                // Initialize the tiles array
                for (let i = 0; i < 64; i++) {
                    this.tiles.push({ id: i, mole: false, plant: false });
                }
            },
            async updateGameState() {
                try {
                    // Fetch game state from the GameController
                    const response = await fetch("/Game/GetGameState");
                    const gameState = await response.json();

                    // Update the tiles based on gameState
                    this.tiles.forEach((tile) => {
                        tile.mole = tile.id === gameState.molePosition;
                        tile.plant = tile.id === gameState.plantPosition;
                    });

                    // Update players and their scores
                    this.players = Object.keys(gameState.playerScores).map((playerId) => ({
                        id: Number(playerId),
                        score: gameState.playerScores[playerId],
                    }));

                    this.gameOver = gameState.gameOver;
                } catch (error) {
                    console.error('Ошибка при обновлении состояния игры:', error);
                }
            },
            async selectTile(tileId) {
                if (this.gameOver) return;

                const playerId = 1; // Use actual player ID from your game logic
                try {
                    await fetch("/Game/PlayerMove", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify({ playerId, tileId }),
                    });

                    this.updateGameState(); // Optionally update the state immediately after the move
                } catch (error) {
                    console.error('Ошибка при выборе плитки:', error);
                }
            },
        },
    });
});
