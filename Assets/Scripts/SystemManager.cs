//Phu Pham
//101250748
//
//T163 - Game Programming
//GAME3110
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    
    GameObject LoginButton, UsernameInputfield, PasswordInputfield, SignupToggle, ConnectionToHost, JoinButton, JoinObserverButton, TTTUI, LoginUI, roomNumInput, LeaveButton ;
    bool isNewUser = false;

    // Set GameObjects to corresponding variables
    void Start()
    {

       GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach(GameObject gameObj in allObjects)
        {
            
            if (gameObj.name == "LoginUI")
                LoginUI = gameObj;
            else if (gameObj.name == "UsernameInputfield")
                UsernameInputfield = gameObj;
            else if(gameObj.name == "PasswordInputfield")
                PasswordInputfield = gameObj;
            else if(gameObj.name == "LoginButton")
                LoginButton = gameObj;
            else if(gameObj.name == "SignupToggle")
                SignupToggle = gameObj;
            else if(gameObj.name == "JoinButton")
                JoinButton = gameObj;
            else if (gameObj.name == "JoinObserverButton")
                JoinObserverButton = gameObj;
            else if (gameObj.name == "RoomNumInputField")
                roomNumInput = gameObj;
            else if (gameObj.name == "LeaveButton")
                LeaveButton = gameObj;
            else if(gameObj.name == "TTTUI")
                TTTUI = gameObj;
            else if (gameObj.name == "NetworkClient")
                ConnectionToHost = gameObj;

        }

        //Add listener for button presses
        LoginButton.GetComponent<Button>().onClick.AddListener(LoginButtonClicked);
        JoinButton.GetComponent<Button>().onClick.AddListener(OnJoinGameRoomButtonClicked);
        SignupToggle.GetComponent<Toggle>().onValueChanged.AddListener(NewUserToggled);
        JoinObserverButton.GetComponent<Button>().onClick.AddListener(OnJoinAsObserverClicked);
        LeaveButton.GetComponent<Button>().onClick.AddListener(OnLeaveRoomButtonClicked);

        ChangeState(GameStates.LoginMenu);
    }

    //Username and Password input
    public void LoginButtonClicked()
    {
        string username = UsernameInputfield.GetComponent<InputField>().text;
        string password = PasswordInputfield.GetComponent<InputField>().text;

        string msg;
        //Check if new user
        if(isNewUser)
             msg = ClientToServerSignifiers.CreateAccount + "," + username + "," + password;
        else
            msg = ClientToServerSignifiers.Login + "," + username + "," + password;
        
        ConnectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        
    }

    //New user button click
    public void NewUserToggled(bool newValue)
    {
        isNewUser = newValue;

    }
    //Join GameRoom button
    private void OnJoinGameRoomButtonClicked()
    {
        ConnectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinGameRoomQueue + "");
        ChangeState(GameStates.WaitingInQueueForOtherPlayer);
    }
    //Join as observer button
    private void OnJoinAsObserverClicked()
    {
        InputField input = roomNumInput.GetComponent<InputField>();
        string roomNum = input.textComponent.text;

        //Check if room number is blank or not
        if(roomNum == "")
        {
            ConnectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinAnyRoomAsObserver + "");
        }
        else if(int.TryParse(roomNum, out int temp))
        {
            ConnectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinSpecificRoomAsObserver + "," + roomNum);
        }
        //Set text to blank
        input.text = "";
    }

    //Leave Room button
    void OnLeaveRoomButtonClicked()
    {
        //Check if OK to leave
        if(TTTUI.activeInHierarchy && TTTUI.GetComponent<TTTGameManager>().IsOKToLeave() == false)
            ConnectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.EndGame + "," + "Opponent Left Early");
        
        ConnectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.LeavingRoom + "");
        ChangeState(GameStates.MainMenu);
    }

    //Set visibility of gameobjects depending on current game states
    public void ChangeState(int state)
    {
        UsernameInputfield.SetActive(false);
        PasswordInputfield.SetActive(false);
        LoginButton.SetActive(false);
        SignupToggle.SetActive(false);
        JoinButton.SetActive(false);

        JoinObserverButton.SetActive(false);
        roomNumInput.SetActive(false);
        LeaveButton.SetActive(false);

        TTTUI.SetActive(false);
        LoginUI.SetActive(false);
        //Account login menu
        if (state == GameStates.LoginMenu)
        { 
            LoginUI.SetActive(true);
            LoginButton.SetActive(true);
            UsernameInputfield.SetActive(true);
            PasswordInputfield.SetActive(true);
            SignupToggle.SetActive(true);

        }
        //Join Room menu
        else if(state == GameStates.MainMenu)
        {
            LoginUI.SetActive(true);
            JoinObserverButton.SetActive(true);
            JoinButton.SetActive(true);
            roomNumInput.SetActive(true);
        }
        //Waiting menu
        else if (state == GameStates.WaitingInQueueForOtherPlayer)
        {
            LoginUI.SetActive(true);
            LeaveButton.SetActive(true);
        }
        //Tic Tac Toe Gameplay
        else if(state == GameStates.TicTacToe)
        {
            TTTUI.SetActive(true);
            TTTUI.GetComponent<TTTGameManager>().SetNetworkConnection(ConnectionToHost.GetComponent<NetworkedClient>());
            LeaveButton.SetActive(true);
        }


    }
    //Remove the listeners when GO is not active
    private void OnDisable()
    {
        if(LoginButton != null)
            LoginButton.GetComponent<Button>().onClick.RemoveAllListeners();
        if(JoinButton!= null)
            JoinButton.GetComponent<Button>().onClick.RemoveAllListeners();
        if(SignupToggle != null)
            SignupToggle.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        
    }


}


static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueueForOtherPlayer = 3;
    public const int TicTacToe = 4;
}