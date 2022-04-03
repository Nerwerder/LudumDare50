using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Carriable
{
    public float minAliveTime = 12f;
    public float maxAliveTime = 20f;
    private float aliveTimer = 0f;
    private float aTimerThreshold = 0f;

    protected override void Start() {
        base.Start();
        aTimerThreshold = Random.Range(minAliveTime, maxAliveTime);
    }

    private void Update() {
        aliveTimer += Time.deltaTime;
        if(aliveTimer > aTimerThreshold) {
            Destroy(this);
        }
    }
}
