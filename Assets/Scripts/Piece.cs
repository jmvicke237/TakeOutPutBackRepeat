using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Piece : MonoBehaviour
{
    //Public variables
    public Color myColor;
    public Vector3 rotationPoint;
    public static int mainBoardWidth;
    public static int mainBoardHeight;
    public static int player2BoardWidth;
    public static int player2BoardHeight;
    public static Transform[,] player2BoardGrid;
    
    //Private variables
    GameObject mainBoard;
    GameObject player1Board;
    GameObject player2Board;
    Bounds mainBoardBounds;
    Bounds player2BoardBounds;
    bool selected = false;
    bool mouseHovering = false;
    Vector3 mousePosition;
    Vector3 mouseScreenPosition;
    GameObject mainCamera;
    Camera mainCameraCamera;
    [SerializeField] bool okToPlacePlayer1 = false;
    [SerializeField] bool okToPlacePlayer2 = false;
    public bool inBoundsMainBoard = false;
    [SerializeField] bool inBoundsPlayer1 = false;
    [SerializeField] bool inBoundsPlayer2 = false;

    
    void Awake()
    {
        //Mainboard setup
        mainBoard = GameObject.Find("MainBoard");
        var mainBoardSize = mainBoard.transform.localScale;
        mainBoardWidth = (int)mainBoardSize.x;
        mainBoardHeight = (int)mainBoardSize.y;
        mainBoardBounds = new Bounds(mainBoard.transform.position, new Vector3(mainBoardWidth, mainBoardHeight, 0));
        
        //Playerboard1 setup
        player1Board = GameObject.Find("Player1Board");
        var player1BoardSize = player1Board.transform.localScale;
        Player1Board.player1BoardWidth = (int)player1BoardSize.x;
        Player1Board.player1BoardHeight = (int)player1BoardSize.y;
        Player1Board.player1BoardBounds = new Bounds(player1Board.transform.position, new Vector3(Player1Board.player1BoardWidth, Player1Board.player1BoardHeight, 0));
        Player1Board.player1BoardGrid = new Transform[(int)Player1Board.player1BoardWidth + 1, (int)Player1Board.player1BoardHeight + 1];
        
        //Playerboard1 setup
        player2Board = GameObject.Find("Player2Board");
        var player2BoardSize = player2Board.transform.localScale;
        player2BoardWidth = (int)player2BoardSize.x;
        player2BoardHeight = (int)player2BoardSize.y;
        player2BoardBounds = new Bounds(player2Board.transform.position, new Vector3(player2BoardWidth, player2BoardHeight, 0));
        player2BoardGrid = new Transform[player2BoardWidth + 1, player2BoardHeight + 1];

        //Color stuff
        myColor = GetComponentInChildren<SpriteRenderer>().color;
        
        mainCamera = GameObject.Find("Main Camera");
        mainCameraCamera = mainCamera.GetComponent<Camera>();
        
        TextMeshPro myText = gameObject.GetComponent<TextMeshPro>();
        myText.fontSize = 8;
        myText.alignment = TextAlignmentOptions.Center;
        myText.text = transform.childCount.ToString();
        myText.fontStyle = FontStyles.Underline;

    }
    void Update()
    {
        if (selected)
        {
            mouseScreenPosition = mainCameraCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 screenPosition = new Vector3(Mathf.RoundToInt(mouseScreenPosition.x), Mathf.RoundToInt(mouseScreenPosition.y),this.transform.position.z);
            Vector3 tempPosition = transform.position;
            transform.position = new Vector3(screenPosition.x, screenPosition.y, screenPosition.z);

            //Check player1board in bounds
            foreach (Transform children in transform)
            {
                if (children.transform.position.x < Player1Board.player1BoardMinX || children.transform.position.x > Player1Board.player1BoardMaxX || children.transform.position.y < Player1Board.player1BoardMinY || children.transform.position.y > Player1Board.player1BoardMaxY)
                {
                    inBoundsPlayer1 = false;
                    break;
                } else
                {
                    inBoundsPlayer1 = true;
                }
            }
            //Check player2board in bounds
            foreach (Transform children in transform)
            {
                if (children.transform.position.x < Player2Board.player2BoardMinX || children.transform.position.x > Player2Board.player2BoardMaxX || children.transform.position.y < Player2Board.player2BoardMinY || children.transform.position.y > Player2Board.player2BoardMaxY)
                {
                    inBoundsPlayer2 = false;
                    break;
                } else
                {
                    inBoundsPlayer2 = true;
                }
            }
            //Check mainboard in bounds
            foreach (Transform children in transform)
            {
                if (children.transform.position.x < mainBoard.transform.position.x - mainBoardWidth / 2 || children.transform.position.x > mainBoard.transform.position.x + mainBoardWidth / 2 || children.transform.position.y < mainBoard.transform.position.y - mainBoardHeight / 2 || transform.position.y >= mainBoard.transform.position.y + mainBoardHeight / 2)
                {
                    inBoundsMainBoard = false;
                    break;
                } else
                {
                    inBoundsMainBoard = true;
                }
            }

            if (inBoundsPlayer1)
            {
                if (!PlaceIsFreePlayer1Board())
                {
                    okToPlacePlayer1 = false;
                } else
                {
                    okToPlacePlayer1 = true;
                }
                if (!okToPlacePlayer1)
                {
                    transform.position = tempPosition;
                }
            } else
            {
                okToPlacePlayer1 = false;
            }
            if (inBoundsPlayer2)
            {
                if (!PlaceIsFreePlayer2Board())
                {
                    okToPlacePlayer2 = false;
                } else
                {
                    okToPlacePlayer2 = true;
                }
                if (!okToPlacePlayer2)
                {
                    transform.position = tempPosition;
                }
            } else
            {
                okToPlacePlayer2 = false;
            }
            
            Movement();
            if (Input.GetMouseButtonDown(0) && !mouseHovering)
            {
                selected = false;
                RemoveHighlight();
            }
        }
    }

    bool PlaceIsFreePlayer1Board()
    {
        foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(player1Board.transform.position.x - (Player1Board.player1BoardWidth / 2));
            int offsetY = Mathf.RoundToInt(player1Board.transform.position.y - (Player1Board.player1BoardHeight / 2));

            //if (Player1Board.player1BoardGrid[roundedX - offsetX, roundedY - offsetY] != null)

            if (Player1Board.player1BoardGrid[roundedX + (-1 * offsetX), roundedY + (-1 * offsetY)] != null)
            {
                return false;
            }
        }
        return true;
    }

    bool PlaceIsFreePlayer2Board()
    {
        foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(player2Board.transform.position.x - (Player2Board.player2BoardWidth / 2));
            int offsetY = Mathf.RoundToInt(player2Board.transform.position.y - (Player2Board.player2BoardHeight / 2));
            
            if (Player2Board.player2BoardGrid[roundedX - offsetX, roundedY + (-1 * offsetY)] != null)
            {
                return false;
            }
        }
        return true;
    }
    public bool ValidMoveMainBoard()
    {
        if (!InBoundsMainBoard())
        {
            return false;
        }

        if (!PlaceIsFreeMainBoard())
        {
            return false;
        }
        return true;

       /* foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            
            //Check if inside board
            if (roundedX < mainBoard.transform.position.x - mainBoardWidth / 2 || roundedX > mainBoard.transform.position.x + mainBoardWidth / 2 || roundedY < mainBoard.transform.position.y - mainBoardHeight / 2 || roundedY >= mainBoard.transform.position.y + mainBoardHeight / 2)
            {
                return false;
            }
        }
        foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            
            if (SpawnNewMainBoard.mainBoardGrid[roundedX - offsetX, roundedY - offsetY] != null) //Check if space is occupied
            {
                return false;
            }
            }
        return true;*/
    }
    public bool InBoundsMainBoard()
    {
        foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            
            //Check if inside board
            if (roundedX < mainBoard.transform.position.x - mainBoardWidth / 2 || roundedX > mainBoard.transform.position.x + mainBoardWidth / 2 || roundedY < mainBoard.transform.position.y - mainBoardHeight / 2 || roundedY >= mainBoard.transform.position.y + mainBoardHeight / 2)
            {
                return false;
            }
        }
        return true;
    }

    public bool PlaceIsFreeMainBoard()
    {
        foreach (Transform children in transform)
        {
            //Prep variables
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            
            //Check if inside board
            if (SpawnNewMainBoard.mainBoardGrid[roundedX - offsetX, roundedY - offsetY] != null) //Check if space is occupied
            {
                return false;
            }
        }
        return true;
    }   
    public void AddToPlayer1BoardGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(player1Board.transform.position.x - (Player1Board.player1BoardWidth / 2));
            int offsetY = Mathf.RoundToInt(player1Board.transform.position.y - (Player1Board.player1BoardHeight / 2));
            Player1Board.player1BoardGrid[roundedX + (-1 * offsetX), roundedY + (-1 * offsetY)] = children;
            var tmpx = roundedX + (-1 * offsetX);
            var tmpy = roundedY + (-1 * offsetY);
            Debug.Log(tmpx + "," + tmpy);
        }
    }

    public void RemoveFromPlayer1BoardGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(player1Board.transform.position.x - (Player1Board.player1BoardWidth / 2));
            int offsetY = Mathf.RoundToInt(player1Board.transform.position.y - (Player1Board.player1BoardHeight / 2));
            Player1Board.player1BoardGrid[roundedX + (-1 * offsetX), roundedY + (-1 * offsetY)] = null;
        }
    }

    public void AddToPlayer2BoardGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(player2Board.transform.position.x - (Player2Board.player2BoardWidth / 2));
            int offsetY = Mathf.RoundToInt(player2Board.transform.position.y - (Player2Board.player2BoardHeight / 2));
            Player2Board.player2BoardGrid[roundedX - offsetX, roundedY + (-1 * offsetY)] = children;
            var tmpx = roundedX - offsetX;
            var tmpy = roundedY + (-1 * offsetY);
            Debug.Log(tmpx + "," + tmpy);
        }
    }

    public void RemoveFromPlayer2BoardGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(player2Board.transform.position.x - (Player2Board.player2BoardWidth / 2));
            int offsetY = Mathf.RoundToInt(player2Board.transform.position.y - (Player2Board.player2BoardHeight / 2));
            Player2Board.player2BoardGrid[roundedX - offsetX, roundedY + (-1 * offsetY)] = null;
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
    
    public void RemoveFromMainBoardGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            int offsetX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int offsetY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            int foo = roundedY - offsetY;
            SpawnNewMainBoard.mainBoardGrid[roundedX - offsetX, roundedY - offsetY] = null;
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
        if (selected)
        {
            if (inBoundsPlayer1)
            {
                if (okToPlacePlayer1)
                {
                    AddToPlayer1BoardGrid();
                }
            } else if (inBoundsPlayer2)
            {
                if (okToPlacePlayer2)
                {
                    AddToPlayer2BoardGrid();
                }
            }
        } else if (!selected)
        {
            if (inBoundsPlayer1)
            {
                RemoveFromPlayer1BoardGrid();
            } else if (inBoundsPlayer2)
            {
                RemoveFromPlayer2BoardGrid();
            } else if (inBoundsMainBoard)
            {
                RemoveFromMainBoardGrid();
            }
        }
        selected = !selected;
    }
    void Movement(){
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
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
