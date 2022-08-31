using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceParent : MonoBehaviour
{
    Color myColor;

    private void Awake() {
        myColor = GetComponentInChildren<SpriteRenderer>().color;
    }
    // Start is called before the first frame update
   private void OnMouseOver() {
       foreach (Transform child in transform)
        {
            Color tmp = myColor;
            tmp.a = .5f;
            child.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }
    }

    private void OnMouseExit() {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = myColor;
        }
    }

    private void OnMouseDrag() {
        
    }
}
