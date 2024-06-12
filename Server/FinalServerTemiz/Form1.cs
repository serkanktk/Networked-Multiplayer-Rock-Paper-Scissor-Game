using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FinalServerTemiz
{
    public partial class Form1 : Form
    {

        // List to store the win counts of players
        List<(string, int)> winCounts = new List<(string, int)>();

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        List<(string, string)> roundPreferences = new List<(string, string)>();

        int leaveCount = 0;

        bool terminating = false;
        bool listening = false;

        // List to store sorted entries
        List<Tuple<int, string>> sortedEntries = new List<Tuple<int, string>>();

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        bool countdownCancelled = false;

        bool gameInProgress = false;

        volatile bool countdownInProgress = false;

        int numberOfPlayersInGame = 0;

        // List to store user information
        List<User> users = new List<User>();

        // Function called when the form is opened
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            this.FormClosing += new FormClosingEventHandler(Form1Close);

            InitializeComponent();
        }

        // Function called when the form is closed
        private void Form1Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Set the flags to stop listening and terminate the server
            listening = false;
            terminating = true;

            sortedEntries.Clear();

            Environment.Exit(0);
        }

        // Function called when the Connect button is clicked to bind to a port
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Port button clicked!");

            int serverPort;

            // Try to parse the port number from the textbox
            if (Int32.TryParse(PortTextBox.Text, out serverPort))
            {
                // Bind the server socket to the specified port
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);
                listening = true;
                ConnectButton.Enabled = false;

                // Start a new thread to accept incoming connections
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                ServerRichTextBox.AppendText("Started listening on port: " + serverPort + "\n");
            }
            else
            {
                ServerRichTextBox.AppendText("Please check port number \n");
            }
        }

        // Function to update the GameRichTextBox with the win counts
        private void updateGameRichTextBox(List<(string, int)> winCounts)
        {
            LeaderboardRichTextBox.Clear();

            // Append each player's win count in the formatted manner
            foreach (var item in winCounts)
            {
                LeaderboardRichTextBox.AppendText($"{item.Item1} {item.Item2}{Environment.NewLine}");
            }
        }


        // Function to accept clients and connect them to the server
        private void Accept()
        {
            while (listening)
            {
                try
                {
                    // Accept a new client connection
                    Socket newClient = serverSocket.Accept();

                    // Append status message to the ServerRichTextBox
                    ServerRichTextBox.AppendText("A client is connected.\n");

                    // Create a new User object for the client
                    User currentUser = new User();
                    currentUser.userSocket = newClient;
                    users.Add(currentUser);

                    // Start a new thread to receive messages from the client
                    Thread receiveThread = new Thread(() => ReceiveMessage(currentUser));
                    receiveThread.Start();
                }
                catch
                {
                    // If terminating, stop listening
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        // Append error message if the socket stops working
                        ServerRichTextBox.AppendText("The socket stopped working.\n");
                    }
                }
            }
        }

        // Function to sort and format win counts
        private static List<(string, int)> SortAndFormatWinCounts(List<(string, int)> winCounts)
        {
            // Sort the list in descending order based on the integer value and return it
            return winCounts.OrderByDescending(item => item.Item2).ToList();
        }

        // Function to check if the name is approved, returns true if approved, false otherwise
        private bool isNameApproved(User currentUser, string incomingMessage)
        {
            foreach (User user in users)
            {
                // If the name already exists, return false
                if (incomingMessage.Substring(2) == user.name)
                {
                    return false;
                }
            }

            // If the name is unique, return true
            return true;
        }

        // Function to send a message to all connected clients
        private void sendMessageToEveryone(string message)
        {
            foreach (User user in users)
            {
                Byte[] bufferClient = Encoding.Default.GetBytes(message);
                user.userSocket.Send(bufferClient);
            }
        }

        // Function to send a message to a specific user
        private void sendMessageToCurrentUser(User user, string message)
        {
            Byte[] bufferClient = Encoding.Default.GetBytes(message);
            user.userSocket.Send(bufferClient);
        }

        // Function to add a user to the countdown
        private void addToCountdown(User currentUser)
        {
            Thread.Sleep(2000);
            currentUser.inCountdown = true;
            currentUser.queueNo = 0;
            numberOfPlayersInGame++;
            sendMessageToEveryone("JG" + currentUser.name);
        }

        // Function to add a user to the queue
        private void addToQueue(User currentUser)
        {
            int max = 0;
            foreach (User user in users)
            {
                // Find the maximum queue number
                if (user.queueNo >= max)
                {
                    max = user.queueNo;
                }
            }

            // Assign the new user the next queue number
            currentUser.queueNo = max + 1;
            ServerRichTextBox.AppendText(currentUser.name + " is queued: " + currentUser.queueNo + "\n");
            sendMessageToCurrentUser(currentUser, "QU");
        }



        // This function reads the txt file and returns the win count of the given user
        private static int returnGivenUsersWinCount(string txtPath, string userName)
        {
            // Read all lines from the text file
            string[] lines = File.ReadAllLines(txtPath);

            // Check each line
            foreach (string line in lines)
            {
                // Split the line into parts by space
                string[] parts = line.Split(' ');
                // If the line has two parts and the username matches
                if (parts.Length == 2 && parts[0] == userName)
                {
                    // Return the win count if it is a valid integer
                    if (int.TryParse(parts[1], out int winCount))
                    {
                        return winCount;
                    }
                    else
                    {
                        throw new FormatException("The win count is not a valid integer.");
                    }
                }
            }

            // Return -1 if the username is not found
            return -1;
        }

        // This function appends a user with a zero win count to the txt file
        private static void AppendUserWithZeroWinCount(string txtFilePath, string userName)
        {
            // Use StreamWriter to append the user and their win count to the file
            using (StreamWriter sw = File.AppendText(txtFilePath))
            {
                sw.WriteLine($"{userName} 0");
            }
        }


        // Main function for receiving messages
        private void ReceiveMessage(User currentUser)
        {
            bool connected = true;
            bool isInitialized = false;

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[128];
                    currentUser.userSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    ServerRichTextBox.AppendText("___________\n");
                    ServerRichTextBox.AppendText("The incoming message: " + incomingMessage + "\n");

                    // If the player just joined the server and the message starts with "NN"
                    if (!isInitialized && incomingMessage.StartsWith("NN"))
                    {
                        // If the name is approved
                        if (isNameApproved(currentUser, incomingMessage))
                        {
                            currentUser.name = incomingMessage.Substring(2);

                            int userWinnings = returnGivenUsersWinCount(@"E:\repos\FinalServerTemiz\leaderboard.txt", currentUser.name);

                            // If the default value is returned, mark the new user's win count as 0 in the txt file
                            if (userWinnings == -1)
                            {
                                AppendUserWithZeroWinCount(@"E:\repos\FinalServerTemiz\leaderboard.txt", currentUser.name);
                                userWinnings = 0;
                            }

                            sendMessageToEveryone("NN" + userWinnings + "NN" + currentUser.name);

                            winCounts.Add((currentUser.name, userWinnings));

                            winCounts = SortAndFormatWinCounts(winCounts);

                            updateGameRichTextBox(winCounts);

                            Thread.Sleep(1000);
                            // Add to the game if there is no countdown in progress and no game in progress
                            if (!countdownInProgress && !gameInProgress)
                            {
                                // Don't check for numberOfPlayers == 4 because the player count is less than 4, but we might be in a game
                                addToCountdown(currentUser);
                            }
                            // Add to the queue
                            else
                            {
                                addToQueue(currentUser);
                            }
                            isInitialized = true;
                        }
                        // If the name is not approved
                        else
                        {
                            sendMessageToCurrentUser(currentUser, "NameTaken");
                        }
                    }

                    // If the player clicked the LeaveTheGame button
                    if (incomingMessage.Substring(0, 2) == "LG")
                    {
                        User userSentMessage = null;
                        foreach (User user in users)
                        {
                            if (incomingMessage.Substring(2) == user.name)
                            {
                                userSentMessage = user;
                                if (user.inCountdown)
                                {
                                    numberOfPlayersInGame--;
                                    countdownInProgress = false;
                                    user.inCountdown = false;
                                    countdownCancelled = true;
                                    cancellationTokenSource.Cancel();
                                    sendMessageToEveryone("LC" + incomingMessage.Substring(2));
                                    addToQueue(user);
                                }
                                else if (user.inGame)
                                {
                                    roundPreferences.Add((incomingMessage.Substring(2), "LeaveTheGame" + leaveCount.ToString()));
                                    leaveCount++;

                                    // Send a message
                                    sendMessageToEveryone("LCC" + incomingMessage.Substring(2));
                              
                                    
                                }
                            }
                        }
                    }

                    // If a new game needs to start
                    if (!countdownInProgress && !gameInProgress && numberOfPlayersInGame == 4)
                    {
                        countdownInProgress = true;
                        GameRichTextBox.AppendText("For new game, number of players: " + numberOfPlayersInGame + "\n");
                        // Start the countdown
                        Thread startCountdown = new Thread(() => StartCountdown());
                        startCountdown.Start();
                    }

                    // Handle incoming game choices
                    if (incomingMessage.StartsWith("Paper"))
                    {
                        string userName = incomingMessage.Substring(5);
                        roundPreferences.Add((incomingMessage.Substring(5), "Paper"));
                    }
                    else if (incomingMessage.StartsWith("Rock"))
                    {
                        string userName = incomingMessage.Substring(4);
                        roundPreferences.Add((incomingMessage.Substring(4), "Rock"));
                    }
                    else if (incomingMessage.StartsWith("Scissors"))
                    {
                        string userName = incomingMessage.Substring(8);
                        roundPreferences.Add((incomingMessage.Substring(8), "Scissors"));
                    }

                    // If no countdown or game is in progress, and the number of players in the game is less than 4, and countdown was cancelled
                    // Then add the first player in the queue to the countdown and start the countdown
                    if (!countdownInProgress && !gameInProgress && numberOfPlayersInGame < 4 && countdownCancelled)
                    {
                        addToQueue();
                        while (numberOfPlayersInGame < 4)
                        {
                            bool processGoing = true;
                            while (processGoing)
                            {
                                int min = 100;
                                foreach (User user in users)
                                {
                                    if (user.inCountdown == false && user.inGame == false && user.queueNo != 0 && user.queueNo < min)
                                    {
                                        addToCountdown(user);
                                        ServerRichTextBox.AppendText(user.name + " is added to countdown\n");
                                    }
                                }
                                if (min == 100)
                                {
                                    processGoing = false;
                                }
                            }
                        }
                        ServerRichTextBox.AppendText("_______NUMBER OF PLAYERS IN THE GAME: " + numberOfPlayersInGame + "\n");
                        Thread.Sleep(2500);
                        if (numberOfPlayersInGame == 4)
                        {
                            countdownInProgress = true;
                            GameRichTextBox.AppendText("2: For new game, number of players: " + numberOfPlayersInGame + "\n");
                            // Start the countdown
                            Thread startCountdown = new Thread(() => StartCountdown());
                            startCountdown.Start();
                        }
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        ServerRichTextBox.AppendText("A client has disconnected\n");
                    }
                    currentUser.userSocket.Close();
                    users.Remove(currentUser);
                    connected = false;
                }
            }
        }

        // Function to increment the win count of a user in the text file
        private static void incrementWinCount(string filePath, string userName)
        {
            // Read the file contents
            var lines = File.ReadAllLines(filePath).ToList();

            bool userFound = false;

            // Check each line and find the user
            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(' ');
                if (parts.Length == 2 && parts[0] == userName)
                {
                    if (int.TryParse(parts[1], out int winCount))
                    {
                        // Find the user and increment the count
                        lines[i] = $"{userName} {winCount + 1}";
                        userFound = true;
                        break;
                    }
                    else
                    {
                        throw new FormatException("The win count is not a valid integer.");
                    }
                }
            }

            // If the user is not found, add a new entry for the user
            if (!userFound)
            {
                lines.Add($"{userName} 1");
            }

            // Write the updated contents back to the file
            File.WriteAllLines(filePath, lines);
        }


        private void StartCountdown()
        {
            try
            {
                // Countdown messages
                string[] countdown = { "Count5", "Count4", "Count3", "Count2", "Count1", "CountStart" };
                for (int i = 0; i < countdown.Length; i++)
                {
                    // If cancellation is requested, stop the countdown
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        ServerRichTextBox.AppendText("Countdown iptal edildi.\n");
                        cancellationTokenSource = new CancellationTokenSource();
                        return; // Exit the loop and end the method
                    }

                    // Send countdown message to players
                    sendMessageToPlayerInCountdown(countdown[i]);
                    Thread.Sleep(1000);
                }

                countdownInProgress = false;
                gameInProgress = true;

                bool noWinner = true;
                List<User> usersInGame = new List<User>();
                foreach (User user in users)
                {
                    // Move users from countdown to game
                    if (user.inCountdown)
                    {
                        user.inCountdown = false;
                        user.inGame = true;
                        usersInGame.Add(user);
                    }
                }

                List<string> eliminatedUsers = new List<string>();
                // Game countdown setup
                while (noWinner)
                {
                    try
                    {
                        // Game countdown messages
                        string[] gameCountdown = { "Game10", "Game9", "Game8", "Game7", "Game6", "Game5", "Game4", "Game3", "Game2", "Game1" };
                        for (int i = 0; i < gameCountdown.Length; i++)
                        {
                            if (i == 0)
                            {
                                List<User> eliminatedOnes = new List<User>();
                                List<User> continueOnes = new List<User>();
                                foreach (User user in users)
                                {
                                    // Separate eliminated and continuing players
                                    if (user.inGame && !eliminatedUsers.Contains(user.name))
                                    {
                                        continueOnes.Add(user);
                                    }
                                    else if (user.inGame && eliminatedUsers.Contains(user.name))
                                    {
                                        eliminatedOnes.Add(user);
                                    }
                                }
                                // Send appropriate message to each group
                                sendMessageInList(eliminatedOnes, gameCountdown[i]);
                                sendMessageInList(continueOnes, "GameC10");
                            }
                            else
                            {
                                sendMessageToPlayerInGame(gameCountdown[i]);
                            }

                            Thread.Sleep(1000);

                            // After "Game1" message, check player choices and eliminate as necessary
                            if (i == gameCountdown.Length - 1)
                            {
                                foreach (User user in users)
                                {
                                    if (user.inGame && !roundPreferences.Any(rp => rp.Item1 == user.name) && !eliminatedUsers.Contains(user.name))
                                    {
                                        roundPreferences.Add((user.name, "NoChoice"));
                                        
                                    }
                                    else if (user.inGame && !roundPreferences.Any(rp => rp.Item1 == user.name) && eliminatedUsers.Contains(user.name))
                                    {
                                        Byte[] bufferClient = Encoding.Default.GetBytes("lock");
                                        user.userSocket.Send(bufferClient);
                                    }

                                }


                                List<string> eliminatedInThisRound = decisionForRound(roundPreferences, eliminatedUsers);
                                if (eliminatedInThisRound == null)
                                {
                                    sendMessageToPlayerInGame("EliminatedNoOne");
                                }
                                else
                                {
                                    eliminatedUsers.AddRange(eliminatedInThisRound);
                                    foreach (string name in eliminatedInThisRound)
                                    {
                                        sendMessageToPlayerInGame("Eliminated" + name);
                                        Thread.Sleep(100);
                                    }

                                    // If three players are eliminated, announce the winner
                                    if (eliminatedUsers.Count == 3)
                                    {
                                        foreach (User user in users)
                                        {
                                            if (user.inGame && !eliminatedUsers.Contains(user.name))
                                            {
                                                sendMessageToEveryone("Winner" + user.name);

                                                winCounts = incrementAndSortWinCount(user.name, winCounts);

                                                updateGameRichTextBox(winCounts);

                                                incrementWinCount(@"E:\repos\FinalServerTemiz\leaderboard.txt", user.name);

                                                Thread.Sleep(100);
                                            }
                                        }

                                        numberOfPlayersInGame -= 4;
                                        gameInProgress = false;
                                        foreach (User user in users)
                                        {
                                            if (user.inGame)
                                            {
                                                user.queueNo = 0;
                                                user.inGame = false;
                                            }
                                        }

                                        cancellationTokenSource.Cancel();

                                        if (cancellationTokenSource.IsCancellationRequested)
                                        {
                                            ServerRichTextBox.AppendText("Oyun bitti.\n");
                                            cancellationTokenSource = new CancellationTokenSource();

                                            ServerRichTextBox.AppendText("NEDEN GIRMEDI:\n");
                                            ServerRichTextBox.AppendText("countdownInProgress: " + countdownInProgress + "\n");
                                            ServerRichTextBox.AppendText("gameInProgress: " + gameInProgress + "\n");
                                            ServerRichTextBox.AppendText("numberOfPlayersInGame: " + numberOfPlayersInGame + "\n");

                                            countdownCancelled = true;

                                            Byte[] bufferClient = Encoding.Default.GetBytes("RESTART");
                                            users[0].userSocket.Send(bufferClient);

                                            roundPreferences.Clear();
                                            return; // Exit the loop and end the method
                                        }
                                    }
                                }
                                roundPreferences.Clear();
                            }
                        }
                    }
                    catch (Exception ex) // Catch general exceptions
                    {
                        // Handle exceptions, e.g., logging, informing the user, etc.
                    }
                }
            }
            catch (OperationCanceledException)
            {
                ServerRichTextBox.AppendText("Operasyon iptal edildi\n");
            }
        }


        // send message to users who are in userList parameter
        private void sendMessageInList(List<User> usersList, string message)
        {
            foreach (User user in usersList)
            {
                Byte[] bufferClient = Encoding.Default.GetBytes(message);
                user.userSocket.Send(bufferClient);
            }
        }

        // send message to every player in game
        private void sendMessageToPlayerInGame(string message)
        {
            foreach (User user in users)
            {
                if (user.inGame)
                {
                    try
                    {
                        Byte[] bufferClient = Encoding.Default.GetBytes(message);
                        user.userSocket.Send(bufferClient);
                    }
                    catch (Exception ex)
                    {
                        // error handling
                        ServerRichTextBox.AppendText("Error sending message to " + user.name + ": " + ex.Message);
                    }
                }
            }
        }

        // send message to every player in countdown
        private void sendMessageToPlayerInCountdown(string message)
        {
            foreach (User user in users)
            {
                if (user.inCountdown)
                {
                    Byte[] bufferClient = Encoding.Default.GetBytes(message);
                    user.userSocket.Send(bufferClient);
                }

            }
        }


        private List<string> decisionForRound(List<(string, string)> roundPreferences, List<string> eliminatedUsers)
        {
            List<string> eliminatedPlayers = new List<string>();


            int numberOfPlayers = roundPreferences.Count;
            int count = roundPreferences.Count;
            int noChoiceCount = roundPreferences.Count(x => x.Item2.StartsWith("NoChoice"));

            // Handle scenarios where everyone has "NoChoice"
            if ((noChoiceCount == 4 && roundPreferences.Count == 4) ||
                (noChoiceCount == 3 && roundPreferences.Count == 3) ||
                (noChoiceCount == 2 && roundPreferences.Count == 2))
            {
                return null;
            }

            // Handle scenarios where everyone chose "LeaveTheGame"
            int leavingCount = roundPreferences.Count(x => x.Item2.StartsWith("LeaveTheGame"));

            int countExtra = count - (noChoiceCount + leavingCount);
            
            // exceptional cases
            if (((countExtra == 0) && ((noChoiceCount == 3 && leavingCount == 1) || (noChoiceCount == 2 && leavingCount == 2) || (noChoiceCount == 1 && leavingCount == 3)))
                || ((numberOfPlayers == 3) && ((noChoiceCount == 1 && leavingCount == 2) || (noChoiceCount == 2 && leavingCount == 1)))
                || ((numberOfPlayers == 2) && (noChoiceCount == 1 && leavingCount == 1)))
            {
                ServerRichTextBox.AppendText("içinde\n");
                eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2.StartsWith("LeaveTheGame")).Select(x => x.Item1));
                return eliminatedPlayers;
            }

            ServerRichTextBox.AppendText("_______________\n");
            ServerRichTextBox.AppendText("countExtra: " + countExtra + "\n");
            ServerRichTextBox.AppendText("noChoiceCount: " + noChoiceCount + "\n");
            ServerRichTextBox.AppendText("leavingCount: " + leavingCount + "\n");
            ServerRichTextBox.AppendText("_______________\n");



            // If everyone chose to leave the game
            if ((leavingCount == 4 && roundPreferences.Count == 4) ||
                (leavingCount == 3 && roundPreferences.Count == 3) ||
                (leavingCount == 2 && roundPreferences.Count == 2))
            {
                // Find the last player who chose to leave the game
                User lastUser = new User();
                int maxNumber = 0;
                foreach (var preference in roundPreferences)
                {
                    if (int.Parse(preference.Item2.Substring(12)) > maxNumber)
                    {
                        maxNumber = int.Parse(preference.Item2.Substring(12));
                    }
                }

                // Identify the last player who chose to leave the game
                foreach (var preference in roundPreferences)
                {
                    if (int.Parse(preference.Item2.Substring(12)) == maxNumber)
                    {
                        string userName = preference.Item1;
                        foreach (User user in users)
                        {
                            if (user.name == userName)
                            {
                                lastUser = user;
                            }
                        }
                    }
                }

                // Add all other players who chose "LeaveTheGame" to the eliminated list
                foreach (var preference in roundPreferences)
                {
                    if (preference.Item2.StartsWith("LeaveTheGame") && preference.Item1 != lastUser.name)
                    {
                        eliminatedPlayers.Add(preference.Item1);
                    }
                }

                return eliminatedPlayers;
            }

            // Add players with "NoChoice" and adjust count
            eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "NoChoice").Select(x => x.Item1));
            count -= noChoiceCount;
            // Add players who chose "LeaveTheGame" and adjust count
            eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2.StartsWith("LeaveTheGame")).Select(x => x.Item1));
            count -= leavingCount;

            // Debugging information
            ServerRichTextBox.AppendText("_______________\n");
            ServerRichTextBox.AppendText("count: " + count + "\n");
            ServerRichTextBox.AppendText("noChoiceCount: " + noChoiceCount + "\n");
            ServerRichTextBox.AppendText("leavingCount: " + leavingCount + "\n");
            foreach (var item in roundPreferences)
            {
                ServerRichTextBox.AppendText($"{item.Item1} {item.Item2}{Environment.NewLine}");
            }
            ServerRichTextBox.AppendText("_______________\n");


            


            // Handle different scenarios based on the count of remaining choices
            if (count == 4)
            {
                int rockCount = roundPreferences.Count(x => x.Item2 == "Rock");
                int paperCount = roundPreferences.Count(x => x.Item2 == "Paper");
                int scissorsCount = roundPreferences.Count(x => x.Item2 == "Scissors");

                // All players made the same choice
                if (rockCount == 4 || paperCount == 4 || scissorsCount == 4)
                {
                    return null;
                }
                // Three players made the same choice
                else if (rockCount == 3 && paperCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Rock").Select(x => x.Item1));
                }
                else if (rockCount == 3 && scissorsCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Scissors").Select(x => x.Item1));
                }
                else if (paperCount == 3 && scissorsCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Paper").Select(x => x.Item1));
                }
                else if (paperCount == 3 && rockCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Rock").Select(x => x.Item1));
                }
                else if (scissorsCount == 3 && rockCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Scissors").Select(x => x.Item1));
                }
                else if (scissorsCount == 3 && paperCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Paper").Select(x => x.Item1));
                }
                // Two players made one choice, two made another
                else if (rockCount == 2 && paperCount == 2)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Rock").Select(x => x.Item1));
                }
                else if (rockCount == 2 && scissorsCount == 2)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Scissors").Select(x => x.Item1));
                }
                else if (paperCount == 2 && scissorsCount == 2)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Paper").Select(x => x.Item1));
                }
                // Two players made one choice, one made each of the others
                else if (rockCount == 2 && paperCount == 1 && scissorsCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 != "Rock").Select(x => x.Item1));
                }
                else if (paperCount == 2 && rockCount == 1 && scissorsCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 != "Paper").Select(x => x.Item1));
                }
                else if (scissorsCount == 2 && rockCount == 1 && paperCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 != "Scissors").Select(x => x.Item1));
                }
            }
            else if (count == 3)
            {
                int rockCount = roundPreferences.Count(x => x.Item2 == "Rock");
                int paperCount = roundPreferences.Count(x => x.Item2 == "Paper");
                int scissorsCount = roundPreferences.Count(x => x.Item2 == "Scissors");

                // All players made the same choice
                if (rockCount == 3 || paperCount == 3 || scissorsCount == 3)
                {
                    return null;
                }
                // Two players made the same choice, one made a different choice
                else if (rockCount == 2 && scissorsCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Scissors").Select(x => x.Item1));
                }
                else if (rockCount == 2 && paperCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Rock").Select(x => x.Item1));
                }
                else if (paperCount == 2 && rockCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Rock").Select(x => x.Item1));
                }
                else if (paperCount == 2 && scissorsCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Paper").Select(x => x.Item1));
                }
                else if (scissorsCount == 2 && paperCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Paper").Select(x => x.Item1));
                }
                else if (scissorsCount == 2 && rockCount == 1)
                {
                    eliminatedPlayers.AddRange(roundPreferences.Where(x => x.Item2 == "Scissors").Select(x => x.Item1));
                }
                // All players made unique choices
                else if (rockCount == 1 && paperCount == 1 && scissorsCount == 1)
                {
                    return null;
                }
            }
            else if (count == 2)
            {
                var player1 = roundPreferences[0];
                var player2 = roundPreferences[1];

                // Determine the outcome based on choices
                if (player1.Item2 == player2.Item2)
                {
                    // Both players made the same choice; no one is eliminated
                }
                else if ((player1.Item2 == "Rock" && player2.Item2 == "Scissors") ||
                         (player1.Item2 == "Scissors" && player2.Item2 == "Paper") ||
                         (player1.Item2 == "Paper" && player2.Item2 == "Rock"))
                {
                    // Player 1 wins, player 2 is eliminated
                    eliminatedPlayers.Add(player2.Item1);
                }
                else
                {
                    // Player 2 wins, player 1 is eliminated
                    eliminatedPlayers.Add(player1.Item1);
                }
            }

            return eliminatedPlayers;
        }

        // This method checks if a given username exists in the provided list of tuples
        bool ContainsUserName(List<(string, string)> listToCheck, string checkItem)
        {
            foreach (var item in listToCheck)
            {
                if (item.Item1.Contains(checkItem))
                {
                    return true;
                }
            }
            return false;
        }

        // This method increments the win count for a given username and sorts the list in descending order of win counts
        private static List<(string, int)> incrementAndSortWinCount(string userName, List<(string, int)> winCounts)
        {
            // Increment the win count for the specified username
            for (int i = 0; i < winCounts.Count; i++)
            {
                if (winCounts[i].Item1 == userName)
                {
                    winCounts[i] = (winCounts[i].Item1, winCounts[i].Item2 + 1);
                    break;
                }
            }

            // Sort the list in descending order based on the win count
            return winCounts.OrderByDescending(item => item.Item2).ToList();
        }

        // This method assigns a queue number to users who do not already have one
        private void addToQueue()
        {
            int max = 0;

            // Find the highest queue number among the users
            foreach (User user in users)
            {
                if (user.queueNo > max)
                {
                    max = user.queueNo;
                }
            }

            int i = 1;

            // Assign a new queue number to users who do not already have one
            foreach (User user in users)
            {
                if (user.queueNo == 0)
                {
                    user.queueNo += max + i;
                }
                i++;
            }
        }


    }



    public class User
    {
        public string lastChoice { get; set; } = null;
        public string name { get; set; } = null;
        public bool inGame { get; set; } = false;
        public bool inCountdown { get; set; } = false;
        public int queueNo { get; set; } = 0;
        public Socket userSocket { get; set; } = null;

        public User()
        {

        }
    }


}
