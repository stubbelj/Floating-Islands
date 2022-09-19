using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tilePrefab;
    float tileWidth = 0.8745f;
    float tileHeight = 0.8745f;
    Camera cam;
    float camHeight;
    float camWidth;

    SpriteRenderer sr;
    GameManager gameManager;
    public bool selected = false;
    public Tile[] neighbors = {null, null, null, null};
    public string type = "blank";
    bool occupied = false;
    public int priority = 0;

    public (int, int) coords = (0, 0);
    public int x;
    public int y;
    Tile[,] tileArray;

    // Start is called before the first frame update
    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tileArray = gameManager.tileArray;
    }

    void Update() {
        x = coords.Item1;
        y = coords.Item2;
    }


    public void OnMouseDown() {
        if (type != "blank") {
            gameManager.SelectedTile = gameObject.GetComponent<Tile>();
        } else if (gameManager.Mode == "placing" && !occupied) {
            gameManager.SelectedTile = null;
            gameObject.GetComponent<SpriteRenderer>().sprite = gameManager.tileSprites[1];
            StartCoroutine(SpawnNewTile(gameManager.currentPlaceableTile));
            gameManager.Mode = "navigating";
        }
    }

    public IEnumerator SpawnNewTile(string newType) {
        occupied = true;
        Tile newTile = GameObject.Instantiate(tilePrefab, new Vector2(transform.position.x + (neighbors[2] != null && neighbors[2].type != "blank" || neighbors[3] != null && neighbors[3].type != "blank" ? 1 : - 1)*5, transform.position.y + (neighbors[1] != null && neighbors[1].type != "blank" || neighbors[2] != null && neighbors[2].type != "blank" ? 1 : - 1)*5), Quaternion.identity).GetComponent<Tile>();
        newTile.sr.sortingOrder = 10;
        newTile.Init(newType);
        newTile.coords = coords;

        while(newTile.transform.position != transform.position) {
            newTile.transform.position = Vector2.MoveTowards(newTile.transform.position, transform.position, 0.05f);
            yield return new WaitForSeconds(0.05f);
        }
        SpawnAdjacentBlanks();
        newTile.neighbors = Neighbors(coords);
        tileArray[coords.Item1,coords.Item2] = newTile;
        newTile.sr.sortingOrder = 0;
        newTile.BeginProduction();
        Destroy(gameObject);
    }

    public void Init(string newType) {
        type = newType;
        switch(type) {
            case "blank":
                sr.sprite = gameManager.tileSprites[0];
                priority = 9;
                break;
            case "grass":
                sr.sprite = gameManager.tileSprites[2];
                priority = 9;
                break;
            case "house_1":
                sr.sprite = gameManager.tileSprites[3];
                priority = 0;
                break;
            case "house_2":
                sr.sprite = gameManager.tileSprites[4];
                priority = 0;
                break;
            case "house_3":
                sr.sprite = gameManager.tileSprites[5];
                priority = 0;
                break;
            case "watermill":
                sr.sprite = gameManager.tileSprites[9];
                priority = 0;
                break;
            case "wheat":
                sr.sprite = gameManager.tileSprites[11];
                priority = 0;
                break;
        }
    }

    Vector2 NeighborPositionByIndex(Vector2 pos, int index) {
        if (index - 2 >= 0) { // 2, 3
            return new Vector2(pos.x - (tileWidth), pos.y - (tileHeight / 2) + ((index % 2) * tileHeight));
        } else {
            return new Vector2(pos.x + (tileWidth), pos.y + (tileHeight / 2) - (index * tileHeight));
        }
    }

    (int, int) NeighborCoordsByIndex(int index) {
        if (index == 0) { return (coords.Item1 + 1, coords.Item2); }
        if (index == 1) { return (coords.Item1, coords.Item2 - 1); }
        if (index == 2) { return (coords.Item1 - 1, coords.Item2); }
        if (index == 3) { return (coords.Item1, coords.Item2 + 1); }
        else return (0, 0);
    }
    
    Tile[] Neighbors((int, int) newCoords) {
        Tile[] neighborArray = {null, null, null, null};
        if (tileArray[newCoords.Item1 + 1, newCoords.Item2] != null) {neighborArray[0] = tileArray[newCoords.Item1 + 1,newCoords.Item2]; }
        if (tileArray[newCoords.Item1,newCoords.Item2 - 1] != null) {neighborArray[1] = tileArray[newCoords.Item1,newCoords.Item2 - 1]; }
        if (tileArray[newCoords.Item1 - 1, newCoords.Item2] != null) {neighborArray[2] = tileArray[newCoords.Item1 - 1,newCoords.Item2]; }
        if (tileArray[newCoords.Item1 ,newCoords.Item2 + 1] != null) {neighborArray[3] = tileArray[newCoords.Item1,newCoords.Item2 + 1]; }
        return neighborArray;
    }

    void SpawnAdjacentBlanks() {
        Tile[] adjList = Neighbors(coords);
        for (int i = 0; i < 4; i++) {
            if (adjList[i] == null) {
                Tile newBlankTile = GameObject.Instantiate(tilePrefab, NeighborPositionByIndex(transform.position, i), Quaternion.identity).GetComponent<Tile>();
                newBlankTile.Init("blank");
                newBlankTile.coords = NeighborCoordsByIndex(i);
                newBlankTile.neighbors = Neighbors(newBlankTile.coords);
                tileArray[newBlankTile.coords.Item1,newBlankTile.coords.Item2] = newBlankTile;
            }
        }
    }

    public void Evaluate() {
        switch(type) {
            case "house_1":
                Debug.Log("house 1");
                break;
            case "house_2":
                Debug.Log("house 2");
                break;
            case "house_3":
                Debug.Log("house 3");
                break;
            case "watermill":
                Debug.Log("watermill");
                break;
            case "wheat":
                Debug.Log("wheat");
                break;
        }
    }

    public void BeginProduction() {
        StartCoroutine(Produce());
    }

    public IEnumerator Produce() {
        while (true) {
            switch(type) {
                case "house_1":
                    yield return new WaitForSeconds(2f);
                    gameManager.Population += 1;
                    break;
                case "house_2":
                    yield return new WaitForSeconds(2f);
                    gameManager.Population += 2;
                    break;
                case "house_3":
                    yield return new WaitForSeconds(2f);
                    gameManager.Population += 3;
                    break;
                case "watermill":
                    yield return new WaitForSeconds(2f);
                    gameManager.Population += 1;
                    break;
                case "wheat":
                    yield return new WaitForSeconds(2f);
                    gameManager.Food += 2;
                    break;
            }
        }
    }
}
