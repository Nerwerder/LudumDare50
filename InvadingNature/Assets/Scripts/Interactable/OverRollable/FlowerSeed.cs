using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeed : OverRollable
{
    private const string noGrowthTag = "NoGrowthZone";

    public List<GameObject> possibleSeedlings;
    private BloomInfo bloom = new BloomInfo();

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
        //How long will it take to sprout
        sTimeThreshold = Random.Range(minSproutingTime,maxSproutingTime);
        //Rotate
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
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

    public void SetBloomColor(Color c) {
        bloom.BloomColor = c;
    }

    private void Sprout() {
        //Choose a random seedling
        GameObject s= possibleSeedlings[Random.Range(0, possibleSeedlings.Count)];
        //Spawn it
        GameObject nS = SpawnInPosition(s, transform.parent.parent, s.transform);
        //Set some values
        FlowerSeedling fS = nS.GetComponentInChildren<FlowerSeedling>();
        fS.health = health;
        fS.bloom = bloom;
        Destroy(transform.parent.gameObject);//Rather ugly
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
