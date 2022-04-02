using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flower that will grow out of FlowerSeedling
/// </summary>
public class Flower : Carriable
{
    public FlowerSeed seed;
    public float minSeedSpawnTime = 2f;
    public float maxSeedSpawnTime = 4f;
    float seedSpawnThreshold = 0f;

    private void ResetTimer() {
        nonCarriedTimer = 0;
        seedSpawnThreshold = Random.Range(minSeedSpawnTime, maxSeedSpawnTime);
    }

    protected override void Start() {
        base.Update();
        ResetTimer();
    }

    protected override void Update() {
        base.Update();
        if(nonCarriedTimer >= seedSpawnThreshold) {
            ResetTimer();
            SpawnSeed();
        }
    }

    private void SpawnSeed() {
        float range = 2f;
        Vector3 sSpawnPos = new Vector3(transform.position.x + Random.Range(-range, range), 0, transform.position.z * Random.Range(-range, range));
        Instantiate(seed, sSpawnPos, Quaternion.identity, transform.parent);
    }

    public override void Interact(Player p) {
        p.CarryMe(this);
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
