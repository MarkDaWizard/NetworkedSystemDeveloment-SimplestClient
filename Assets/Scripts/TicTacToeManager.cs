using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeManager : MonoBehaviour
{
    GameObject playerSymbolText, turnIndicatorText, characterSelectionPanel, xButton, oButton, roomNumberText, previousButton, nextButton;

    NetworkedClient connectionToHost;
    
    List<TicTacToeSquareBehaviour> ticTacToeSquares;

    string playerSymbol, opponentSymbol;

    bool isPlayersTurn = false;
    bool isGameOver = false;
    bool isObserver = false;
    bool wasPlayerOne = false;
    int roomNumber;

    string[] turns;

    const int three = 3;

    int turnCount = 0;

    public void SetTurnData(string[] data)
    {
        turns = data;
        turnCount = data.Length;
    }


    void Awake()
    {
        ticTacToeSquares = new List<TicTacToeSquareBehaviour>(GetComponentsInChildren<TicTacToeSquareBehaviour>());

        foreach(TicTacToeSquareBehaviour square in ticTacToeSquares)
        {
            square.OnSquarePressed += OnTTTSquareClicked;
        }
        //Set gameobject to corresponding variables
        foreach(GameObject gameObj in FindObjectsOfType<GameObject>())
        {
            if(gameObj.name == "PlayerSymbolText")
                playerSymbolText = gameObj;
            else if(gameObj.name == "TurnIndicatorText")
                turnIndicatorText = gameObj;
            else if(gameObj.name == "CharacterSelection")
                characterSelectionPanel = gameObj;
            else if(gameObj.name == "X Button")
                xButton = gameObj;
            else if(gameObj.name == "O Button")
                oButton = gameObj;
            else if(gameObj.name == "RoomNumberText")
                roomNumberText = gameObj;
            else if(gameObj.name == "PreviousButton")
                previousButton = gameObj;
            else if(gameObj.name == "NextButton")
                nextButton = gameObj;

        }

        xButton.GetComponent<Button>().onClick.AddListener(XButtonPressed);
        oButton.GetComponent<Button>().onClick.AddListener(OButtonPressed);
        nextButton.GetComponent<Button>().onClick.AddListener(NextButtonPressed);
        previousButton.GetComponent<Button>().onClick.AddListener(PreviousButtonPressed);
    }

    //Player presses a square
    private void OnTTTSquareClicked(TicTacToeSquareBehaviour square)
    {
        //Check if player has selected a symbol and if it's their turn or not
        if(playerSymbol == "" || !isPlayersTurn) 
            return;
        //Claim a square
        square.ClaimSquare(playerSymbol);
        if (connectionToHost != null)
            connectionToHost.SendMessageToHost(ClientToServerSignifiers.TTTSquareChosen + "," + square.ID);
        //Set current turn to opponent's
        isPlayersTurn = false;
        turnIndicatorText.GetComponent<Text>().text = "Opponent's turn";
        //Check for win/draw condition
        WinCheck(square.row, square.column);
        DrawCheck();
    }

    //Check if win
    void WinCheck(int checkingRow, int checkingCol)
    {
        int rowCount, colCount, diagonal1Count, diagonal2Count;
        rowCount = colCount = diagonal1Count = diagonal2Count = 0;
        //Check each square in TTT
        foreach(TicTacToeSquareBehaviour square in ticTacToeSquares)
        {
            if(square.isSquareTaken == false || square.icon == opponentSymbol)
                continue;

            if(square.row == checkingRow)
                rowCount++;
            if(square.column == checkingCol)
                colCount++;
            if(square.diagonal1)
                diagonal1Count++;
            if(square.diagonal2)
                diagonal2Count++;
        }
        //Check if a row OR collumn OR diagonal has 3 same symbol
        if(rowCount == three || colCount == three || diagonal1Count == three || diagonal2Count == three)
        {
            //Player win
            OnGameOver("You Won!");
            //Opponent lost
            connectionToHost.SendMessageToHost(ClientToServerSignifiers.EndGame + "," + "You Lost");
        }
        
    }

    //Check if draw
    private void DrawCheck()
    {
        int takenTileCount = 0;
        foreach (TicTacToeSquareBehaviour s in ticTacToeSquares)
        {
            if (s.isSquareTaken)
                takenTileCount++;
        }

        if (takenTileCount >= 9 && isGameOver == false)
        {
            connectionToHost.SendMessageToHost(ClientToServerSignifiers.EndGame + "," + " Draw!");
            OnGameOver("Game Over. Draw!");
        }
    }

    //Oponent made a move
    public void OpponentMadeMove(int squareID)
    {
        //Claim the square that was clicked
        foreach(TicTacToeSquareBehaviour square in ticTacToeSquares)
        {
            if(square.ID == squareID)
                square.ClaimSquare(opponentSymbol);
        }
        //Set current turn to player's
        if(!isObserver)
        { 
            isPlayersTurn = true;
            turnIndicatorText.GetComponent<Text>().text = "Your turn";
        }
        //Add turn data to observer
        else
        {
            DisplayTurn(squareID);
        }
    }

    //Game Ended
    public void OnGameOver(string endingMsg)
    {
        //Set turn indicator to game ended
        turnIndicatorText.GetComponent<Text>().text = endingMsg;
        //Tell observer game ended
        if(isObserver)
            turnIndicatorText.GetComponent<Text>().text = "Game Over";
        //Enable replay
        ChangeState(TicTacToeStates.GameOver);
    }
    //Set network connection param
    public void SetNetworkConnection(NetworkedClient networkClient)
    {
         connectionToHost = networkClient;
    }

    //Symbol selection
    void XButtonPressed()
    {
        SymbolSelected("X", "O");
    }
    void OButtonPressed()
    {
        SymbolSelected("O", "X");
    }

    //Replay controls
    void NextButtonPressed()
    {
        if(turnCount < turns.Length)
        { 
            DisplayTurn(int.Parse(turns[turnCount]));
        }
    }

    void PreviousButtonPressed()
    {
        if(turnCount > 0)
        {
            turnCount--;
            ticTacToeSquares[int.Parse(turns[turnCount])].ResetSquare();
        }
    }

    //Set player's and opponent's symbol
    void SymbolSelected(string symbol1, string symbol2)
    {
        playerSymbol = symbol1;
        opponentSymbol = symbol2;

        playerSymbolText.GetComponent<Text>().text = "You Are: " + symbol1;

        characterSelectionPanel.SetActive(false);
        turnIndicatorText.SetActive(true);

        //Check if opponent made a move before player chose a symbol
        foreach(TicTacToeSquareBehaviour square in ticTacToeSquares)
        {
            if(square.isSquareTaken)
                square.ClaimSquare(opponentSymbol);
        }
    }

    //Player's first turn
    public void IsPlayerOne()
    {
        isPlayersTurn = true;
        turnIndicatorText.GetComponent<Text>().text = "Your turn";
        wasPlayerOne = true;
    }

    //Set game room's number
    public void SetRoomNumberText(string roomNumber)
    {
        this.roomNumber = int.Parse(roomNumber);
        roomNumberText.GetComponent<Text>().text = "Room number: " + roomNumber;
    }
    //Enter as observer
    public void EnterGameAsObserver(string[] csv_TurnsSoFar)
    {
        ChangeState(TicTacToeStates.Observing);

        //Get current match's data
        foreach(string index in csv_TurnsSoFar)
        {
            int squareIndex = int.Parse(index);
            DisplayTurn(squareIndex);
        }
        //Check if game ended
        if(isGameOver)
            ChangeState(TicTacToeStates.GameOver);
    }

    //Display the current turn for each player
    void DisplayTurn(int squareID)
    {
        //Set the symbol according to current player's
        if(wasPlayerOne)
        {
            if (turnCount++ % 2 == 0)
                ticTacToeSquares[squareID].ClaimSquare(playerSymbol);
            else
                ticTacToeSquares[squareID].ClaimSquare(opponentSymbol);
        }
        else
        {
            if (turnCount++ % 2 == 1)
                ticTacToeSquares[squareID].ClaimSquare(playerSymbol);
            else
                ticTacToeSquares[squareID].ClaimSquare(opponentSymbol);
        }
    }

    //Check if player can leave without ending the match
    public bool IsOKToLeave()
    {
        return (isObserver || isGameOver);
    }

    //Reset the game
    private void ResetGameState()
    {
        playerSymbolText.GetComponent<Text>().text = "You Are: " ;
        //Clear up all squares
        foreach (TicTacToeSquareBehaviour square in ticTacToeSquares)
        {
            square.ResetSquare();
        }
        //Reset turn indicator & turn counter
        turnIndicatorText.GetComponent<Text>().text = "It's your opponent's turn";
        turnIndicatorText.SetActive(false);
        turnCount = 0;
    }

    //Set the state of the game
    public void ChangeState(int state)
    {
        isPlayersTurn = false;
        nextButton.SetActive(false);
        previousButton.SetActive(false);
        //Game just started
        if(state == TicTacToeStates.StartingGame)
        {
            ResetGameState();
           
            isGameOver = false;
            
            characterSelectionPanel.SetActive(true);

            isObserver = false;
        }
        //Player is observer
        else if(state == TicTacToeStates.Observing)
        {
            ResetGameState();
            playerSymbol = "X";
            opponentSymbol = "O";
            characterSelectionPanel.SetActive(false);

            playerSymbolText.GetComponent<Text>().text = "You are an observer";

            isObserver = true;
        }
        //Game ended
        else if(state == TicTacToeStates.GameOver)
        {
            connectionToHost.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.RequestTurnData + "," + roomNumber);
            isGameOver = true;
            turnIndicatorText.SetActive(true);
            //Enable replay controls
            nextButton.SetActive(true);
            previousButton.SetActive(true);
        }
    }
}


public class TicTacToeStates
{
    public const int StartingGame = 1;
    public const int Observing = 2;
    public const int GameOver = 3;
}