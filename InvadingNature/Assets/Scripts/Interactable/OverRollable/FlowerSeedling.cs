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

    [HideInInspector] public BloomInfo bloom;

    protected override void Start() {
        base.Start();
        pTimerThreshold = Random.Range(minPhaseTime, maxPhaseTime);
    }

    protected override void Update() {
        base.Update();
        phaseTimer += Time.deltaTime;
        if (phaseTimer > pTimerThreshold) {
            NextPhase();
        }
    }

    public void NextPhase() {
        GameObject np = SpawnInPosition(nextPhase);
        FlowerSeedling seedling = np.GetComponentInChildren<FlowerSeedling>();
        if(seedling) {
            seedling.bloom = bloom;
        }
        Flower flower = np.GetComponentInChildren<Flower>();
        if(flower) {
            flower.bloom = bloom;
        }
        Destroy(transform.parent.gameObject);
    }

    public override void Interact(Player p) {
        //Nothing
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
