using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeed : OverRollable
{
    public List<GameObject> possibleSeedlings;

    //Sprouting
    public float minSproutingTime = 3f;
    public float maxSproutingTime = 5f;
    float sTimeThreshold = 0f;
    float sproutTimer = 0f;

    GrowthZoneChecker zoneChecker = new GrowthZoneChecker(true, false);
    public float wrongZoneDamage = 20f;

    protected override void Start() {
        base.Start();
        //How long will it take to sprout
        sTimeThreshold = Random.Range(minSproutingTime,maxSproutingTime);
        //Rotate
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    protected override void Update() {
        base.Update();
        if(zoneChecker.CanGrow()) {
            sproutTimer += (Time.deltaTime * plantInfo.GrowthFactor);
            if (sproutTimer > sTimeThreshold) {
                Sprout();
            }
        } else {
            TakeDamage(wrongZoneDamage * Time.deltaTime);
        }
    }

    private void Sprout() {
        //Choose a random seedling
        GameObject s= possibleSeedlings[Random.Range(0, possibleSeedlings.Count)];
        //Spawn it
        GameObject nS = SpawnInPosition(s, s.transform);
        //Transfer the Damage to the next Phase
        plantInfo.Damage = (int)(maxHealth - curHealth);
        nS.GetComponentInChildren<FlowerSeedling>().plantInfo = plantInfo;
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        zoneChecker.EnterCollider(other);
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        zoneChecker.ExitCollider(other);
    }
}
