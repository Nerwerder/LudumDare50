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
    float timer = 0f;

    //Uprooted
    public float minDeathTime = 4f;
    public float maxDeathTime = 8f;
    float dTimerThreshhold = 0f;
    float deathTimer = 0f;

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
                timer += Time.deltaTime;
                if (timer >= treeTimeThreshold) {
                    SpawnInPosition(tree);
                    Destroy(transform.parent.gameObject);
                }
            } else {
                deathTimer += Time.deltaTime;
                if (deathTimer >= dTimerThreshhold) {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }
}
