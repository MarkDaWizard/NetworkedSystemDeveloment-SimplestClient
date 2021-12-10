using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject chatUI, submitButton, userIDText, passwordText, accountCreateCheckbox, joinButton, userLabel, passwordLabel, playerInfo, gameBoard, messageText, sendButton, messageDropDownList, chatBox, chatPanel, sendPrefixedMessageButton, observerJoinButton, replayButton, playerDropDownList;
    //,btnPlay
    GameObject messageToClientText, sendToClientButton, loginUIHolder, chatboxUIHolder, prefixedUIHolder, messagingUIHolder, joinUIHolder, replayText, replayPanel;
    public GameObject networkedClient;
    string currentPlayerName = "";
    bool isPlayer = false;
    List<string> preFixMsg = new List<string> { ":)", ":(", "UwU", "<3" };
    //static GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
        //instance = this.gameObject;
        GameObject[] allobjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObj in allobjects)
        {
            if (gameObj.name == "joinButton")
            {
                joinButton = gameObj;
            }
            else if (gameObj.name == "playerInfo")
            {
                playerInfo = gameObj;
            }
            else if (gameObj.name == "submitButton")
            {
                submitButton = gameObj;
            }
            else if (gameObj.name == "userIDText")
            {
                userIDText = gameObj;
            }
            else if (gameObj.name == "passwordText")
            {
                passwordText = gameObj;
            }
            else if (gameObj.name == "accountCreateCheckbox")
            {
                accountCreateCheckbox = gameObj;
            }
            else if (gameObj.name == "userLabel")
            {
                userLabel = gameObj;
            }
            else if (gameObj.name == "passwordLabel")
            {
                passwordLabel = gameObj;
            }
            else if (gameObj.name == "replayButton")
            {
                replayButton = gameObj;
            }
            else if (gameObj.name == "replayText")
            {
                replayText = gameObj;
            }
            else if (gameObj.name == "replayPanel")
            {
                replayPanel = gameObj;
            }
            else if (gameObj.name == "messageText")
                messageText = gameObj;
            else if (gameObj.name == "sendButton")
                sendButton = gameObj;
            else if (gameObj.name == "messageDropDownList")
                messageDropDownList = gameObj;
            else if (gameObj.name == "chatBox")
                chatBox = gameObj;
            else if (gameObj.name == "chatPanel")
                chatPanel = gameObj;
            else if (gameObj.name == "sendPrefixedMessageButton")
                sendPrefixedMessageButton = gameObj;
            else if (gameObj.name == "observerJoinButton")
                observerJoinButton = gameObj;
            else if (gameObj.name == "playerDropDownList")
            {
                playerDropDownList = gameObj;
            }
            else if (gameObj.name == "prefixedUIHolder")
            {
                prefixedUIHolder = gameObj;
            }
            else if (gameObj.name == "joinUIHolder")
            {
                joinUIHolder = gameObj;
            }
            else if (gameObj.name == "messagingUIHolder")
            {
                messagingUIHolder = gameObj;
            }
            else if (gameObj.name == "chatboxUIHolder")
            {
                chatboxUIHolder = gameObj;
            }
            else if (gameObj.name == "loginUIHolder")
            {
                loginUIHolder = gameObj;
            }
            else if (gameObj.name == "sendToClientButton")
            {
                sendToClientButton = gameObj;
            }
            else if (gameObj.name == "messageToClientText")
            {
                messageToClientText = gameObj;
            }
            else if(gameObj.name == "ChatUIHolder")
            {
                chatUI = gameObj;
            }
            
        }
        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        joinButton.GetComponent<Button>().onClick.AddListener(JoinButtonPressed);
        observerJoinButton.GetComponent<Button>().onClick.AddListener(ObserveButtonPressed);
        replayButton.GetComponent<Button>().onClick.AddListener(ReplayButtonPressed);
        sendButton.GetComponent<Button>().onClick.AddListener(SendButtonPressed);
        sendPrefixedMessageButton.GetComponent<Button>().onClick.AddListener(SendPrefButtonPressed);
        sendToClientButton.GetComponent<Button>().onClick.AddListener(SendClientButtonPressed);
        accountCreateCheckbox.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);

        ChangeState(GameStates.LoginMenu);

        messageDropDownList.GetComponent<Dropdown>().AddOptions(preFixMsg);


    }
    public bool getIsPlayer()
    {
        return isPlayer;
    }
    public void updateChat(string msg)
    {
        chatBox.GetComponent<TMP_Text>().text += msg + "\n";
    }
    public void updateReplay(string msg)
    {
        replayText.GetComponent<TMP_Text>().text += msg + "\n";
    }
    public void updateUserName(string name)
    {
        currentPlayerName = name;
        playerInfo.GetComponent<Text>().text = "Logged in as: " + name;
    }
    public void LoadPlayer(List<string> list)
    {
        playerDropDownList.GetComponent<Dropdown>().ClearOptions();
        foreach (string it in list)
        {
            if (it.Contains(currentPlayerName))
            {
                list.Remove(it);
                break;
            }
        }
        playerDropDownList.GetComponent<Dropdown>().AddOptions(list);

    }
    public void ReplayButtonPressed()
    {
        string msg = ClientToServerSignifiers.ReplayMsg + "," + currentPlayerName;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SendClientButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendClientMsg + "," + playerDropDownList.GetComponent<Dropdown>().options[playerDropDownList.GetComponent<Dropdown>().value].text.ToString() + "," + messageToClientText.GetComponent<InputField>().text + "," + currentPlayerName;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void SendPrefButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendPrefixMsg + "," + messageDropDownList.GetComponent<Dropdown>().options[messageDropDownList.GetComponent<Dropdown>().value].text.ToString() + "," + currentPlayerName;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void SendButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendMsg + "," + messageText.GetComponent<InputField>().text + "," + currentPlayerName;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void SubmitButtonPressed()
    {
        string p = passwordText.GetComponent<InputField>().text;
        string n = userIDText.GetComponent<InputField>().text;
        string msg;
        if (accountCreateCheckbox.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccount + "," + n + "," + p;
        else
            msg = ClientToServerSignifiers.Login + "," + n + "," + p;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void JoinButtonPressed()
    {
        string msg = ClientToServerSignifiers.JoinGammeRoomQueue + "," + currentPlayerName;
        isPlayer = true;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        //  ChangeState(GameStates.TicTacToe);
    }
    public void ObserveButtonPressed()
    {
        string msg = ClientToServerSignifiers.JoinAsObserver + "," + currentPlayerName;
        isPlayer = false;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        //ChangeState(GameStates.Observer);
    }
    public void CreateToggleChanged(bool newValue)
    {

    }
    public void PlayButtonPressed()
    {
        string msg = ClientToServerSignifiers.PlayGame + "";
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        //load tictactoe
        ChangeState(GameStates.Running);
    }
    public void ChangeState(int newState)
    {
        //Wipe UI
        loginUIHolder.SetActive(false);
        chatUI.SetActive(false);
        //Login
        if (newState == GameStates.LoginMenu)
        {

            loginUIHolder.SetActive(true);
        }
        //MainMenu
        else if (newState == GameStates.MainMenu)
        {


            chatUI.SetActive(true);
        }
        //Waiting
        else if (newState == GameStates.WaitingForPlayer)
        {
            playerInfo.GetComponent<Text>().text = "Waiting for players";
        }
        //Chat
        else if (newState == GameStates.TicTacToe)
        {
            chatUI.SetActive(true);
            joinUIHolder.SetActive(false);
        }
        //Observer
        else if (newState == GameStates.Observer)
        {
            chatUI.SetActive(true);
            joinUIHolder.SetActive(false);
            prefixedUIHolder.SetActive(false);
            messagingUIHolder.SetActive(false);

        }
    }

}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueue = 3;
    public const int TicTacToe = 4;
    public const int WaitingForPlayer = 5;
    public const int Running = 6;
    public const int Observer = 7;
}