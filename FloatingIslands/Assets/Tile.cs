using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer sr;

    //float width = 1.875f;
    //float height = 1.875f;

    // Start is called before the first frame update
    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        //width = sr.bounds.size.x;
        //height = sr.bounds.size.y;
    }

    void Update() {
    }
}
