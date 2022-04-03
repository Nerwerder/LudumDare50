using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactable
{
    public float fuel = 0f;

    public override void InteractWithItem(Carriable c) {
        if(c.burnable) {
            fuel += c.burnValue;
            c.Burn();
        }
    }

    public override void InteractWithPlayer(Player p) {
        //Nothing
    }
}
