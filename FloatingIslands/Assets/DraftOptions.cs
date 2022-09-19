using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftOptions : MonoBehaviour
{
    GameManager gameManager;
    System.Random r;
    
    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start() {
        List<string> tempDraftableTileTypes = new List<string>(gameManager.draftableTileTypes);
        r = gameManager.r;
        foreach(Transform child in transform) {
            int randomInt = r.Next(tempDraftableTileTypes.Count);
            string newType = tempDraftableTileTypes[randomInt];
            tempDraftableTileTypes.RemoveAt(randomInt);
            child.gameObject.GetComponent<DraftOption>().type = newType;
            child.gameObject.GetComponent<DraftOption>().SetSprite();
        }
    }
}
