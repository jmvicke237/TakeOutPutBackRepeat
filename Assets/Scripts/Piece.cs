using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //Public variables
    public Color myColor;
    public Vector3 rotationPoint;
    public static int mainBoardHeight;
    public static int mainBoardWidth;
    public static int player1BoardHeight;
    public static int player1BoardWidth;
    public static Transform[,] player1BoardGrid;
    
    //Private variables
    GameObject mainBoard;
    GameObject player1Board;
    bool selected = false;
    bool mouseHovering = false;
    Vector3 mousePosition;
    Vector3 mouseScreenPosition;
    GameObject mainCamera;
    Camera mainCameraCamera;

    
    void Awake()
    {
        //Mainboard setup
        mainBoard = GameObject.Find("MainBoard");
        var mainBoardSize = mainBoard.transform.localScale;
        mainBoardHeight = (int)mainBoardSize.y;
        mainBoardWidth = (int)mainBoardSize.x;
        
        //Playerboard1 setup
        player1Board = GameObject.Find("Player1Board");
        var player1BoardSize = player1Board.transform.localScale;
        player1BoardHeight = (int)player1BoardSize.y;
        player1BoardWidth = (int)player1BoardSize.x;
        player1BoardGrid = new Transform[player1BoardWidth, player1BoardHeight];

        //Color stuff
        myColor = GetComponentInChildren<SpriteRenderer>().color;
        
        mainCamera = GameObject.Find("Main Camera");
        mainCameraCamera = mainCamera.GetComponent<Camera>();
    }
    void Update()
    {
        if (selected)
        {
            mouseScreenPosition = mainCameraCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 screenPosition = new Vector3(Mathf.RoundToInt(mouseScreenPosition.x), Mathf.RoundToInt(mouseScreenPosition.y),this.transform.position.z);
            Vector3 tempPosition = transform.position;
            transform.position = new Vector3(screenPosition.x, screenPosition.y, screenPosition.z);
            Movement();
            if (Input.GetMouseButtonDown(0) && !mouseHovering)
            {
                selected = false;
                RemoveHighlight();
            }
        }
    }
    public bool ValidMoveMainBoard()
    {
        foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            
            //Check if inside board
            if (roundedX < mainBoard.transform.position.x - mainBoardWidth / 2 || roundedX >= mainBoard.transform.position.x + mainBoardWidth / 2 || roundedY < mainBoard.transform.position.y - mainBoardHeight / 2 || roundedY >= mainBoard.transform.position.y + mainBoardHeight / 2)
            {
                return false;
            }

            //Check if cell is occupied
            if (SpawnNewMainBoard.mainBoardGrid[roundedX - offsetX, roundedY - offsetY] != null)
            {
                return false;
            }
        }
        return true;
    }   
    public void AddToPlayer1BoardGrid()
    {
        if (transform.position.x < player1Board.transform.position.x + (player1BoardWidth / 2) && transform.position.x >= player1Board.transform.position.x - (player1BoardWidth / 2) && transform.position.y < player1Board.transform.position.y + (player1BoardHeight / 2) && transform.position.y >= player1Board.transform.position.y - (player1BoardHeight / 2))
        {
            foreach (Transform children in transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);
                int offsetX = Mathf.RoundToInt(player1Board.transform.position.x - (player1BoardWidth / 2));
                int offsetY = Mathf.RoundToInt(player1Board.transform.position.y - (player1BoardHeight / 2));
                player1BoardGrid[roundedX + (-1 * offsetX), roundedY + (-1 * offsetY)] = children;
            }
        }
    }
    public void AddToMainBoardGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            int foo = roundedY - offsetY;
            SpawnNewMainBoard.mainBoardGrid[roundedX - offsetX, roundedY - offsetY] = children;
        }
    }
    private void OnMouseOver() {
       HighlightPieces();
       mouseHovering = true;
    }
    private void OnMouseExit() {
        if (!selected)
        {
            RemoveHighlight();
        }
        mouseHovering = false;
    }
    private void OnMouseDown() {
        selected = !selected;
    }
    void Movement(){
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
        }
    }
    void HighlightPieces()
    {
        foreach (Transform child in transform)
        {
            Color tmp = myColor;
            tmp.a = .5f;
            child.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }
    }
    void RemoveHighlight()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = myColor;
        }
    }
}
