using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acorn that will grow into a Sapling (that should grow into a Tree)
/// </summary>
public class Acorn : Seed
{
    public GameObject sapling;

    protected override void Sprout() {
        var go = SpawnInPosition(sapling);
        go.GetComponent<OakSapling>().plantInfo = plantInfo;
        Destroy(gameObject);
    }
}
