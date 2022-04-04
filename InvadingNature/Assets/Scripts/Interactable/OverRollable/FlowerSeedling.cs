using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeedling : OverRollable
{
    //Phase
    public GameObject nextPhase;
    public float minPhaseTime = 2f;
    public float maxPhaseTime = 4f;
    float pTimerThreshold = 0f;
    float phaseTimer = 0f;

    protected override void Start() {
        base.Start();
        pTimerThreshold = Random.Range(minPhaseTime, maxPhaseTime);
        TakeDamage((float)plantInfo.Damage);
    }

    protected override void Update() {
        base.Update();
        phaseTimer += (Time.deltaTime * plantInfo.GrowthFactor);
        if (phaseTimer > pTimerThreshold) {
            NextPhase();
        }
    }

    public void NextPhase() {
        GameObject np = SpawnInPosition(nextPhase);
        FlowerSeedling seedling = np.GetComponentInChildren<FlowerSeedling>();
        plantInfo.Damage = (int)(maxHealth - curHealth);
        if (seedling) {
            seedling.plantInfo = plantInfo;
        }
        Flower flower = np.GetComponentInChildren<Flower>();
        if(flower) {
            flower.plantInfo = plantInfo;
        }
        Destroy(gameObject);
    }
}
