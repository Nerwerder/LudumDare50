using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flower that will grow out of FlowerSeedling
/// </summary>
public class Flower : Carriable
{
    private float timeAlive = 0f;

    private void Update() {
        timeAlive += Time.deltaTime;
    }

    public override void Interact(Player p) {
        p.CarryMe(this);
    }

    public override void InteractWith(Interactable o) {
        throw new System.NotImplementedException();
    }

    public override void RollOver() {
        throw new System.NotImplementedException();
    }
}
