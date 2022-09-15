using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    
    Camera cam;
    float camHeight;
    float camWidth;

    void Awake() {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    public void SpawnNewTile(Vector2 pos, string tileType) {
        //one tile width and length out of screen, in increments of 1.875
        GameObject.Instantiate(tilePrefab, new Vector2((camWidth / 1.875f + 1)*(1.875f)*(pos.x < 1 ? -1 : 1), (camHeight / 1.875f + 1)*(1.875f)*(pos.y < 1 ? -1 : 1)), Quaternion.identity);
    }
}
