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
    public int minSpawnedAcornOnDeath = 3;
    public int maxSpawnedAcornOnDeath = 7;
    public float acornSpawnRange = 2f;

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

    //Timer
    float timerThreshold = 0f;
    float timer = 0f;

    private void ResetTimer() {
        timer = 0f;
        if(nextPhase) {
            timerThreshold = Random.Range(minPhaseTime, maxPhaseTime);
        } else if (acorn) {
            timerThreshold = Random.Range(minAcornSpawnTime, maxAcornSpawnTime);
        } else {
            Debug.Assert(false, "Tree without nextPhase or ability to spawn Acorns was not expected");
        }
    }

    private void Start() {
        ResetTimer();
    }

    private void Update() {
        timer += Time.deltaTime;
        if(timer >= timerThreshold) {
            if(nextPhase) {
                SpawnInPosition(nextPhase);
                Destroy(transform.parent.gameObject);
            } else if (acorn) {
                SpawnInRange(acorn, acornSpawnRange, 3f);
                ResetTimer();
            } else {
                Debug.Assert(false, "Tree without nextPhase or ability to spawn Acorns was not expected");
            }
        }
    }

    private void SpawnInRange(GameObject g, float range, float height) {
        Vector3 aSpawnPos = new Vector3(transform.position.x + Random.Range(-range,range), height, transform.position.z * Random.Range(-range,range));
        Instantiate(g, aSpawnPos, Quaternion.identity, transform.parent.parent);
    }

    public override void Interact(Player p) {
        if(++hits >= hitsToCutDown) {
            CutDown();
        }
    }

    private void CutDown() {
        //Spawn Wood
        for(int k = 0; k < spawnedWoodOnDeath; ++k) {
            SpawnInRange(wood, woodSpawnRange, 1f);
        }
        //Spawn Acorns
        for(int k = 0; k < spawnedAcornsOnDeath; ++k) {
            SpawnInRange(acorn, acornSpawnRange, 1f);
        }
        Destroy(transform.parent.gameObject);
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
