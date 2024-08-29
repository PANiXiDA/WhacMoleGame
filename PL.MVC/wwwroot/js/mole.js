let players = [
    { id: 1, color: 'brown', score: 0 },
    { id: 2, color: 'purple', score: 0 },
    { id: 3, color: 'green', score: 0 }
];

let currMoleTiles = {};
let currPlantTile;
let gameOver = false;

window.onload = function () {
    setGame();
}

function setGame() {
    for (let i = 0; i < 64; i++) {
        let tile = document.createElement("div");
        tile.id = i.toString();
        tile.addEventListener("click", selectTile);
        document.getElementById("board").appendChild(tile);
    }

    let scoresDiv = document.getElementById("scores");
    players.forEach(player => {
        let playerScore = document.createElement("div");
        playerScore.id = `score-${player.id}`;
        playerScore.innerText = `Player ${player.id} (Score: ${player.score})`;
        playerScore.style.color = player.color;
        scoresDiv.appendChild(playerScore);
    });

    players.forEach(player => {
        setInterval(() => setMole(player), 1000);
    });
    setInterval(setPlant, 2000);
}

function getRandomTile() {
    let num = Math.floor(Math.random() * 64);
    return num.toString();
}

function setMole(player) {
    if (gameOver) {
        return;
    }

    if (currMoleTiles[player.id]) {
        currMoleTiles[player.id].innerHTML = "";
    }

    let mole = document.createElement("img");
    mole.src = "/img/monty-mole.png";
    mole.classList.add(`mole-${player.color}`);

    let num = getRandomTile();
    if (currPlantTile && currPlantTile.id == num) {
        return;
    }

    currMoleTiles[player.id] = document.getElementById(num);
    currMoleTiles[player.id].appendChild(mole);
}

function setPlant() {
    if (gameOver) {
        return;
    }

    if (currPlantTile) {
        currPlantTile.innerHTML = "";
    }

    let plant = document.createElement("img");
    plant.src = "/img/piranha-plant.png";

    let num = getRandomTile();
    if (Object.values(currMoleTiles).some(tile => tile.id == num)) {
        return;
    }

    currPlantTile = document.getElementById(num);
    currPlantTile.appendChild(plant);
}

function selectTile() {
    if (gameOver) {
        return;
    }

    let playerId = Object.keys(currMoleTiles).find(key => currMoleTiles[key] === this);
    if (playerId) {
        players[playerId - 1].score += 10;
        document.getElementById(`score-${playerId}`).innerText = `Player ${playerId} (Score: ${players[playerId - 1].score})`;
    } else if (this === currPlantTile) {
        document.getElementById("scores").innerText = "GAME OVER";
        gameOver = true;
    }
}