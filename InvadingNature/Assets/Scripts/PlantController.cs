using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    //FLOWERS
    public bool enableFlowers = true;
    public GameObject seedling;
    public List<Color> bloomColors;
    private float flowerTimer = 0f;
    private float fTimerThreshold = 0f;
    public float flowerMinTime = 0f;
    public float flowerMaxTime = 1f;

    //TREES
    public bool enableTrees = true;
    public GameObject acorn;
    public List<Color> leafColors;
    private float acornTimer = 0f;
    private float aTimerThreshold = 0f;
    public float acornMinTime = 3f;
    public float acornMaxTime = 5f;

    //All
    public float spawnRadius = 5f;
    public float growthFactor = 1f;
    public float spawnFactor = 1f;
    //Increase with time
    public float spawnIncreaseInterval = 20f;
    public float spawnIncreaseFactor = 1.01f;
    public float growthIncreaseInterval = 20f;
    public float growthIncreaseFactor = 1.01f;
    float spawnIncreaseTimer = 0f;
    float growthIncreaseTimer = 0f;

    void Start()
    {
        ResetFlowerTimer();
        ResetAcornTimer();
    }

    void Update()
    {
        //Slowly increase the SpawnRate with Time (so even the most hardcore Gamer will fail at some point) 
        spawnIncreaseTimer += Time.deltaTime;
        if (spawnIncreaseTimer > spawnIncreaseInterval) {
            spawnFactor *= spawnIncreaseFactor;
            spawnIncreaseTimer = 0f;
        }
        growthIncreaseTimer += Time.deltaTime;
        if(growthIncreaseTimer > growthIncreaseInterval) {
            growthFactor *= growthIncreaseFactor;
            growthIncreaseTimer = 0f;
        }

        //Flower
        if (enableFlowers) {
            flowerTimer += (Time.deltaTime * spawnFactor);
            if (flowerTimer > fTimerThreshold) {
                var go = Spawn(seedling);
                var col = bloomColors[Random.Range(0, bloomColors.Count)];
                var seed = go.GetComponentInChildren<FlowerSeed>();
                seed.plantInfo = new PlantInfo(col, growthFactor);
                ResetFlowerTimer();
            }
        }

        //Acorn
        if(enableTrees) {
            acornTimer += (Time.deltaTime * spawnFactor);
            if (acornTimer > aTimerThreshold) {
                var go = Spawn(acorn);
                var col = leafColors[Random.Range(0, leafColors.Count)];
                var acn = go.GetComponentInChildren<Acorn>();
                acn.plantInfo = new PlantInfo(col, growthFactor);
                ResetAcornTimer();
            }
        }
    }

    private Vector3 GetRandomPositionInRange() {
        return new Vector3(spawnRadius * Random.Range(-1f, 1f), transform.position.y, spawnRadius * Random.Range(-1f, 1f));
    }

    private GameObject Spawn(GameObject g) {
        return Instantiate(g, GetRandomPositionInRange(), Quaternion.identity, transform);
    }

    private void ResetFlowerTimer() {
        flowerTimer = 0f;
        fTimerThreshold = Random.Range(flowerMinTime,  flowerMaxTime);
    }

    private void ResetAcornTimer() {
        acornTimer = 0f;
        aTimerThreshold = Random.Range(acornMinTime, acornMaxTime);
    }
}
