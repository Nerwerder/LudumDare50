using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flower that will grow out of FlowerSeedling
/// </summary>
public class Flower : Carriable
{
    private float aliveTimer = 0f;

    private void Update() {
        aliveTimer += Time.deltaTime;
    }

    public override void Interact(Player p) {
        p.CarryMe(this);
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
