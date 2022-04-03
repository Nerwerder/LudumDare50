using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acorn that will grow into a Tree
/// </summary>
public class Acorn : Carriable
{
    private const string noGrowthTag = "NoGrowthZone";
    private const string noTreeTag = "NoTreeZone";

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
    public float minDyingTime = 10f;
    public float maxDyingTime = 15f;
    float dyingTimer = 0f;
    float dTimerThreshold = 0f;

    [HideInInspector] public PlantInfo plantInfo = null;

    protected override void Start() {
        base.Start();
        //How long will it take to sprout?
        sTimeThreshold = Random.Range(minSproutingTime, maxSproutingTime);
        Debug.Assert(plantInfo != null, "No PlantInfo in Acorn");
        //How long will it take to die?
        dTimerThreshold = Random.Range(minDyingTime, maxDyingTime);
    }

    void Update() {
        if(!Carried) {
            if (inNonGrowthZone == 0) {
                sproutingTimer += (Time.deltaTime * plantInfo.GrowthFactor);
                if (sproutingTimer > sTimeThreshold) {
                    var go = SpawnInPosition(sapling);
                    var sa = go.GetComponent<OakSapling>();
                    Debug.Assert(sa, "No sapling in sapling?");
                    sa.plantInfo = plantInfo;
                    Destroy(gameObject);
                }
            } else {
                dyingTimer += Time.deltaTime;
                if(dyingTimer > dTimerThreshold) {
                    Destroy(gameObject);
                }
            }
        }
    }

    public override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (other.gameObject.tag == noGrowthTag || other.gameObject.tag == noTreeTag) {
            ++inNonGrowthZone;  //A seed could be in multiple overlapping Growth Zones at the same time
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == noGrowthTag || other.gameObject.tag == noTreeTag) {
            --inNonGrowthZone;
        }
    }
}
