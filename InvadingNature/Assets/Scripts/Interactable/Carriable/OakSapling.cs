using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakSapling : Carriable
{
    //Turn into a tree
    public GameObject tree;
    public float minTreeTime = 5f;
    public float maxTreeTime = 10f;
    private float treeTimeThreshold = 0;
    float growthTimer = 0f;

    protected override void Start() {
        base.Start();
        treeTimeThreshold = Random.Range(minTreeTime, maxTreeTime);
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    protected override void Update() {
        if ((!Carried) && (!Uprooted)) {
            growthTimer += (Time.deltaTime * plantInfo.GrowthFactor);
            if (growthTimer >= treeTimeThreshold) {
                var go = SpawnInPosition(tree);
                go.GetComponent<Tree>().plantInfo = plantInfo;
                Destroy(gameObject);
            }
        }
    }
}
