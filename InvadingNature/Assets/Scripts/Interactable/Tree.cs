using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    public Tree nextStage = null;
    public Acorn acorn = null;
    public float minStageTime = 5f;
    public float maxStageTime = 10f;
    float sTimerThreshold = 0f;
    float timer = 0f;

    private void ResetTimer() {
        timer = 0f;
        sTimerThreshold = Random.Range(minStageTime, maxStageTime);
    }

    private void Start() {
        ResetTimer();
    }

    private void Update() {
        timer += Time.deltaTime;
        if (nextStage == null && (timer > sTimerThreshold)) {
            ResetTimer();
            SpawnAcorn();
        } else if(nextStage != null && (timer > sTimerThreshold)) {
            TurnInto(nextStage.gameObject);
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
