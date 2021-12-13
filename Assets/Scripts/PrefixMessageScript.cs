//Phu Pham
//101250748
//
//T163 - Game Programming
//GAME3110
using UnityEngine;
using UnityEngine.UI;
//Script for sending prefix messages
public class PrefixMessageScript : MonoBehaviour
{
    
    MessageBoxScript chatlog;
    private void Start()
    {
        chatlog = GetComponentInParent<MessageBoxScript>();
        GetComponent<Button>().onClick.AddListener(SendMessageToChat);
    }

    void SendMessageToChat()
    {
        if(chatlog != null)
        { 
            string text  = GetComponentInChildren<Text>().text;
            chatlog.OnPrefixMessagePressed(text);
        }
    }
}
