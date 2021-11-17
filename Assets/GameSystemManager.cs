using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    
    public GameObject networkedClient;
    GameObject btnSubmit, txtUserId, txtPwd, chkCreate, btnJoin, lblU, lblP, lblInfo, gameBoard, txtMsg, btnSend, ddlMsg, chatBox, pnlChat, btnSendPrefixMsg, btnJoinObserver, btnReplay, ddlPlayer;
    //,btnPlay
    GameObject txtCMsg, btnCSend, LoginSys, MsgSend, PMsgSend, C2C, JoinSys, txtReplay, pnlReplay;
    //static GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allobjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allobjects)
        {
            if (go.name == "btnJoin")
            {
                btnJoin = go;
            }
            else if (go.name == "lblInfo")
            {
                lblInfo = go;
            }
            else if (go.name == "btnLogin")
            {
                btnSubmit = go;
            }
            else if (go.name == "txtUser")
            {
                txtUserId = go;
            }
            else if (go.name == "txtPwd")
            {
                txtPwd = go;
            }
            else if (go.name == "chkCreate")
            {
                chkCreate = go;
            }
            else if (go.name == "lblUser")
            {
                lblU = go;
            }
            else if (go.name == "lblPwd")
            {
                lblP = go;
            }
            else if (go.name == "btnReplay")
            {
                btnReplay = go;
            }
            else if (go.name == "txtReplay")
            {
                txtReplay = go;
            }
            else if (go.name == "pnlReplay")
            {
                pnlReplay = go;
            }
            //else if (go.name == "gameBoard")
            //    gameBoard = go;
            else if (go.name == "txtMsg")
                txtMsg = go;
            else if (go.name == "btnSend")
                btnSend = go;
            else if (go.name == "ddlMsg")
                ddlMsg = go;
            else if (go.name == "chatBox")
                chatBox = go;
            else if (go.name == "pnlChat")
                pnlChat = go;
            else if (go.name == "btnSendPrefixMsg")
                btnSendPrefixMsg = go;
            else if (go.name == "btnJoinObserver")
                btnJoinObserver = go;
            else if (go.name == "ddlPlayer")
            {
                ddlPlayer = go;
            }
            else if (go.name == "PMsgSend")
            {
                PMsgSend = go;
            }
            else if (go.name == "JoinSys")
            {
                JoinSys = go;
            }
            else if (go.name == "C2C")
            {
                C2C = go;
            }
            else if (go.name == "MsgSend")
            {
                MsgSend = go;
            }
            else if (go.name == "LoginSys")
            {
                LoginSys = go;
            }
            else if (go.name == "btnCSend")
            {
                btnCSend = go;
            }
            else if (go.name == "txtCMsg")
            {
                txtCMsg = go;
            }



        }
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(int newState)
    {
        //LoginSys.SetActive(false);
        //JoinSys.SetActive(false);
        btnJoin.SetActive(false);
        btnJoinObserver.SetActive(false);
        btnSubmit.SetActive(false);
        chkCreate.SetActive(false);
        txtPwd.SetActive(false);
        txtUserId.SetActive(false);
        lblU.SetActive(false);
        lblP.SetActive(false);
        //btnPlay.SetActive(false);
        //txtMsg, btnSend, ddlMsg, chatBox, btnSendPrefixMsg
        //MsgSend.SetActive(false);
        txtMsg.SetActive(false);
        btnSend.SetActive(false);
        //PMsgSend.SetActive(false);
        ddlMsg.SetActive(false);
        btnSendPrefixMsg.SetActive(false);
        //C2C.SetActive(false);
        ddlPlayer.SetActive(false);
        btnCSend.SetActive(false);
        txtCMsg.SetActive(false);
        chatBox.SetActive(false);
        pnlChat.SetActive(false);
        txtReplay.SetActive(false);
        btnReplay.SetActive(false);
        pnlReplay.SetActive(false);
        lblInfo.SetActive(false);
        if (newState == GameStates.LoginMenu)
        {
            // LoginSys.SetActive(true);
            btnSubmit.SetActive(true);
            chkCreate.SetActive(true);
            txtPwd.SetActive(true);
            txtUserId.SetActive(true);
            lblU.SetActive(true);
            lblP.SetActive(true);
        }
        else if (newState == GameStates.MainMenu)
        {
            lblInfo.SetActive(true);
            //JoinSys.SetActive(true);
            btnJoin.SetActive(true);
            btnJoinObserver.SetActive(true);
            //MsgSend.SetActive(true);
            txtMsg.SetActive(true);
            btnSend.SetActive(true);
            //PMsgSend.SetActive(true);
            ddlMsg.SetActive(true);
            btnSendPrefixMsg.SetActive(true);
            //C2C.SetActive(true);
            ddlPlayer.SetActive(true);
            btnCSend.SetActive(true);
            txtCMsg.SetActive(true);
            chatBox.SetActive(true);
            pnlChat.SetActive(true);

            txtReplay.SetActive(true);
            btnReplay.SetActive(true);
            pnlReplay.SetActive(true);
        }
        else if (newState == GameStates.WaitingInQueue)
        {
        }
        else if (newState == GameStates.WaitingForPlayer)
        {
            lblInfo.GetComponent<Text>().text = "waiting for player";
        }
        else if (newState == GameStates.TicTacToe)
        {
            lblInfo.SetActive(true);
            //btnPlay.SetActive(true);
            //MsgSend.SetActive(true);
            txtMsg.SetActive(true);
            btnSend.SetActive(true);
            //PMsgSend.SetActive(true);
            btnSendPrefixMsg.SetActive(true);
            ddlMsg.SetActive(true);
            //C2C.SetActive(true);
            ddlPlayer.SetActive(true);
            btnCSend.SetActive(true);
            txtCMsg.SetActive(true);
            chatBox.SetActive(true);
            pnlChat.SetActive(true);

            txtReplay.SetActive(true);
            btnReplay.SetActive(true);
            pnlReplay.SetActive(true);
        }
        else if (newState == GameStates.Observer)
        {
            lblInfo.SetActive(true);
            chatBox.SetActive(true);
            pnlChat.SetActive(true);
            txtReplay.SetActive(true);
            btnReplay.SetActive(true);
            pnlReplay.SetActive(true);
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