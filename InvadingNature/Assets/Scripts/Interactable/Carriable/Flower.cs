using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flower that will grow out of FlowerSeedling
/// </summary>
public class Flower : Carriable
{
    //Phase
    public GameObject nextPhase = null;
    public float minPhaseTime = 2f;
    public float maxPhaseTime = 4f;

    //Spawn Seeds
    public GameObject seed = null;
    public float minSeedSpawnTime = 2f;
    public float maxSeedSpawnTime = 4f;
    public int minSpawnedSeedOnDeath = 3;
    public int maxSpawnedSeedOnDeath = 7;
    public float seedSpawnRange = 2f;

    //On the Ground
    public float minDeathTime = 4f;
    public float maxDeathTime = 8f;
    float dTimerThreshhold = 0f;
    float deathTimer = 0f;

    //Common
    float tThreshold = 0f;
    float timer = 0f;

    //Blom
    [HideInInspector] public BloomInfo bloom;

    private void ResetTimer() {
        timer = 0;
        if(nextPhase) {
            tThreshold = Random.Range(minPhaseTime, maxPhaseTime);
        } else if (seed) {
            tThreshold = Random.Range(minSeedSpawnTime, maxSeedSpawnTime);
        } else {
            Debug.Assert(false, "Unknown Timer state");
        }
    }

    protected override void Start() {
        base.Start();
        ResetTimer();
        dTimerThreshhold = Random.Range(minPhaseTime, maxDeathTime);
        //This is ... not nice
        gameObject.GetComponent<Renderer>().materials[2].color = bloom.BloomColor;
    }

    void Update() {
        //Plant is in the Ground and alive
        if(!Carried && !Uprooted) {
            timer += Time.deltaTime;
            if (timer >= tThreshold) {
                if (nextPhase) {
                    var go = SpawnInPosition(nextPhase);
                    go.GetComponentInChildren<Flower>().bloom = bloom;
                    Destroy(transform.parent.gameObject);
                } else if (seed) {
                    SpawnSeed();
                    ResetTimer();
                }
            }
        }

        //Plant is on the Ground and dead
        if(!Carried && Uprooted) {
            deathTimer += Time.deltaTime;
            if(deathTimer > dTimerThreshhold) {
                int spawned = Random.Range(minSpawnedSeedOnDeath, maxSpawnedSeedOnDeath);
                for(int k = 0; k < spawned; ++k) {
                    SpawnSeed();
                }
                Destroy(transform.parent.gameObject);
            }
        }

    }

    private void SpawnSeed() {
        if(seed != null) {
            float xOffset = Random.Range(-seedSpawnRange, seedSpawnRange);
            float zOffset = Random.Range(-seedSpawnRange, seedSpawnRange);
            Vector3 sSpawnPos = new Vector3(transform.position.x + xOffset, 0, transform.position.z + zOffset);
            var go = Instantiate(seed, sSpawnPos, Quaternion.identity, transform.parent.parent);
            var fs = go.GetComponentInChildren<FlowerSeed>();
            fs.SetBloomColor(bloom.BloomColor);
        }
    }
}
