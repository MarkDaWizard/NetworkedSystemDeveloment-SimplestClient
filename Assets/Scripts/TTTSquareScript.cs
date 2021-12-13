//Phu Pham
//101250748
//
//T163 - Game Programming
//GAME3110
using UnityEngine;
using UnityEngine.UI;


public class TTTSquareScript : MonoBehaviour
{
    public int row, column, ID;

    public bool diagonal1, diagonal2, isSquareTaken;
    public string icon;

    private const int maxColumns = 3;

    public delegate void SquarePressedDelegate(TTTSquareScript squarePressed);
    public event SquarePressedDelegate OnSquarePressed;

    //Set the size of current TTT grid
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        ID = (row * maxColumns) + column;
        diagonal1 = row == column;
        diagonal2 = (row + column) == maxColumns-1;
    }

    //Called when a square is clicked
    void OnClick()
    {
        if(!isSquareTaken)
            OnSquarePressed.Invoke(this);
    }

    //Claim a square, preventing interaction to it
    public void ClaimSquare(string icon)
    {
        this.icon = icon;
        isSquareTaken= true;
        GetComponentInChildren<Text>().text = icon;
    }
    //Reset a square to blank
    public void ResetSquare()
    {
        this.icon = "";
        isSquareTaken = false;
        GetComponentInChildren<Text>().text = "";
    }
}
