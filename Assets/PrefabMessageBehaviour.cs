using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabMessageBehaviour : MonoBehaviour
{

    ChatBoxBehaviour chatlog;
    private void Start()
    {
        chatlog = GetComponentInParent<ChatBoxBehaviour>();
        GetComponent<Button>().onClick.AddListener(SendMessageToChat);
    }

    void SendMessageToChat()
    {
        if(chatlog != null)
        { 
            string text  = GetComponentInChildren<Text>().text;
            chatlog.OnPrefabMessagePressed(text);
        }
    }
}
