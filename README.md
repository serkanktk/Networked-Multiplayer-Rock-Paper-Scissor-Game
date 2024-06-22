# Networked-Multiplayer-Rock-Paper-Scissor-Game

Welcome to the Rock-Paper-Scissors Multiplayer Game project! This project features a client-server architecture where multiple players can connect to a server and play the classic Rock-Paper-Scissors game in a networked environment.

## Introduction

This project simulates a networked multiplayer Rock-Paper-Scissors game, allowing up to 4 players to connect to a server, join a game, and compete in rounds. The server handles game logic, player management, and score tracking. The game leverages multithreading to handle multiple client connections concurrently and ensures smooth gameplay. The server maintains a leaderboard and handles various game states, including player joins, moves, and disconnections.

### Game Logic

- **Server**: Manages player connections, game state, and the leaderboard. It listens for incoming client connections, accepts them, and maintains a list of connected players. The server handles game rounds, processes player moves, and determines the winner.
- **Client**: Connects to the server, allows players to enter their names, and sends their moves (Rock, Paper, or Scissors) to the server. The client receives game updates and displays them to the player.

### Multithreading

- The server uses multiple threads to handle client connections and game logic simultaneously. A main thread listens for incoming connections, while separate threads handle communication with each connected client. This ensures that the server can manage multiple players without performance issues.

### Query Handling

- **Player Joins**: When a player joins, the client sends their name to the server. The server checks for name uniqueness and either accepts the connection or requests a different name.
- **Move Selection**: During the game, players send their move (Rock, Paper, or Scissors) to the server. The server processes these moves and determines the outcome of each round.
- **Leaderboard**: The server maintains a leaderboard that tracks player wins. This information is updated in real-time and broadcast to all connected clients.

## Features

- **Multiplayer Support**: Up to 4 players can connect and play simultaneously.
- **Client-Server Architecture**: Separate server and client modules for network communication.
- **Leaderboard**: Tracks and displays player wins.
- **Error Handling**: Robust error handling to ensure smooth gameplay.
- **User-Friendly Interface**: Easy-to-use GUI for both server and client.

## Getting Started

### Prerequisites

- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) or later
- Visual Studio 2022 or later

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/RockPaperScissorsMultiplayer.git
   cd RockPaperScissorsMultiplayer

   Open the solution file `RockPaperScissorsMultiplayer.sln` in Visual Studio.

2. Restore the NuGet packages and build the solution.

## Usage

### Server

1. Navigate to the `FinalServerTemiz` project.
2. Run the project.
3. Enter the port number in the provided text box and click the `Connect` button to start the server.
4. The server will start listening for incoming client connections.

### Client

1. Navigate to the `FinalClientTemiz` project.
2. Run the project.
3. Enter the server's IP address and port number in the provided text boxes.
4. Click the `Connect` button to connect to the server.
5. Enter your username and click the `Enter` button to join the game.
6. Use the `Rock`, `Paper`, and `Scissors` buttons to make your move when the game starts.

## Game Rules

- **Unique Names**: Each player must have a unique name.
- **Game Start**: The game starts when 4 players are connected.
- **Countdown**: A countdown is displayed before the game starts.
- **Move Selection**: Players select their move (Rock, Paper, or Scissors) within 10 seconds.
- **Winning**: The player who beats all others in the round wins. Ties result in a replay of the round.
- **Leaving the Game**: Players can leave the game at any time. The game adjusts accordingly.





![serverSS](https://github.com/serkanktk/Networked-Multiplayer-Rock-Paper-Scissor-Game/assets/128151657/38dcdfc9-4dda-4b11-b49b-a1ec73c646c3)


![clientSS](https://github.com/serkanktk/Networked-Multiplayer-Rock-Paper-Scissor-Game/assets/128151657/bee7d55a-022b-4e72-8b37-586a6c9620e8)


![clientJoinedTheGame](https://github.com/serkanktk/Networked-Multiplayer-Rock-Paper-Scissor-Game/assets/128151657/0588fb1d-e0d7-434a-a3bb-5519a27c023c)


![clientElimination](https://github.com/serkanktk/Networked-Multiplayer-Rock-Paper-Scissor-Game/assets/128151657/bd0f0450-c374-4a85-b7d7-740e01cd78d6)


