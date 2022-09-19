using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftOption : MonoBehaviour
{
    GameManager gameManager;
    System.Random r;
    SpriteRenderer sr;
    public string type;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite() {
        switch(type) {
            case "house_1":
                sr.sprite = gameManager.tileSprites[3];
                break;
            case "house_2":
                sr.sprite = gameManager.tileSprites[4];
                break;
            case "house_3":
                sr.sprite = gameManager.tileSprites[5];
                break;
            case "watermill":
                sr.sprite = gameManager.tileSprites[9];
                break;
            case "wheat":
                sr.sprite = gameManager.tileSprites[11];
                break;
        }
    }

    public void OnMouseDown() {
        gameManager.Mode = "placing";
        gameManager.currentPlaceableTile = type;
        Destroy(transform.parent.gameObject);
    }
}
