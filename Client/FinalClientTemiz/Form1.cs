using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalClientTemiz
{
    public partial class Form1 : Form
    {

        // Variables to track connection status and state
        bool terminating = false;
        bool connected = false;
        bool isNamed = false;
        string currentUserName = null;
        Socket clientSocket;

        // Constructor called when the form is opened
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            InitializeComponent();
        }

        // Event handler called when the form is closing
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;

            terminating = true;

            Environment.Exit(0);
        }

        // Event handler called when the Connect button is clicked
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string IP = IpTextBox.Text;

            int portNum;

            // Try to parse the port number from the text box
            if (Int32.TryParse(PortTextBox.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);

                    // Disable the IP and Port text boxes and the Connect button
                    IpTextBox.Enabled = false;
                    PortTextBox.Enabled = false;
                    ConnectButton.Enabled = false;

                    NameTextBox.Enabled = true;
                    EnterButton.Enabled = true;

                    connected = true;

                    // Notify the user that the connection was successful
                    UpdatesRichTextBox.AppendText("Connected to the server!\n");

                    Thread receiveThread = new Thread(ReceiveClient);
                    receiveThread.Start();
                }
                catch
                {
                    // Notify the user that the connection failed
                    UpdatesRichTextBox.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                // Notify the user that the port number is invalid
                UpdatesRichTextBox.AppendText("Check the port\n");
            }
        }

        // Event handler called when the Enter button is clicked
        private void EnterButton_Click(object sender, EventArgs e)
        {
            string message = NameTextBox.Text;

            if (message != "")
            {
                if (message.Length <= 64)
                {
                    // Encode the name with a prefix "NN" and send it to the server
                    Byte[] buffer = Encoding.Default.GetBytes("NN" + message);
                    clientSocket.Send(buffer);
                }
                else
                {
                    // Notify the user that the name is too long
                    UpdatesRichTextBox.AppendText("Please enter a shorter name!\n");
                }
            }
            else
            {
                // Notify the user that the name cannot be empty
                UpdatesRichTextBox.AppendText("The name cannot be empty!\n");
            }
        }


        // instance'ın message içerisindeki 2. posizyonunu döndüren fonksiyon
        private static int FindSecondOccurrence(string message, string instance)
        {
            int firstIndex = message.IndexOf(instance);

            if (firstIndex == -1)
            {
                return -1;
            }

            int secondIndex = message.IndexOf(instance, firstIndex + instance.Length);

            return secondIndex;
        }



        private void ReceiveClient()
        {
            while (connected)
            {
                try
                {
                    // Buffer to store incoming data
                    Byte[] buffer = new Byte[128];
                    clientSocket.Receive(buffer);
                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    // Append the incoming message to the UpdatesRichTextBox
                    UpdatesRichTextBox.AppendText("_____________________\n");
                    UpdatesRichTextBox.AppendText("The incoming message from server: " + incomingMessage + "\n");

                    // Check if the name is confirmed
                    if (incomingMessage.StartsWith("NN"))
                    {
                        // If the name is for the current user
                        if (incomingMessage.Substring(FindSecondOccurrence(incomingMessage, "NN") + 2) == NameTextBox.Text)
                        {
                            currentUserName = incomingMessage.Substring(FindSecondOccurrence(incomingMessage, "NN") + 2);
                            UpdatesRichTextBox.AppendText("Welcome to server " + currentUserName + " (Win count: " + incomingMessage.Substring(2, FindSecondOccurrence(incomingMessage, "NN") - 2) + ")\n");
                            NameTextBox.Enabled = false;
                            EnterButton.Enabled = false;
                        }
                        // If the name is not for the current user
                        else
                        {
                            UpdatesRichTextBox.AppendText(incomingMessage.Substring(FindSecondOccurrence(incomingMessage, "NN") + 2) + " (Win count: " + incomingMessage.Substring(2, FindSecondOccurrence(incomingMessage, "NN") - 2) + ") joined the server.\n");
                        }
                    }

                    // If the name is already taken
                    if (incomingMessage == "NameTaken")
                    {
                        UpdatesRichTextBox.AppendText("Please enter another name. The name is taken.\n");
                    }

                    // If a new player joins the game
                    if (incomingMessage.StartsWith("JG"))
                    {
                        // If the current user joins the game
                        if (incomingMessage.Substring(2) == currentUserName)
                        {
                            LeaveTheGameButton.Enabled = true;
                        }
                        GameRichTextBox.AppendText(incomingMessage.Substring(2) + " joined the game.\n");
                    }

                    // If the game is closed
                    if (incomingMessage.StartsWith("CloseOff"))
                    {
                        LeaveTheGameButton.Enabled = false;
                        RockButton.Enabled = false;
                        PaperButton.Enabled = false;
                        ScissorsButton.Enabled = false;
                    }

                    // If the player is put in the queue
                    if (incomingMessage.StartsWith("QU"))
                    {
                        UpdatesRichTextBox.AppendText("You are in queue.\n");
                    }

                    // If countdown messages are received
                    if (incomingMessage.StartsWith("Count"))
                    {
                        GameRichTextBox.AppendText(incomingMessage.Substring(5) + "\n");
                        if (incomingMessage.Substring(5) == "Start")
                        {
                            LeaveTheGameButton.Enabled = true;
                            // ScissorsButton.Enabled = true;
                            // PaperButton.Enabled = true;
                            // RockButton.Enabled = true;
                        }
                    }

                    // If a player leaves the countdown
                    if (incomingMessage.StartsWith("LC"))
                    {
                        
                        if (incomingMessage.Substring(2) == currentUserName)
                        {
                            LeaveTheGameButton.Enabled = false;
                            
                        }
                        else if(incomingMessage.Substring(3) == currentUserName)
                        {
                            LeaveTheGameButton.Enabled = false;
                            RockButton.Enabled = false;
                            PaperButton.Enabled = false;
                            ScissorsButton.Enabled = false;
                            
                        }

                        if(incomingMessage.Substring(0,3) == "LCC")
                        {
                            GameRichTextBox.AppendText(incomingMessage.Substring(3) + " left the game.\n");
                        }
                        else
                        {
                            GameRichTextBox.AppendText(incomingMessage.Substring(2) + " left the countdown.\n");
                        }


                    }


                    if(incomingMessage == "lock")
                    {
                        LeaveTheGameButton.Enabled = false;
                        ScissorsButton.Enabled = false;
                        PaperButton.Enabled = false;
                        RockButton.Enabled = false;
                    }




                    // If a RESTART message is received
                    if (incomingMessage == "RESTART")
                    {
                        Byte[] buf = Encoding.Default.GetBytes("RESTART");
                        clientSocket.Send(buf);
                    }

                    // If game countdown messages are received
                    if (incomingMessage.StartsWith("Game"))
                    {
                        if (incomingMessage.Substring(4) == "C10")
                        {
                            GameRichTextBox.AppendText(incomingMessage.Substring(5) + "\n");
                            LeaveTheGameButton.Enabled = true;
                            ScissorsButton.Enabled = true;
                            PaperButton.Enabled = true;
                            RockButton.Enabled = true;
                        }
                        else
                        {
                            GameRichTextBox.AppendText(incomingMessage.Substring(4) + "\n");
                        }
                    }

                    // If a player is eliminated
                    if (incomingMessage.StartsWith("Eliminated"))
                    {
                        if (incomingMessage.Substring(10) == "NoOne")
                        {
                            GameRichTextBox.AppendText("No one eliminated in this round.\n");
                        }
                        else
                        {
                            GameRichTextBox.AppendText(incomingMessage.Substring(10) + " is eliminated.\n");
                        }
                    }

                    // If activation message is received
                    if (incomingMessage == "activate")
                    {
                        LeaveTheGameButton.Enabled = true;
                        RockButton.Enabled = true;
                        PaperButton.Enabled = true;
                        ScissorsButton.Enabled = true;
                    }

                    // If a winner is announced
                    if (incomingMessage.StartsWith("Winner"))
                    {
                        GameRichTextBox.AppendText(incomingMessage.Substring(6) + " wins the game.\n");
                        LeaveTheGameButton.Enabled = false;
                        RockButton.Enabled = false;
                        PaperButton.Enabled = false;
                        ScissorsButton.Enabled = false;
                    }

                }
                catch
                {
                    if (!terminating)
                    {
                        UpdatesRichTextBox.AppendText("The server has disconnected\n");

                        IpTextBox.Enabled = true;
                        PortTextBox.Enabled = true;
                        ConnectButton.Enabled = true;

                        NameTextBox.Enabled = false;
                        EnterButton.Enabled = false;
                        RockButton.Enabled = false;
                        PaperButton.Enabled = false;
                        ScissorsButton.Enabled = false;
                        LeaveTheGameButton.Enabled = false;
                        isNamed = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }
            }
        }
        private void LeaveTheGameButton_Click(object sender, EventArgs e)
        {
            // Send a message to the server indicating that the current user is leaving the game
            Byte[] buffer = Encoding.Default.GetBytes("LG" + currentUserName);
            clientSocket.Send(buffer);
        }

        private void RockButton_Click(object sender, EventArgs e)
        {
            // Append the chosen move to the GameRichTextBox
            GameRichTextBox.AppendText("You have chosen Rock\n");
            // Disable all the game buttons to prevent further input
            RockButton.Enabled = false;
            PaperButton.Enabled = false;
            ScissorsButton.Enabled = false;
            LeaveTheGameButton.Enabled = false;
            // Send the chosen move to the server
            Byte[] buffer = Encoding.Default.GetBytes("Rock" + currentUserName);
            clientSocket.Send(buffer);
        }

        private void PaperButton_Click(object sender, EventArgs e)
        {
            // Append the chosen move to the GameRichTextBox
            GameRichTextBox.AppendText("You have chosen Paper\n");
            // Disable all the game buttons to prevent further input
            RockButton.Enabled = false;
            PaperButton.Enabled = false;
            ScissorsButton.Enabled = false;
            LeaveTheGameButton.Enabled = false;
            // Send the chosen move to the server
            Byte[] buffer = Encoding.Default.GetBytes("Paper" + currentUserName);
            clientSocket.Send(buffer);
        }

        private void ScissorsButton_Click(object sender, EventArgs e)
        {
            // Append the chosen move to the GameRichTextBox
            GameRichTextBox.AppendText("You have chosen Scissors.\n");
            // Disable all the game buttons to prevent further input
            RockButton.Enabled = false;
            PaperButton.Enabled = false;
            ScissorsButton.Enabled = false;
            LeaveTheGameButton.Enabled = false;
            // Send the chosen move to the server
            Byte[] buffer = Encoding.Default.GetBytes("Scissors" + currentUserName);
            clientSocket.Send(buffer);
        }

    }
}
