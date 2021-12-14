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
    
    MessageBoxScript messageBox;
    private void Start()
    {
        messageBox = GetComponentInParent<MessageBoxScript>();
        GetComponent<Button>().onClick.AddListener(SendMessageToChat);
    }

    void SendMessageToChat()
    {
        if(messageBox != null)
        { 
            string text  = GetComponentInChildren<Text>().text;
            messageBox.OnPrefixMessagePressed(text);
        }
    }
}
