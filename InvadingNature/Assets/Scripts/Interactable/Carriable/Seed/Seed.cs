using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seed : Carriable
{
    protected GrowthZoneChecker zoneChecker;
    public bool checkNoGrowthZone = false;
    public bool checkNoTreeZone = false;

    //Point of no Return (with the exception of a player picking the element up)
    protected bool scaleChanged = false;
    //Scale out with Damage
    public float scaleOutPercentage = 0.1f;

    //Sprouting
    protected float sproutTimer = 0f;
    public float minSproutingTime = 3f;
    public float maxSproutingTime = 5f;
    float sTimeThreshold = 0f;

    protected override void Start() {
        base.Start();
        zoneChecker = new GrowthZoneChecker(checkNoGrowthZone, checkNoTreeZone);
        sTimeThreshold = Random.Range(minSproutingTime, maxSproutingTime);
    }

    protected override void Update() {
        base.Update();
        if (zoneChecker.CanGrow() && (!scaleChanged) && (!Carried)) {
            sproutTimer += (Time.deltaTime * plantInfo.GrowthFactor);
            if (sproutTimer > sTimeThreshold) {
                Sprout();
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
        if (despawnTimer > (despawnTime * (1f - scaleOutPercentage))) {
            float factor = (despawnTime - despawnTimer) / (despawnTime * scaleOutPercentage);
            transform.localScale = new Vector3(factor, factor, factor);
            scaleChanged = true;
        }

        if (despawnTimer > despawnTime) {
            Despawn();
        }
    }

    protected abstract void Sprout();

    public override void InteractWithPlayer(Player p) {
        if (scaleChanged) {
            transform.localScale = Vector3.one;
            scaleChanged = false;
        }
        base.InteractWithPlayer(p);
        sproutTimer = 0f;
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        zoneChecker.EnterCollider(other);
    }

    void OnTriggerExit(Collider other) {
        zoneChecker.ExitCollider(other);
    }
}
