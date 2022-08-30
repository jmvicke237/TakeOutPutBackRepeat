using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector3 rotationPoint;
    public static int mainBoardHeight;
    public static int mainBoardWidth;
    public static Transform[,] grid;

    [SerializeField] GameObject mainBoard;
    // Start is called before the first frame update
    void Start()
    {
        mainBoard = GameObject.Find("MainBoard");
        var mainBoardSize = mainBoard.transform.localScale;
        mainBoardHeight = (int)mainBoardSize.y;
        mainBoardWidth = (int)mainBoardSize.x;
        grid = new Transform[mainBoardHeight, mainBoardHeight];
    }

    // Update is called once per frame
    void Update()
    {
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
                AddToGrid();
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
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            }
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), -90);
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < mainBoard.transform.position.x - mainBoardWidth / 2 || roundedX >= mainBoard.transform.position.x + mainBoardWidth / 2 || roundedY < mainBoard.transform.position.y - mainBoardHeight / 2 || roundedY >= mainBoard.transform.position.y + mainBoardHeight / 2)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }

            /*if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }*/
        }

        return true;
    }
}
