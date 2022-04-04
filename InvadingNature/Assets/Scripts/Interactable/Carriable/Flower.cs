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
    public float seedSpawnSlowdown = 1.2f;

    //On the Ground
    public float minDeathTime = 4f;
    public float maxDeathTime = 8f;
    float dTimerThreshhold = 0f;
    float deathTimer = 0f;

    //Common
    float tThreshold = 0f;
    float growthTimer = 0f;

    //Bloom
    [HideInInspector] public PlantInfo plantInfo = null;

    //Building Damage
    private BuildingController buildingcontroller = null;
    public float buildingDamagePerSecond = 1f;
    private float accumulatedDamage = 0f;
    public float maxDamageDistance = 12f;

    private void ResetTimer() {
        growthTimer = 0;
        if(nextPhase) {
            tThreshold = Random.Range(minPhaseTime, maxPhaseTime);
        } else if (seed) {
            tThreshold = Random.Range(minSeedSpawnTime, maxSeedSpawnTime);
            minSeedSpawnTime *= seedSpawnSlowdown;
            maxSeedSpawnTime *= seedSpawnSlowdown;
        } else {
            Debug.Assert(false, "Unknown Timer state");
        }
    }

    protected override void Start() {
        base.Start();
        ResetTimer();
        //Death Timer
        dTimerThreshhold = Random.Range(minPhaseTime, maxDeathTime);
        //Change the color of the Flower
        Debug.Assert(plantInfo != null, "Bloom has to be set");
        gameObject.GetComponent<Renderer>().materials[2].color = plantInfo.BloomColor;
        //Get the nearest Building
        buildingcontroller = FindObjectOfType<BuildingController>();
        Debug.Assert(buildingcontroller, "Flower was not able to find the BuildingController");
        //Flowers should ignore collisions with the Player (but still behave like a rigidBody after bein uprooted and thrown)
        var player = FindObjectOfType<Player>();
        if(player) {
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), player.gameObject.GetComponent<BoxCollider>(), true);
        }
    }

    void Update() {
        //Plant is in the Ground and alive
        if(!Carried && !Uprooted) {
            //Damage the nearest Building
            accumulatedDamage += buildingDamagePerSecond * Time.deltaTime;
            if(accumulatedDamage > 1f) {
                buildingcontroller.DamageNearestBuilding(this, accumulatedDamage, maxDamageDistance);
                accumulatedDamage = 0f;
            }
            //Check for the next Phase and for reproduction
            growthTimer += (Time.deltaTime * plantInfo.GrowthFactor);
            if (growthTimer >= tThreshold) {
                if (nextPhase) {
                    var go = SpawnInPosition(nextPhase);
                    Debug.Assert(plantInfo != null, "Bloom has to be set");
                    go.GetComponentInChildren<Flower>().plantInfo = plantInfo;
                    Destroy(gameObject);
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
                Destroy(gameObject);
            }
        }

    }

    private void SpawnSeed() {
        if(seed != null) {
            float xOffset = Random.Range(-seedSpawnRange, seedSpawnRange);
            float zOffset = Random.Range(-seedSpawnRange, seedSpawnRange);
            Vector3 sSpawnPos = new Vector3(transform.position.x + xOffset, 0, transform.position.z + zOffset);
            var go = Instantiate(seed, sSpawnPos, Quaternion.identity, transform.parent);
            var fs = go.GetComponentInChildren<FlowerSeed>();
            Debug.Assert(plantInfo != null, "Bloom has to be set");
            fs.plantInfo = new PlantInfo(plantInfo);
        }
    }
}
