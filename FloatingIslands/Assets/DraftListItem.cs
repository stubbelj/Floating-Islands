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

    new public void OnMouseDown() {
        gameManager.Draft(type, index);
        gameManager.SelectedTile = gameObject.GetComponent<Tile>();
    }

    new public void Init() {
        gameObject.GetComponent<Image>().sprite = gameManager.tilePrefabs[type].GetComponent<SpriteRenderer>().sprite;
    }
}
