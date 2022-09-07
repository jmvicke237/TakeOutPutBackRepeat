using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewMainBoard : MonoBehaviour
{
    public bool recolor;
    public List<GameObject> patchworkList;
    public List<GameObject> tetrisList;
    List<List<GameObject>> listOfPieceLists = new List<List<GameObject>>();
    
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
        listOfPieceLists.Add(patchworkList);
        listOfPieceLists.Add(tetrisList);
        SpawnPieces();
    }
    void SpawnPieces()
    {
        int randomListIndex = Random.Range(0, listOfPieceLists.Count);
        List<GameObject> spawnList = listOfPieceLists[randomListIndex];
        while (spawnList.Count > 0)
        {
            int randomListElement = Random.Range(0, spawnList.Count);
            GameObject newPiece = Instantiate(spawnList[randomListElement], transform.position, Quaternion.identity);
            if (recolor)
            {
                int rand = Random.Range(0,newColors.Count);
                foreach (Transform child in newPiece.transform)
                {
                    child.gameObject.GetComponent<SpriteRenderer>().color = newColors[rand];
                }
                newPiece.GetComponent<Piece>().myColor = newColors[rand];
            }
            spawnList.RemoveAt(randomListElement);
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
