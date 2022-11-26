using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blank : Tile
{
    new public void Init() {
        type = "blank";
        priority = 9;
    }
}
