using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Board : MonoBehaviour
{
    public static float player2BoardWidth;
    public static float player2BoardHeight;
    public static float player2BoardMinX;
    public static float player2BoardMaxX;
    public static float player2BoardMinY;
    public static float player2BoardMaxY;
    public static Transform[,] player2BoardGrid;
    public static Bounds player2BoardBounds;

    private void Awake() {
        var player2BoardSize = transform.localScale;
        player2BoardWidth = player2BoardSize.x;
        player2BoardHeight = player2BoardSize.y;
        player2BoardMinX = transform.position.x - (player2BoardWidth / 2);
        player2BoardMaxX = transform.position.x + (player2BoardWidth / 2);
        player2BoardMinY = transform.position.y - (player2BoardHeight / 2);
        player2BoardMaxY = transform.position.y + (player2BoardHeight / 2);
        player2BoardGrid = new Transform[(int)player2BoardWidth + 1, (int)player2BoardHeight + 1];
    }
}
