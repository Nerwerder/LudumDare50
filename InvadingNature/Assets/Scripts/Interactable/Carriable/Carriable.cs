using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Carriable : Interactable
{
    /// <summary>
    /// If a carrieable is placed back into the scene, it will have to return to the old parent.
    /// </summary>
    [HideInInspector] public Transform oldParent = null;

    private void Start() {
        oldParent = transform.parent;
    }
}
