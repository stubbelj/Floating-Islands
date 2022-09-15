using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    public void OnClick() {
        Debug.Log("cliky");
        GameObject.Find("GameManager").GetComponent<GameManager>().SpawnNewTile(transform.position, "grass");
    }
}
