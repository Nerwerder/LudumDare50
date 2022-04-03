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
    private float acornTimer = 0f;
    private float aTimerThreshold = 0f;
    public float acornMinTime = 3f;
    public float acornMaxTime = 5f;

    //All
    public float spawnRadius = 5f;

    void Start()
    {
        ResetFlowerTimer();
        ResetAcornTimer();
    }

    void Update()
    {
        //Flower
        if(enableFlowers) {
            flowerTimer += Time.deltaTime;
            if (flowerTimer > fTimerThreshold) {
                var gm = Spawn(seedling);
                var col = bloomColors[Random.Range(0, bloomColors.Count)];
                var seed = gm.GetComponentInChildren<FlowerSeed>();
                Debug.Assert(seed, "No FlowerSeed in Spawned FlowerSeed?");
                seed.SetBloomColor(col);
                ResetFlowerTimer();
            }
        }

        //Acorn
        if(enableTrees) {
            acornTimer += Time.deltaTime;
            if (acornTimer > aTimerThreshold) {
                Spawn(acorn);
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
