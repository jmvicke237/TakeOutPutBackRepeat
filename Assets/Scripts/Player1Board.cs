using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Board : MonoBehaviour
{
    public static float player1BoardWidth;
    public static float player1BoardHeight;
    public static float player1BoardMinX;
    public static float player1BoardMaxX;
    public static float player1BoardMinY;
    public static float player1BoardMaxY;
    public static Transform[,] player1BoardGrid;
    public static Bounds player1BoardBounds;

    private void Awake() {
        var player1BoardSize = transform.localScale;
        player1BoardWidth = player1BoardSize.x;
        player1BoardHeight = player1BoardSize.y;
        player1BoardMinX = transform.position.x - (player1BoardWidth / 2);
        player1BoardMaxX = transform.position.x + (player1BoardWidth / 2);
        player1BoardMinY = transform.position.y - (player1BoardHeight / 2);
        player1BoardMaxY = transform.position.y + (player1BoardHeight / 2);
        player1BoardGrid = new Transform[(int)player1BoardWidth + 1, (int)player1BoardHeight + 1];
    }
}
