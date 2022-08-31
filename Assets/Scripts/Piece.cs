using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    
    public Color myColor;
    public Vector3 rotationPoint;
    public static int mainBoardHeight;
    public static int mainBoardWidth;
    [SerializeField] GameObject mainBoard;
    bool selected = false;
    bool mouseHovering = false;
    Vector3 mousePosition;
    Vector3 mouseScreenPosition;
    GameObject mainCamera;
    Camera mainCameraCamera;

    
    void Awake()
    {
        mainBoard = GameObject.Find("MainBoard");
        var mainBoardSize = mainBoard.transform.localScale;
        mainBoardHeight = (int)mainBoardSize.y;
        mainBoardWidth = (int)mainBoardSize.x;
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
            transform.position = new Vector3(screenPosition.x, screenPosition.y, screenPosition.z);
            Movement();
            if (Input.GetMouseButtonDown(0) && !mouseHovering)
            {
                selected = false;
                RemoveHighlight();
            }
        }
        
    }
    public void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            SpawnNewMainBoard.mainBoardGrid[roundedX, roundedY] = children;
        }
    }
    public bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            if (roundedX < mainBoard.transform.position.x - mainBoardWidth / 2 || roundedX >= mainBoard.transform.position.x + mainBoardWidth / 2 || roundedY < mainBoard.transform.position.y - mainBoardHeight / 2 || roundedY >= mainBoard.transform.position.y + mainBoardHeight / 2)
            {
                return false;
            }
            if (SpawnNewMainBoard.mainBoardGrid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1,0,0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1,0,0);
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1,0,0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1,0,0);
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0,-1,0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0,-1,0);
            }
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0,1,0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0,1,0);
            }
        } else if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
            // if (!ValidMove())
            // {
            //     transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            // }
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            // if (!ValidMove())
            // {
            //     transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
            // }
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
