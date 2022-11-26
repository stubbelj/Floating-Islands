using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class house_1 : Tile
{
    new public void Produce() {
        StartCoroutine(ProductionCoroutine());
    }

    public IEnumerator ProductionCoroutine() {
        yield return new WaitForSeconds(5f);
        gameManager.Population += 1;
    }
}
