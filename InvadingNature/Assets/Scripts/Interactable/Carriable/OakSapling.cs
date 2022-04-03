using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakSapling : Carriable
{
    //Turn into tree
    public GameObject tree;
    public float minTreeTime = 5f;
    public float maxTreeTime = 10f;
    private float treeTimeThreshold = 0;
    float growthTimer = 0f;

    //Uprooted
    public float minDeathTime = 4f;
    public float maxDeathTime = 8f;
    float dTimerThreshhold = 0f;
    float deathTimer = 0f;

    [HideInInspector] public PlantInfo plantInfo = null;

    protected override void Start() {
        base.Start();
        treeTimeThreshold = Random.Range(minTreeTime, maxTreeTime);
        dTimerThreshhold = Random.Range(minDeathTime, maxDeathTime);
        //Rotate
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    void Update() {
        if (!Carried) {
            if (!Uprooted) {
                growthTimer += (Time.deltaTime * plantInfo.GrowthFactor);
                if (growthTimer >= treeTimeThreshold) {
                    var go = SpawnInPosition(tree);
                    var tr = go.GetComponent<Tree>();
                    Debug.Assert(tr, "Found no Tree in Tree?");
                    tr.plantInfo = plantInfo;
                    Destroy(gameObject);
                }
            } else {
                deathTimer += Time.deltaTime;
                if (deathTimer >= dTimerThreshhold) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
