using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSeedling : OverRollable
{
    public Flower flower;
    public float minFloweringTime = 2f;
    public float maxFloweringTime = 4f;
    float fTimerThreshold = 0f;

    protected override void Start() {
        base.Start();
        fTimerThreshold = Random.Range(minFloweringTime, maxFloweringTime);
    }

    protected override void Update() {
        base.Update();
        if (timer > fTimerThreshold) {
            TurnInto(flower.gameObject);
        }
    }

    public override void Interact(Player p) {
        //Nothing
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
