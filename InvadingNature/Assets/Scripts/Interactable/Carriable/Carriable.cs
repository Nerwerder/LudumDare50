using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Carriable : Interactable
{
    /// <summary>
    /// If a carrieable is placed back into the scene, it will have to return to the old parent.
    /// </summary>
    [HideInInspector] public Transform oldParent = null;

    protected bool carried = false;
    /// <summary>
    /// Is this object currently carried
    /// </summary>
    public bool Carried {
        set { carried = value; 
            if(carried) {
                uprooted = true;
            }
        }
        get { return carried; }
    }

    bool uprooted = false;
    /// <summary>
    /// Was this object once carried
    /// </summary>
    public bool Uprooted {
        get { return uprooted; }
    }

    protected virtual void Start() {
        oldParent = transform.parent;
    }

}
