using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeed : OverRollable
{
    private const string noGrowthTag = "NoGrowthZone";

    public List<GameObject> possibleSeedlings;
    //Sprouting
    public float minSproutingTime = 3f;
    public float maxSproutingTime = 5f;
    float sTimeThreshold = 0f;
    float sproutingTimer = 0f;

    //Dying
    /// <summary>
    /// How much Damage does a Seed take in a nonGrotshZone per Second
    /// </summary>
    public float noGrowthZoneDamage = 10f;
    int inNonGrowthZone = 0;


    protected override void Start() {
        base.Start();
        sTimeThreshold = Random.Range(minSproutingTime,maxSproutingTime);
    }

    protected override void Update() {
        base.Update();
        if(inNonGrowthZone > 0) {
            TakeDamage(Time.deltaTime * noGrowthZoneDamage);
        } else {
            sproutingTimer += Time.deltaTime;
            if (sproutingTimer > sTimeThreshold) {
                Sprout();
            }
        }
    }

    private void Sprout() {
        //Choose a random seedling
        GameObject s= possibleSeedlings[Random.Range(0, possibleSeedlings.Count)];
        GameObject nS = Instantiate(s, transform.position, Quaternion.identity, transform.parent);
        nS.GetComponentInChildren<FlowerSeedling>().health  = health; //Keep the health
        Destroy(gameObject);
    }

    public override void Interact(Player p) {
        //Nothing
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == noGrowthTag) {
            ++inNonGrowthZone;  //A seed could be in multiple overlapping Growth Zones at the same time
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == noGrowthTag) {
            --inNonGrowthZone;
        }
    }
}
