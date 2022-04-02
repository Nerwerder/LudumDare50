using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Carriable : Interactable
{
    /// <summary>
    /// If a carrieable is placed back into the scene, it will have to return to the old parent.
    /// </summary>
    [HideInInspector] public Transform oldParent = null;

    /// <summary>
    /// Ho much time did this object spend on the ground
    /// </summary>
    protected float nonCarriedTimer = 0f;

    /// <summary>
    /// Is this object currently carried?
    /// </summary>
    protected bool carried = false;
    public bool Carrying {
        set { carried = value; }
        get { return carried; }
    }

    protected override void Start() {
        base.Start();
        oldParent = transform.parent;
    }

    protected override void Update() {
        base.Update();
        if(!carried) {
            nonCarriedTimer += Time.deltaTime;
        }
    }
}
