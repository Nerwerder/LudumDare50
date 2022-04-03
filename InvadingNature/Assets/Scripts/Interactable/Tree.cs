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

    //Spawn Wood
    public GameObject Wood = null;
    public int minSpawnedWoodOnDeath = 2;
    public int maxSpawnedWoodOnDeadth = 3;
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
                SpawnAcorn();
                ResetTimer();
            } else {
                Debug.Assert(false, "Tree without nextPhase or ability to spawn Acorns was not expected");
            }
        }
    }

    private void SpawnAcorn() {
        float range = 3f;
        Vector3 aSpawnPos = new Vector3(transform.position.x + Random.Range(-range,range), 5, transform.position.z * Random.Range(-range,range));
        Instantiate(acorn, aSpawnPos, Quaternion.identity, transform.parent);
    }

    public override void Interact(Player p) {
        throw new System.NotImplementedException();
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
