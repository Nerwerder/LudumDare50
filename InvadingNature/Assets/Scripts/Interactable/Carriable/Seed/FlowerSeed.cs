using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeed : Seed
{
    //Sprouting
    public List<GameObject> possibleSeedlings;

    protected override void Start() {
        base.Start();
        //Rotate
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    protected override void Sprout() {
        //Choose a random seedling
        GameObject s= possibleSeedlings[Random.Range(0, possibleSeedlings.Count)];
        //Spawn it
        GameObject nS = SpawnInPosition(s, s.transform);
        //Transfer the Damage to the next Phase
        nS.GetComponentInChildren<Flower>().plantInfo = plantInfo;
        Destroy(gameObject);
    }
}
