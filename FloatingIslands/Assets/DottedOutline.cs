using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedOutline : MonoBehaviour
{
    GameManager gameManager;
        
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //cursorPosition.y -= 0.4375f;
        //transform.position = gameManager.WorldToTileCoordinates(cursorPosition);

    }
}
