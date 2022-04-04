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
    //Point of no Return (with the exception of a player picking the element up)
    private bool scaleChanged = false;
    public float scaleOutPercentage = 0.15f;

    protected override void Start() {
        base.Start();
        //How long will it take to sprout?
        sTimeThreshold = Random.Range(minSproutingTime, maxSproutingTime);
        Debug.Assert(plantInfo != null, "No PlantInfo in Acorn");
    }

    protected override void Update() {
        base.Update();
        if((!Carried) && (zoneChecker.CanGrow()) && (!scaleChanged)) {
            sproutTimer += (Time.deltaTime * plantInfo.GrowthFactor);
            if (sproutTimer > sTimeThreshold) {
                var go = SpawnInPosition(sapling);
                go.GetComponent<OakSapling>().plantInfo = plantInfo;
                Destroy(gameObject);
            }
        }
    }

    protected override void Deteriorate() {
        //Scale changed is the point of no Return
        if (((!Carried) && (!zoneChecker.CanGrow())) || (scaleChanged)) {
            DespawnTimerTick();
        }
    }

    protected override void DespawnTimerTick() {
        despawnTimer += Time.deltaTime;

        //Time for Fade Out
        if (despawnTimer > (despawnTime*(1f- scaleOutPercentage))) {
            float factor = (despawnTime - despawnTimer) / (despawnTime* scaleOutPercentage);
            transform.localScale = new Vector3(factor, factor, factor);
            scaleChanged = true;
        }

        if (despawnTimer > despawnTime) {
            Despawn();
        }
    }

    public override void InteractWithPlayer(Player p) {
        if(scaleChanged) {
            transform.localScale = Vector3.one;
            scaleChanged = false;
        }
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
