using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraftListItem : Tile
{
    public int index;

    void Awake()
    {
        isUI = true;
        if(!isUI) {
            sr = gameObject.GetComponent<SpriteRenderer>();
        } else {
            img = gameObject.GetComponent<Image>();
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //tileArray = gameManager.tileArray;
    }

    new public void SetSprite() {
        switch(type) {
            case "blank":
                img.sprite = gameManager.tileSprites[0];
                break;
            case "grass":
                img.sprite = gameManager.tileSprites[2];
                break;
            case "house_1":
                img.sprite = gameManager.tileSprites[3];
                break;
            case "house_2":
                img.sprite = gameManager.tileSprites[4];
                break;
            case "house_3":
                img.sprite = gameManager.tileSprites[5];
                break;
            case "watermill":
                img.sprite = gameManager.tileSprites[9];
                break;
            case "wheat":
                img.sprite = gameManager.tileSprites[11];
                break;
        }
    }

    new public void OnMouseDown() {
        gameManager.Draft(type, index);
        gameManager.SelectedTile = gameObject.GetComponent<Tile>();
    }
}
