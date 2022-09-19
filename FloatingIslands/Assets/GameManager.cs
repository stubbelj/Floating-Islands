using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    float tileWidth = 0.8745f;
    float tileHeight = 0.8745f;

    public GameObject tilePrefab;
    public GameObject dottedOutlinePrefab;
    public GameObject draftOptionsPrefab;
    public GameObject draftQueueItemPrefab;
    public Sprite[] tileSprites;
    public List<string> draftableTileTypes = new List<string>(){"house_1", "house_2", "house_3", "watermill", "wheat"};
    public System.Random r = new System.Random();

    public Stack<GameObject> draftQueue = new Stack<GameObject>();

    private int population = 0;
    public int Population {
        get {
            return population;
        }
        set {
            population = value;
            populationUI.text = (population).ToString();
        }
    }
    
    private int food = 0;
    public int Food {
        get {
            return food;
        }
        set {
            food = value;
            foodUI.text = (food).ToString();
        }
    }

    //navigate is the default, drafting is for while the window is open, placing is for choosing where to spawn in tile after drafting
    private string mode = "navigating";
    public string Mode {
        get {
            return mode;
        }
        set {
            SelectedTile = null;
            if (value == "drafting") { Time.timeScale = 0; }
            if (mode == "drafting" && value != mode) {Time.timeScale = 1; }
            mode = value;
        }
    }
    public string currentPlaceableTile;

    private Tile selectedTile;
    public Tile SelectedTile {
        get { 
            return selectedTile; 
            }
        set {
            if (selectedTile != value) {
                if (selectedTile != null) {
                    Destroy(selectedTile.transform.Find("DottedOutline(Clone)").gameObject);
                    selectedTile.selected = false;
                }
                if (value != null) {
                    GameObject newOutline = GameObject.Instantiate(dottedOutlinePrefab, value.gameObject.transform);
                    newOutline.transform.parent = value.gameObject.transform;
                    value.selected = true;
                }
                selectedTile = value;
            } else {
                if (selectedTile != null) {
                    Destroy(value.gameObject.transform.Find("DottedOutline(Clone)").gameObject);
                    selectedTile = null;
                    value.selected = false;
                }
            }
        }
    }

    TMP_Text populationUI;
    TMP_Text foodUI;
    Camera cam;
    float camHeight;
    float camWidth;

    public Tile[,] tileArray = new Tile[1000, 1000];
    public List<Stack<Tile>> tileQueue = new List<Stack<Tile>>();

    void Awake() {
        foodUI = GameObject.Find("FoodUI").GetComponent<TMP_Text>();
        populationUI = GameObject.Find("PopulationUI").GetComponent<TMP_Text>();

        for (int i = 0; i < 10; i++) {
            tileQueue.Add(new Stack<Tile>());
        }

        for (int i = 0; i < 1000; i++) {
            for (int j = 0; j < 1000; j++) {
                tileArray[i, j] = null;
            }
        }
        
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        SpawnPreset(0);

        //AddDraft("House_1");
        Draft();

    }

    void Update() {
    }

    void AddTilesToQueue() {
        Stack<(int, int)> stack = new Stack<(int, int)>();
        bool[,] vis = new bool[1000, 1000];
        int[] adjX = { 0, 1, 0, -1 };
        int[] adjY = { -1, 0, 1, 0 };

        stack.Push((500, 500));
        int x, y, newX, newY;
        while(stack.Count > 0) {
            (int, int) curr = ((int, int))stack.Peek();
            stack.Pop();
            x = curr.Item1;
            y = curr.Item2;
            if (vis[x, y]){ continue; }
            vis[x, y] = true;

            Debug.Log(tileArray[x, y]);
            tileQueue[tileArray[x, y].priority].Push(tileArray[x, y]);

            for(int i = 0; i < 4; i++) {
                newX = x + adjX[i];
                newY = y + adjY[i];
                if (tileArray[newX, newY] != null && tileArray[newX, newY].priority < 10) { stack.Push((newX, newY)); }
            }
        }

    }

    void EvaluateTileQueue() {
        for (int i = 0; i < 10; i++) {
            while (tileQueue[i].Count > 0) {
                tileQueue[i].Peek().Evaluate();
                tileQueue[i].Pop();
            }
        }
    }

    public void Draft() {
        Mode = "drafting";
        GameObject newDraftOptions = GameObject.Instantiate(draftOptionsPrefab, new Vector2(0, -5f), Quaternion.identity);
        newDraftOptions.transform.parent = GameObject.Find("Canvas").transform;
    }
/*
    public void DraftQueueInsert(string draftType) {
        draftQueue.Push(GameObject.Instantiate(draftQueueItemPrefab, new Vector2(0, 0), Quaternion.identity));
        GameObject newDraftQueueItem = GameObject.Instantiate(draftQueueItemPrefab, new Vector2(-400 + (draftQueue.Count - 1)*100, -240), Quaternion.identity);
        newDraftQueueItem.transform.parent = GameObject.Find("UICanvas");
    }

    public void DraftQueuePop(string draftType) {
        GameObject top = draftQueue.Peek();
        Draft(top.GetComponent<DraftQueueItem>().type);
        Destroy(top);
        draftQueue.Pop();
        foreach (GameObject item in draftQueue) {
            item.transform.x -= 100;
        }
    }*/

    public void SpawnPreset(int presetNum) {
        string center = "";
        switch (presetNum) {
            case 0:
                center = "grass";
                break;
        }

        Tile newTile = GameObject.Instantiate(tilePrefab, new Vector2(0, 0), Quaternion.identity).GetComponent<Tile>();
        newTile.Init(center);
        newTile.coords = (500, 500);
        tileArray[500,500] = newTile;

        Tile newBlankTile = GameObject.Instantiate(tilePrefab, new Vector2(tileWidth, tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile.Init("blank");
        newTile.neighbors[0] = newBlankTile;
        newBlankTile.neighbors[2] = newTile;
        newBlankTile.coords = (501, 500);
        tileArray[501,500] = newBlankTile;

        Tile newBlankTile1 = GameObject.Instantiate(tilePrefab, new Vector2(tileWidth, -tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile1.Init("blank");
        newTile.neighbors[1] = newBlankTile1;
        newBlankTile1.neighbors[3] = newTile;
        newBlankTile1.coords = (500, 499);
        tileArray[500,499] = newBlankTile;

        Tile newBlankTile2 = GameObject.Instantiate(tilePrefab, new Vector2(-tileWidth, tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile2.Init("blank");
        newTile.neighbors[3] = newBlankTile2;
        newBlankTile2.neighbors[1] = newTile;
        newBlankTile2.coords = (500, 501);
        tileArray[500,501] = newBlankTile;

        Tile newBlankTile3 = GameObject.Instantiate(tilePrefab, new Vector2(-tileWidth, -tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile3.Init("blank");
        newTile.neighbors[2] = newBlankTile3;
        newBlankTile3.neighbors[0] = newTile;
        newBlankTile3.coords = (499, 500);
        tileArray[499,500] = newBlankTile;
    }

}
