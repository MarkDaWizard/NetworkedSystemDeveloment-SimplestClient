using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject submitButton, userIDText, passwordText, accountCreateCheckbox, joinButton, userLabel, passwordLabel, playerInfo, gameBoard, messageText, sendButton, messageDropDownList, chatBox, chatPanel, sendPrefixedMessageButton, observerJoinButton, replayButton, playerDropDownList;
    //,btnPlay
    GameObject messageToClientText, sendToClientButton, loginUIHolder, chatboxUIHolder, prefixedUIHolder, messagingUIHolder, joinUIHolder, replayText, replayPanel;
    public GameObject networkedClient;
    string currentPlayerName = "";
    bool isPlayer = false;
    List<string> preFixMsg = new List<string> { "hello", "test", "bye", "call you later" };
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
        playerInfo.GetComponent<Text>().text = "Logged in user: " + name;
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
        Debug.Log("replay " + msg);
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
        Debug.Log("client send " + msg);
    }
    public void SendPrefButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendPrefixMsg + "," + messageDropDownList.GetComponent<Dropdown>().options[messageDropDownList.GetComponent<Dropdown>().value].text.ToString() + "," + currentPlayerName;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log("sendpre " + msg);
    }
    public void SendButtonPressed()
    {
        string msg = ClientToServerSignifiers.SendMsg + "," + messageText.GetComponent<InputField>().text + "," + currentPlayerName;
        Debug.Log("msg:" + msg);
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log("send " + msg);
    }
    public void SubmitButtonPressed()
    {
        Debug.Log("button");
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
        //LoginSys.SetActive(false);
        //JoinSys.SetActive(false);
        joinButton.SetActive(false);
        observerJoinButton.SetActive(false);
        submitButton.SetActive(false);
        accountCreateCheckbox.SetActive(false);
        passwordText.SetActive(false);
        userIDText.SetActive(false);
        userLabel.SetActive(false);
        passwordLabel.SetActive(false);
        //btnPlay.SetActive(false);
        //txtMsg, btnSend, ddlMsg, chatBox, btnSendPrefixMsg
        //MsgSend.SetActive(false);
        messageText.SetActive(false);
        sendButton.SetActive(false);
        //PMsgSend.SetActive(false);
        messageDropDownList.SetActive(false);
        sendPrefixedMessageButton.SetActive(false);
        //C2C.SetActive(false);
        playerDropDownList.SetActive(false);
        sendToClientButton.SetActive(false);
        messageToClientText.SetActive(false);
        chatBox.SetActive(false);
        chatPanel.SetActive(false);
        replayText.SetActive(false);
        replayButton.SetActive(false);
        replayPanel.SetActive(false);
        playerInfo.SetActive(false);
        if (newState == GameStates.LoginMenu)
        {
            // LoginSys.SetActive(true);
            submitButton.SetActive(true);
            accountCreateCheckbox.SetActive(true);
            passwordText.SetActive(true);
            userIDText.SetActive(true);
            userLabel.SetActive(true);
            passwordLabel.SetActive(true);
        }
        else if (newState == GameStates.MainMenu)
        {
            playerInfo.SetActive(true);
            //JoinSys.SetActive(true);
            joinButton.SetActive(true);
            observerJoinButton.SetActive(true);
            //MsgSend.SetActive(true);
            messageText.SetActive(true);
            sendButton.SetActive(true);
            //PMsgSend.SetActive(true);
            messageDropDownList.SetActive(true);
            sendPrefixedMessageButton.SetActive(true);
            //C2C.SetActive(true);
            playerDropDownList.SetActive(true);
            sendToClientButton.SetActive(true);
            messageToClientText.SetActive(true);
            chatBox.SetActive(true);
            chatPanel.SetActive(true);

            replayText.SetActive(true);
            replayButton.SetActive(true);
            replayPanel.SetActive(true);
        }
        else if (newState == GameStates.WaitingInQueue)
        {
        }
        else if (newState == GameStates.WaitingForPlayer)
        {
            playerInfo.GetComponent<Text>().text = "waiting for player";
        }
        else if (newState == GameStates.TicTacToe)
        {
            playerInfo.SetActive(true);
            //btnPlay.SetActive(true);
            //MsgSend.SetActive(true);
            messageText.SetActive(true);
            sendButton.SetActive(true);
            //PMsgSend.SetActive(true);
            sendPrefixedMessageButton.SetActive(true);
            messageDropDownList.SetActive(true);
            //C2C.SetActive(true);
            playerDropDownList.SetActive(true);
            sendToClientButton.SetActive(true);
            messageToClientText.SetActive(true);
            chatBox.SetActive(true);
            chatPanel.SetActive(true);

            replayText.SetActive(true);
            replayButton.SetActive(true);
            replayPanel.SetActive(true);
        }
        else if (newState == GameStates.Observer)
        {
            playerInfo.SetActive(true);
            chatBox.SetActive(true);
            chatPanel.SetActive(true);
            replayText.SetActive(true);
            replayButton.SetActive(true);
            replayPanel.SetActive(true);
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