using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    
    public GameObject networkedClient;
    
    //static GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }
   
    // Update is called once per frame
    void Update()
    {
        
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