using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeedling : OverRollable
{
    public GameObject nextPhase;
    public float minPhaseTime = 2f;
    public float maxPhaseTime = 4f;
    float fTimerThreshold = 0f;
    float timer = 0f;

    protected override void Start() {
        base.Start();
        fTimerThreshold = Random.Range(minPhaseTime, maxPhaseTime);
    }

    protected override void Update() {
        base.Update();
        timer += Time.deltaTime;
        if (timer > fTimerThreshold) {
            TurnInto(nextPhase);
        }
    }

    public override void Interact(Player p) {
        //Nothing
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
