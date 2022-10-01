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
        if(gameManager.Mode == "navigating" && gameManager.Population >= gameManager.currentDraftCost) {
            gameManager.Population -= gameManager.currentDraftCost;
            gameManager.currentDraftCost += 10;
            gameManager.Draft();
        }
    }
}
