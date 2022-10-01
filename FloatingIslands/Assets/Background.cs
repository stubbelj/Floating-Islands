using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    GameManager gameManager;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnMouseDown() {
        gameManager.SelectedTile = null;
        gameManager.Mode = "navigating";
    }
}
