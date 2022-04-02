using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public FlowerSeed seedling;

    private float flowerTimer = 0f;
    private float flowerThreshold = 0f;
    public float flowerMinTime = 0f;
    public float flowerMaxTime = 1f;
    public float flowerSpawnRadius = 5f;

    void Start()
    {
        ResetFlowerTimer();
    }

    void Update()
    {
        flowerTimer += Time.deltaTime;
        if(flowerTimer > flowerThreshold) {
            SpawnFlower();
            ResetFlowerTimer();
        }
    }

    private void SpawnFlower() {
        var randomPos = new Vector3(flowerSpawnRadius * Random.Range(-1f, 1f), transform.position.y, flowerSpawnRadius * Random.Range(-1f, 1f));
        Instantiate(seedling, randomPos, Quaternion.identity, transform);
    }

    private void ResetFlowerTimer() {
        flowerTimer = 0f;

        flowerThreshold = Random.Range(flowerMinTime,  flowerMaxTime);
    }
}
