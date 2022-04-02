using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acorn that will grow into a Tree
/// </summary>
public class Acorn : Carriable
{
    public Tree tree;
    public float minTreeTime = 5f;
    public float maxTreeTime = 10f;
    private float treeTimeThreshold = 0;
    float timer = 0f;

    protected override void Start() {
        base.Start();
        treeTimeThreshold = Random.Range(minTreeTime, maxTreeTime);
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer > treeTimeThreshold) {
            TurnInto(tree.gameObject);
        }
    }

    public override void Interact(Player p) {
        p.CarryMe(this);
    }

    public override void InteractWith(Interactable o) {
        //Nothing
    }
}
