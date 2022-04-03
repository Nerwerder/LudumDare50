using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Carriable : Interactable
{
    /// <summary>
    /// If a carrieable is placed back into the scene, it will have to return to the old parent.
    /// </summary>
    [HideInInspector] public Transform oldParent = null;

    private bool carried = false;
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

    private Player carryingPlayer = null;

    bool uprooted = false;
    /// <summary>
    /// Was this object once carried
    /// </summary>
    public bool Uprooted {
        get { return uprooted; }
    }

    //Most Carriables can be trown in/at the Generator
    public bool burnable = true;
    public float burnValue = 1f;

    public void Burn() {
        if(carried) {
            //Get back from the player
            carryingPlayer.ReleaseItem();
        }
        Destroy(gameObject);
    }

    protected virtual void Start() {
        oldParent = transform.parent;
    }

    public override void InteractWithPlayer(Player p) {
        GetComponent<Rigidbody>().isKinematic = true;
        Carried = true;
        carryingPlayer = p;
        p.CarryItem(this);
    }

    public void UpdateTransform(Vector3 position, Quaternion rotation, Transform parent) {
        transform.position = position;
        transform.rotation = rotation;
        transform.parent = parent;
    }

    public void Release() {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent(oldParent);
        Carried = false;
        carryingPlayer = null;
    }

    public override void InteractWithItem(Carriable c) {
        //Nothing
    }

    public virtual void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == interactableTag) {
            var i = other.GetComponent<Interactable>();
            if(i.interactableType == InteractableType.Generator) {
                i.InteractWithItem(this);
            }
        }
    }
}
