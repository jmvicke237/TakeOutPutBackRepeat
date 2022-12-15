using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewMainBoard : MonoBehaviour
{
    public bool recolor;
    public Color playerOneColor;
    public Color playerTwoColor;
    public Color neutralColor;
    public List<Color> pieceColors;
    // public List<GameObject> patchworkList;
    // public List<GameObject> tetrisList;
    public List<GameObject> playerOnePieces;
    public List<GameObject> playerTwoPieces;
    public List<GameObject> neutralPieces;

    List<List<GameObject>> listOfPieceLists = new List<List<GameObject>>();
    List<GameObject> listOfPieces = new List<GameObject>();
    
    public static Transform[,] mainBoardGrid;
    public List<Color> newColors;
    GameObject mainBoard;
    int mainBoardHeight;
    int mainBoardWidth;
    private void Start() {
        mainBoard = GameObject.Find("MainBoard");
        Vector2 mainBoardSize = mainBoard.transform.localScale;
        mainBoardWidth = (int)mainBoardSize.x;
        mainBoardHeight = (int)mainBoardSize.y;
        mainBoardGrid = new Transform[mainBoardWidth + 1, mainBoardHeight + 1];
        // listOfPieceLists.Add(patchworkList);
        // listOfPieceLists.Add(tetrisList);
        listOfPieceLists.Add(playerOnePieces);
        foreach (GameObject piece in playerOnePieces)
        {
            piece.gameObject.GetComponent<Piece>().pieceKind = 1;
        }
        listOfPieceLists.Add(playerTwoPieces);
        foreach (GameObject piece in playerTwoPieces)
        {
            piece.gameObject.GetComponent<Piece>().pieceKind = 2;
        }
        listOfPieceLists.Add(neutralPieces);
        foreach (GameObject piece in neutralPieces)
        {
            piece.gameObject.GetComponent<Piece>().pieceKind = 0;
        }
        foreach (List<GameObject> list in listOfPieceLists)
        {
            foreach (GameObject piece in list)
            {
                listOfPieces.Add(piece);
            }
        }
        
        SpawnPieces();
    }
    void SpawnPieces()
    {
        // int randomListIndex = Random.Range(0, listOfPieceLists.Count);
        // List<GameObject> spawnList = listOfPieceLists[randomListIndex];
        while (listOfPieces.Count > 0)
        {
            int randomListElement = Random.Range(0, listOfPieces.Count);
            GameObject newPiece = Instantiate(listOfPieces[randomListElement], transform.position, Quaternion.identity);
            foreach (Transform child in newPiece.transform)
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = pieceColors[newPiece.GetComponent<Piece>().pieceKind];
            }
            newPiece.GetComponent<Piece>().myColor = pieceColors[newPiece.GetComponent<Piece>().pieceKind];
            // if (recolor)
            // {
            //     int rand = Random.Range(0,newColors.Count);
            //     foreach (Transform child in newPiece.transform)
            //     {
            //         child.gameObject.GetComponent<SpriteRenderer>().color = newColors[rand];
            //     }
            //     newPiece.GetComponent<Piece>().myColor = newColors[rand];
            // }
            listOfPieces.RemoveAt(randomListElement);
            int randomRotation = Random.Range(0,4);
            newPiece.transform.RotateAround(transform.TransformPoint(newPiece.GetComponent<Piece>().rotationPoint), new Vector3(0,0,1), randomRotation * 90);
            int tryX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
            int tryY = Mathf.RoundToInt(mainBoard.transform.position.y - (mainBoardHeight / 2));
            tryY++;
            newPiece.transform.position = new Vector3(tryX,tryY,0);
            bool blocked = true;
            while (blocked)
            {
                while (!newPiece.GetComponent<Piece>().ValidMoveMainBoard())
                {
                    tryX += 1;
                    if (tryX >= Mathf.RoundToInt(mainBoard.transform.position.x + (mainBoardWidth / 2)))
                    {
                        tryX = Mathf.RoundToInt(mainBoard.transform.position.x - (mainBoardWidth / 2));
                        tryY += 1;
                    }
                    newPiece.transform.position = new Vector3(tryX,tryY,0);
                } 
                blocked = false;
            }
            newPiece.GetComponent<Piece>().AddToMainBoardGrid();
            newPiece.GetComponent<Piece>().inBoundsMainBoard = true;
        }        
    }
}
