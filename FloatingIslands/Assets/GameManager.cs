using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    float tileWidth = 0.8745f;
    float tileHeight = 0.8745f;

    public Dictionary<string, GameObject> tilePrefabs = new Dictionary<string, GameObject>();
    public List<GameObject> tilePrefabsList = new List<GameObject>();
    public GameObject dottedOutlinePrefab;
    public GameObject draftOptionsPrefab;
    public GameObject draftListItemPrefab;
    public List<string> draftableTileTypes = new List<string>();
    public Dictionary<string, string> tileDescriptions = new Dictionary<string, string>(){
        {"bioengineering_lab", "A laboratory for genetic engineering research."},
        {"blank", "A blank tile. You should not be able to read the description for this."},
        {"blood_shrine", "An altar for ritual of the most despicable kind."},
        {"factory", "Industrialization begins with the explotation of the workforce."},
        {"fortune_teller", "See your future, and know your fate."},
        {"fungus_farm", "Composting strengthens soil and reduces waste!"},
        {"gold_mine", "We've struck gold!"},
        {"grass", "A grassy tile."},
        {"house_1", "A tiny abode for a lonely individual."},
        {"house_2", "A modest structure just right for a couple."},
        {"house_3", "Is three a crowd?"},
        {"junkyard", "Reduce, Reuse, Recycle!"},
        {"lake", "Ideal for fishing."},
        {"marketplace", "The bustling center of commerce and community."},
        {"shortcut", "Bend space to gurantee instant delivery times."},
        {"spore_tower", "A mighty mycelium mansion."},
        {"solar_panel", "Harness the power of the sun!"},
        {"teleporter", "Reshape earth and air as you see fit."},
        {"watermill", "A classic and not yet obselete device."},
        {"wheat", "Golden fields of grain."},
        {"wonk_chemical_plant", "Take care never to Wonk! outside the prescence of a standardized Wonk! anchor."},
        {"wonk_hq", "The Wonk! Corporation is not responsible for damage or injuries sustained during Wonk!-ing."}
    };

    public System.Random r = new System.Random();
    public Sprite dottedOutlineSprite;
    public Sprite blankFillerSprite;

    public string currentPlaceableTile;
    public int currentPlaceableTileIndex = -1;
    TMP_Text populationUI;
    TMP_Text foodUI;
    TMP_Text activityUI;
    Camera cam;
    float camHeight;
    float camWidth;
    public int currentDraftCost = 0;

    public Tile[,] tileArray = new Tile[1000, 1000];
    public List<Queue<Tile>> tileQueue = new List<Queue<Tile>>();
    public List<GameObject> draftList = new List<GameObject>();

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
            modeDisplay = value;
            SelectedTile = null;
            if (value == "drafting") { 
                //Time.timeScale = 0; 
                activityUI.text = "Choose a tile!";
            }
            //if (mode == "drafting" && value != mode) {Time.timeScale = 1; }
            if (value == "placing") { activityUI.text = "Select a location for your new tile"; }
            if (value == "navigating") { activityUI.text = "You are navigating the gamespace"; }
            mode = value;
        }
    }

    public string modeDisplay = "navigating";

    private Tile selectedTile;
    public Tile SelectedTile {
        get { 
            return selectedTile; 
            }
        set {
            if (selectedTile != value) {
                if (selectedTile != null) {
                    if (GameObject.Find("DottedOutline(Clone)") != null) {
                        Destroy(selectedTile.transform.Find("DottedOutline(Clone)").gameObject);
                        selectedTile.isSelected = false;
                    }
                }
                if (value != null) {
                    GameObject newOutline = GameObject.Instantiate(dottedOutlinePrefab, value.gameObject.transform);
                    newOutline.transform.parent = value.gameObject.transform;
                    value.isSelected = true;
                }
                selectedTile = value;
            } else {
                if (selectedTile != null) {
                    Destroy(value.gameObject.transform.Find("DottedOutline(Clone)").gameObject);
                    selectedTile = null;
                    value.isSelected = false;
                }
            }
        }
    }

    void Awake() {        
        foreach (GameObject tilePrefab in tilePrefabsList) {
            tilePrefabs[tilePrefab.name] = tilePrefab;
        }

        foodUI = GameObject.Find("FoodUI").GetComponent<TMP_Text>();
        populationUI = GameObject.Find("PopulationUI").GetComponent<TMP_Text>();
        activityUI = GameObject.Find("ActivityUI").GetComponent<TMP_Text>();

        for (int i = 0; i < 10; i++) {
            tileQueue.Add(new Queue<Tile>());
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

        DraftListAdd("house_1");
        DraftListAdd("house_2");

    }

    void Update() {
    }

    void AddTilesToQueue() {
        Queue<(int, int)> queue = new Queue<(int, int)>();
        bool[,] vis = new bool[1000, 1000];
        int[] adjX = { 0, 1, 0, -1 };
        int[] adjY = { -1, 0, 1, 0 };

        queue.Enqueue((500, 500));
        int x, y, newX, newY;
        while(queue.Count > 0) {
            (int, int) curr = ((int, int))queue.Peek();
            queue.Dequeue();
            x = curr.Item1;
            y = curr.Item2;
            if (vis[x, y]){ continue; }
            vis[x, y] = true;

            tileQueue[tileArray[x, y].priority].Enqueue(tileArray[x, y]);

            for(int i = 0; i < 4; i++) {
                newX = x + adjX[i];
                newY = y + adjY[i];
                if (tileArray[newX, newY] != null && tileArray[newX, newY].priority < 10) { queue.Enqueue((newX, newY)); }
            }
        }

    }

    void EvaluateTileQueue() {
        for (int i = 0; i < 10; i++) {
            while (tileQueue[i].Count > 0) {
                tileQueue[i].Peek().Evaluate();
                tileQueue[i].Dequeue();
            }
        }
    }

    public void Draft() {
        Mode = "drafting";
        GameObject newDraftOptions = GameObject.Instantiate(draftOptionsPrefab, new Vector2(0, -5f), Quaternion.identity);
        newDraftOptions.transform.parent = GameObject.Find("Canvas").transform;
    }

    public void Draft(string newType) {
        Mode = "placing";
        currentPlaceableTile = newType;
    }

    public void Draft(string newType, int newPlaceableTileIndex) {
        Mode = "placing";
        currentPlaceableTile = newType;
        currentPlaceableTileIndex = newPlaceableTileIndex;
    }

    public void DraftListAdd(string draftType) {
        GameObject newDraftListItem = GameObject.Instantiate(draftListItemPrefab, new Vector2(0, 0), Quaternion.identity);
        newDraftListItem.transform.SetParent(GameObject.Find("DraftListItemCanvas").transform, false);
        newDraftListItem.transform.localPosition = new Vector2(-400f + (draftList.Count)*110f, -220f);
        newDraftListItem.GetComponent<DraftListItem>().index = draftList.Count;
        newDraftListItem.GetComponent<DraftListItem>().type = draftType;
        newDraftListItem.GetComponent<DraftListItem>().Init();
        draftList.Add(newDraftListItem);
        selectedTile = newDraftListItem.GetComponent<Tile>();
    }

    public void DraftListRemove(int index) {
        Draft(draftList[index].GetComponent<DraftListItem>().type);
        for (int i = index; i < draftList.Count; i++) {
            draftList[i].transform.localPosition = new Vector2(draftList[i].transform.localPosition.x - 110f, draftList[i].transform.localPosition.y);
            draftList[i].GetComponent<DraftListItem>().index -= 1;
        }
        Destroy(draftList[index]);
        draftList.RemoveAt(index);
    }

    public void SpawnPreset(int presetNum) {
        string center = "";
        switch (presetNum) {
            case 0:
                center = "grass";
                break;
        }

        Tile newTile = GameObject.Instantiate(tilePrefabs[center], new Vector2(0, 0), Quaternion.identity).GetComponent<Tile>();
        newTile.Init();
        newTile.coords = (500, 500);
        tileArray[500,500] = newTile;
        newTile.type = center;

        Tile newBlankTile = GameObject.Instantiate(tilePrefabs["blank"], new Vector2(tileWidth, tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile.Init();
        newTile.neighbors[0] = newBlankTile;
        newBlankTile.neighbors[2] = newTile;
        newBlankTile.coords = (501, 500);
        tileArray[501,500] = newBlankTile;

        Tile newBlankTile1 = GameObject.Instantiate(tilePrefabs["blank"], new Vector2(tileWidth, -tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile1.Init();
        newTile.neighbors[1] = newBlankTile1;
        newBlankTile1.neighbors[3] = newTile;
        newBlankTile1.coords = (500, 499);
        tileArray[500,499] = newBlankTile;

        Tile newBlankTile2 = GameObject.Instantiate(tilePrefabs["blank"], new Vector2(-tileWidth, tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile2.Init();
        newTile.neighbors[3] = newBlankTile2;
        newBlankTile2.neighbors[1] = newTile;
        newBlankTile2.coords = (500, 501);
        tileArray[500,501] = newBlankTile;

        Tile newBlankTile3 = GameObject.Instantiate(tilePrefabs["blank"], new Vector2(-tileWidth, -tileHeight / 2), Quaternion.identity).GetComponent<Tile>();
        newBlankTile3.Init();
        newTile.neighbors[2] = newBlankTile3;
        newBlankTile3.neighbors[0] = newTile;
        newBlankTile3.coords = (499, 500);
        tileArray[499,500] = newBlankTile;

        foreach(string type in tilePrefabs.Keys) {
            draftableTileTypes.Add(type);
        }
    }

}
