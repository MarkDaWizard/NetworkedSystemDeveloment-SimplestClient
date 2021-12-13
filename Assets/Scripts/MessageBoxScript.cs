//Phu Pham
//101250748
//
//T163 - Game Programming
//GAME3110



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script for behaviour of the chat box
public class MessageBoxScript : MonoBehaviour
{
  [SerializeField]
  List<Text> textLines;

    public GameObject inputField, sendButton, connectionToClient;

   // List<Button> prefabMessages;

    private void Start()
    {
        sendButton.GetComponent<Button>().onClick.AddListener(OnSendButtonClicked);
    }

    //Add a message onto the text boxes
    public void AddChatMessage(string msg, bool fromPlayer)
    {
        //Add the new message, copy current one onto the box above
        for(int i = textLines.Count -1; i > 0; i--)
        {
            textLines[i].text = textLines[i-1].text;
            textLines[i].alignment = textLines[i-1].alignment;
        }
        textLines[0].text = msg;

        //Set alignment of text to differentiate sender/receiver
        if(fromPlayer)
        {
            textLines[0].alignment = TextAnchor.MiddleRight;
            
        }
        else
        {
            textLines[0].alignment = TextAnchor.MiddleLeft;
        }
    }

    //Send the message to the server
    void SendChatMessageToServer(string msg)
    {
        connectionToClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.ChatMessage + "," + msg);
    }

    //Send prefix message to server
    public void OnPrefixMessagePressed(string msg)
    {
        AddChatMessage(msg, true);
        SendChatMessageToServer(msg);
    }
    //Send button clicked
    void OnSendButtonClicked()
    {
        InputField input = inputField.GetComponent<InputField>();
        string msg = input.textComponent.text;
        if(msg == "")
            return;

        input.text = "";

        AddChatMessage(msg, true);
        SendChatMessageToServer(msg);
    }
    //Clear messages
    void ClearAllMessages()
    {
        foreach(Text t in textLines)
        {
            t.text = "";
        }
    }

    //Clear all messages when message box is inactive
    private void OnDisable()
    {
        if(textLines != null)
        {
            ClearAllMessages();
        }
    }
}
