//Phu Pham
//101250748
//
//T163 - Game Programming
//GAME3110





using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;

    GameObject gameSystemManager, ticTacToeManager, chatBox;


    // Start is called before the first frame update
    void Start()
    {
        //Find and set GameObjects to corresponding variables
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach(GameObject gameObj in allObjects)
        {
            if(gameObj.GetComponent<SystemManager>() != null)
                gameSystemManager = gameObj;
            if(gameObj.GetComponent<TTTGameManager>() != null)
                ticTacToeManager = gameObj;
            if(gameObj.GetComponent<MessageBoxScript>() != null)
                chatBox = gameObj;
        }

        Connect();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    
    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.5.145", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }
    
    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    
    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
       bool a = NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg received = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');

        int signifier = int.Parse(csv[0]);

        //Account creation
        if(signifier == ServerToClientSignifiers.AccountCreated || signifier == ServerToClientSignifiers.LoginComplete)
        {
            gameSystemManager.GetComponent<SystemManager>().ChangeState(GameStates.MainMenu);
        }
        //Game starts
        else if(signifier == ServerToClientSignifiers.GameStart)
        {
            gameSystemManager.GetComponent<SystemManager>().ChangeState(GameStates.TicTacToe);
            ticTacToeManager.GetComponent<TTTGameManager>().ChangeState(TicTacToeStates.GameStart);
            ticTacToeManager.GetComponent<TTTGameManager>().SetRoomNumberText(csv[1]);
        }
        //Check if player is going first
        else if(signifier == ServerToClientSignifiers.ChosenAsPlayerOne)
        {
            ticTacToeManager.GetComponent<TTTGameManager>().IsPlayerOne();
        }
        //Opponent makes a move
        else if(signifier == ServerToClientSignifiers.OpponentAction)
        {
            ticTacToeManager.GetComponent<TTTGameManager>().OpponentMadeMove(int.Parse(csv[1]));
        }
        //Game ended
        else if(signifier == ServerToClientSignifiers.GameOver)
        {
            ticTacToeManager.GetComponent<TTTGameManager>().OnGameOver(csv[1]);
        }
        //Message handling
        else if(signifier == ServerToClientSignifiers.ChatLogMessage)
        {
            chatBox.GetComponent<MessageBoxScript>().AddChatMessage(csv[1], false);
        }
        //Join room as observer
        else if(signifier == ServerToClientSignifiers.EnteredGameRoomAsObserver)
        { 
            //Get data of previous turns
            TTTGameManager ticTackToe =  ticTacToeManager.GetComponent<TTTGameManager>();
            gameSystemManager.GetComponent<SystemManager>().ChangeState(GameStates.TicTacToe);
            ticTackToe.SetRoomNumberText(csv[1]);

            string[] takenSquares = new string[csv.Length - 2];

            for(int i = 2; i < csv.Length; i++)
            {
                takenSquares[i-2] = csv[i];
            }

            ticTackToe.EnterGameAsObserver(takenSquares);
        }
        //Get data from a turn
        else if(signifier == ServerToClientSignifiers.TurnData)
        {
            string[] turns = new string[csv.Length - 1];

            for (int i = 1; i < csv.Length; i++)
            {
                turns[i - 1] = csv[i];
            }
            ticTacToeManager.GetComponent<TTTGameManager>().SetTurnData(turns);
        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }


}


public static class ClientToServerSignifiers
{
    public const int CreateAccount = 1;
    public const int Login = 2;
    public const int JoinGameRoomQueue = 3;
    public const int TTTSquareChosen = 4;
    public const int ChatMessage = 8;
    public const int JoinAnyRoomAsObserver = 9;
    public const int JoinSpecificRoomAsObserver = 10;
    public const int EndGame = 11;
    public const int LeavingRoom = 12;
    public const int RequestTurnData = 14;
}

public static class ServerToClientSignifiers
{
    public const int LoginComplete = 1;
    public const int LoginFailed = 2;
    public const int AccountCreated = 3;
    public const int AccountCreationFailed = 4;
    public const int GameStart = 5;
    public const int ChosenAsPlayerOne = 6;
    public const int OpponentAction = 7;
    public const int ChatLogMessage = 11;
    public const int EnteredGameRoomAsObserver = 12;
    public const int GameOver = 13;
    public const int TurnData = 14;
}

