using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acorn that will grow into a Tree
/// </summary>
public class Acorn : Carriable
{
    private const string noGrowthTag = "NoGrowthZone";

    public GameObject sapling;

    //Sprouting
    public float minSproutingTime = 3f;
    public float maxSproutingTime = 5f;
    float sTimeThreshold = 0f;
    float sproutingTimer = 0f;

    //Dying
    /// <summary>
    /// Acorn will not sprout inside of a noGrowthZone
    /// </summary>
    int inNonGrowthZone = 0;

    protected override void Start() {
        base.Start();
        //How long will it take to sprout
        sTimeThreshold = Random.Range(minSproutingTime, maxSproutingTime);

    }

    void Update() {
        if (inNonGrowthZone == 0 && !Carried) {
            sproutingTimer += Time.deltaTime;
            if (sproutingTimer > sTimeThreshold) {
                SpawnInPosition(sapling);
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == noGrowthTag) {
            ++inNonGrowthZone;  //A seed could be in multiple overlapping Growth Zones at the same time
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == noGrowthTag) {
            --inNonGrowthZone;
        }
    }
}
