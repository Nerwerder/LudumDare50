using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acorn that will grow into a Sapling (that should grow into a Tree)
/// </summary>
public class Acorn : Carriable
{
    //Sprouting
    public GameObject sapling;
    public float minSproutingTime = 3f;
    public float maxSproutingTime = 5f;
    float sTimeThreshold = 0f;
    float sproutTimer = 0f;

    GrowthZoneChecker zoneChecker = new GrowthZoneChecker(true, true);

    protected override void Start() {
        base.Start();
        //How long will it take to sprout?
        sTimeThreshold = Random.Range(minSproutingTime, maxSproutingTime);
        Debug.Assert(plantInfo != null, "No PlantInfo in Acorn");
    }

    protected override void Update() {
        base.Update();
        if((!Carried) && (zoneChecker.CanGrow())) {
            sproutTimer += (Time.deltaTime * plantInfo.GrowthFactor);
            if (sproutTimer > sTimeThreshold) {
                var go = SpawnInPosition(sapling);
                go.GetComponent<OakSapling>().plantInfo = plantInfo;
                Destroy(gameObject);
            }
        }
    }

    protected override void Deteriorate() {
        if((!Carried) && (!zoneChecker.CanGrow())) {
            DespawnTimerTick();
        }
    }

    public override void InteractWithPlayer(Player p) {
        base.InteractWithPlayer(p);
        sproutTimer = 0f;
    }

    public override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        zoneChecker.EnterCollider(other);
    }

    private void OnTriggerExit(Collider other) {
        zoneChecker.ExitCollider(other);
    }
}
