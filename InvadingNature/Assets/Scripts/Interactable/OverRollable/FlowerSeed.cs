using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeed : OverRollable
{
    public List<FlowerSeedling> possibleSeedlings;
    //Sprouting
    public float minSproutingTime = 3f;
    public float maxSproutingTime = 5f;
    float sTimeThreshold = 0f;

    protected override void Start() {
        base.Start();
        sTimeThreshold = Random.Range(minSproutingTime,maxSproutingTime);
    }

    protected override void Update() {
        base.Update();
        if(timer > sTimeThreshold) {
            Sprout();
        }
    }

    private void Sprout() {
        //Choose a random seedling
        FlowerSeedling s= possibleSeedlings[Random.Range(0, possibleSeedlings.Count)];
        FlowerSeedling nS = Instantiate(s, transform.position, Quaternion.identity, transform.parent);
        nS.health = health; //Keep the health
        Destroy(gameObject);
    }

    public override void Interact(Player p) {
        //Nothing
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
