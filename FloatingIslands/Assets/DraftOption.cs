using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DraftOption : MonoBehaviour
{
    GameManager gameManager;
    System.Random r;
    SpriteRenderer sr;
    TMP_Text text;
    public string type;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        text = gameObject.transform.Find("Description").gameObject.GetComponent<TMP_Text>();
    }

    public void SetSprite() {
        switch(type) {
            case "house_1":
                sr.sprite = gameManager.tileSprites[3];
                text.text = "Generates 1 population per cycle.";
                break;
            case "house_2":
                sr.sprite = gameManager.tileSprites[4];
                text.text = "Generates 2 population per cycle.";
                break;
            case "house_3":
                sr.sprite = gameManager.tileSprites[5];
                text.text = "Generates 3 population per cycle.";
                break;
            case "watermill":
                sr.sprite = gameManager.tileSprites[9];
                text.text = "Generates 1 energy per cycle.";
                break;
            case "wheat":
                sr.sprite = gameManager.tileSprites[11];
                text.text = "Generates 1 population per cycle.";
                break;
        }
    }

    public void OnMouseDown() {
        gameManager.DraftListAdd(type);
        gameManager.Mode = "navigating";
        Destroy(transform.parent.gameObject);
    }
}
