using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftButton : MonoBehaviour
{
    GameManager gameManager;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnMouseDown() {
        if(gameManager.Population >= 1) {
            gameManager.Population -= 1;
            gameManager.Draft();
        }
    }
}
