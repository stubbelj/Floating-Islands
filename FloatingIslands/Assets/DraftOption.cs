using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DraftOption : Tile
{
    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text = gameObject.transform.Find("Description").gameObject.GetComponent<TMP_Text>();

        isUI = false;
        if(!isUI) {
            sr = gameObject.GetComponent<SpriteRenderer>();
        } else {
            img = gameObject.GetComponent<Image>();
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    new public void OnMouseDown() {
        gameManager.DraftListAdd(type);
        gameManager.Mode = "navigating";
        Destroy(transform.parent.gameObject);
    }

    new public void Init() {
        GetComponent<SpriteRenderer>().sprite = gameManager.tilePrefabs[type].GetComponent<SpriteRenderer>().sprite;
        description = gameManager.tileDescriptions[type];
    }
}
