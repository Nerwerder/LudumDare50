using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    //Phase
    public GameObject nextPhase = null;
    public float minPhaseTime = 2f;
    public float maxPhaseTime = 4f;

    //Spawn Acorns
    public GameObject acorn = null;
    public float minAcornSpawnTime = 2f;
    public float maxAcornSpawnTime = 4f;
    public float minAcornSpawnRange = 1.2f;
    public float maxAcornSpawnRange = 2.5f;
    public float acornSpawnSlowdown = 1.2f;

    //Cut the Tree
    public int hitsToCutDown = 2;
    /// <summary>
    /// Ho often was this Tree hit already
    /// </summary>
    private int hits = 0;

    //Spawn Wood
    public GameObject wood = null;
    public int spawnedWoodOnDeath = 3;
    public int spawnedAcornsOnDeath = 2;
    public float woodSpawnRange = 1f;

    //Building Damage
    private BuildingController buildingcontroller = null;
    public float buildingDamagePerSecond = 1f;
    private float accumulatedDamage = 0f;
    public float maxDamageDistance = 15f;

    //Timer
    float timerThreshold = 0f;
    float growthTimer = 0f;

    [HideInInspector] public PlantInfo plantInfo = null;
    private void ResetTimer() {
        growthTimer = 0f;
        if(nextPhase) {
            timerThreshold = Random.Range(minPhaseTime, maxPhaseTime);
        } else if (acorn) {
            timerThreshold = Random.Range(minAcornSpawnTime, maxAcornSpawnTime);
            minAcornSpawnTime *= acornSpawnSlowdown;
            maxAcornSpawnTime *= acornSpawnSlowdown;
        } else {
            Debug.Assert(false, "Tree without nextPhase or ability to spawn Acorns was not expected");
        }
    }

    private void Start() {
        ResetTimer();
        //Change the color of the Leaves
        gameObject.GetComponent<Renderer>().materials[1].color = plantInfo.BloomColor;
        //Take the Damage fomr earlier stages
        hits = plantInfo.Damage;
        //Get the nearest Building
        buildingcontroller = FindObjectOfType<BuildingController>();
        Debug.Assert(buildingcontroller, "Flower was not able to find the BuildingController");
    }

    private void Update() {
        //Damage the nearest Building
        accumulatedDamage += buildingDamagePerSecond * Time.deltaTime;
        if (accumulatedDamage > 1f) {
            buildingcontroller.DamageNearestBuilding(this, accumulatedDamage, maxDamageDistance);
            accumulatedDamage = 0f;
        }
        //Grow into the next Phase or create Acorns
        growthTimer += (Time.deltaTime * plantInfo.GrowthFactor);
        if(growthTimer >= timerThreshold) {
            if(nextPhase) {
                var go = SpawnInPosition(nextPhase);
                //Transfer the taken damage to the next phase
                plantInfo.Damage = hits;
                go.GetComponent<Tree>().plantInfo = plantInfo;
                Destroy(gameObject);
            } else if (acorn) {
                SpanAcorn(3f);
                ResetTimer();
            } else {
                Debug.Assert(false, "Tree without nextPhase or ability to spawn Acorns was not expected");
            }
        }
    }

    private float GetPoint(float minRange, float maxRange) {
        float distance = Random.Range(minRange, maxRange);
        return (Random.Range(0, 2) == 0) ? (distance) : (-distance);
    }

    private GameObject SpawnInRange(GameObject g, float minRange, float maxRange, float height) {
        Vector3 aSpawnPos = new Vector3(transform.position.x + GetPoint(minRange, maxRange), height, transform.position.z + GetPoint(minRange, maxRange));
        return Instantiate(g, aSpawnPos, Quaternion.identity, transform.parent);
    }

    private void SpanAcorn(float height) {
        var go = SpawnInRange(acorn, minAcornSpawnRange, maxAcornSpawnRange, height);
        go.GetComponent<Acorn>().plantInfo = new PlantInfo(plantInfo);
    }

    public override void InteractWithPlayer(Player p) {
        if(++hits >= hitsToCutDown) {
            CutDown();
        }
    }

    private void CutDown() {
        //Spawn Wood
        for(int k = 0; k < spawnedWoodOnDeath; ++k) {
            SpawnInRange(wood, 0f, woodSpawnRange, 1f);
        }
        //Spawn Acorns
        for(int k = 0; k < spawnedAcornsOnDeath; ++k) {
            SpanAcorn(1f);
        }
        Destroy(gameObject);
    }

    public override void InteractWithItem(Carriable o) {
        //Nothing
    }
}
