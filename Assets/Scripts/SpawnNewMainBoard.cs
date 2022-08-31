using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewMainBoard : MonoBehaviour
{
    //Spawn a piece
    //Move the piece around the board until I find a valid placement
    //Leave piece at valid spot
    //Spawn a new piece
    //public GameObject[] polyominoes;
    public List<GameObject> polyominoesList;

    GameObject mainBoard;
    int mainBoardHeight;
    int mainBoardWidth;
    public static Transform[,] mainBoardGrid;
    private void Start() {
        mainBoard = GameObject.Find("MainBoard");
        var mainBoardSize = mainBoard.transform.localScale;
        mainBoardHeight = (int)mainBoardSize.y;
        mainBoardWidth = (int)mainBoardSize.x;
        mainBoardGrid = new Transform[mainBoardHeight, mainBoardHeight];
        SpawnPieces();
    }
    void SpawnPieces()
    {
        while (polyominoesList.Count > 0)
        {
            int randomListElement = Random.Range(0, polyominoesList.Count);
            GameObject newPiece = Instantiate(polyominoesList[randomListElement], transform.position, Quaternion.identity);
            polyominoesList.RemoveAt(randomListElement);
            int randomRotation = Random.Range(0,4);
            newPiece.transform.RotateAround(transform.TransformPoint(newPiece.GetComponent<Piece>().rotationPoint), new Vector3(0,0,1), 90 * randomRotation);
            Piece newPieceScript = newPiece.GetComponent<Piece>();
            int tryX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int tryY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            newPiece.transform.position = new Vector3(tryX,tryY,0);
            bool blocked = true;
            while (blocked)
            {
                if (!newPiece.GetComponent<Piece>().ValidMove())
                {
                    tryX += 1;
                    if (tryX > Mathf.RoundToInt(mainBoard.transform.position.x + (mainBoardWidth / 2)))
                    {
                        tryX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
                        tryY += 1;
                    }
                    newPiece.transform.position = new Vector3(tryX,tryY,0);
                } else
                {
                    blocked = false;
                }
            }
            foreach (Transform children in newPiece.transform)
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);

                mainBoardGrid[roundedX, roundedY] = children;
            }
        }        
    }
}
