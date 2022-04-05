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

    /// <summary>
    /// Safe the Player that carries this Item
    /// </summary>
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

    //How long can the carryable survive On the Ground
    public float despawnTime = 10f;
    public bool onlyDeteriorateUprooted = false;
    protected float despawnTimer = 0f;

    //If the player carries something it should not collide with him (Flowers already don't collide with the player)
    public bool ignoreCollisionsWithPlayerIfCarried = true;

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

    protected virtual void Update() {
        Deteriorate();
    }

    protected virtual void Deteriorate() {
        if ((!Carried) && ((!onlyDeteriorateUprooted) || (onlyDeteriorateUprooted && uprooted))) {
            DespawnTimerTick();
        }
    }

    protected virtual void DespawnTimerTick() {
        despawnTimer += Time.deltaTime;
        if (despawnTimer > despawnTime) {
            Despawn();
        }
    }

    protected virtual void Despawn() {
        Destroy(gameObject);
    }

    private void SetCarryPhysics(bool p) {
        var rb = GetComponent<Rigidbody>();
        if (rb) {
            rb.isKinematic = p;
        }
        if (ignoreCollisionsWithPlayerIfCarried) {
            var pc = carryingPlayer.GetComponent<BoxCollider>();
            Debug.Assert(pc, "Was not able to find the PlayerCollider");
            var bc = GetComponent<BoxCollider>();
            if (bc) {
                Physics.IgnoreCollision(pc, bc, p);
            } else if (GetComponent<CapsuleCollider>()) {
                Physics.IgnoreCollision(pc, GetComponent<CapsuleCollider>(), p);
            } else if (GetComponent<SphereCollider>()) {
                Physics.IgnoreCollision(pc, GetComponent<SphereCollider>(), p);
            } else {
                Debug.Assert(false, "Was not able to find Carriable collider");
            }
        }
    }

    public override void InteractWithPlayer(Player p) {
        carryingPlayer = p;
        Carried = true;
        SetCarryPhysics(true);
        //Carrying something resets the despawnTimer
        despawnTimer = 0f;
        p.CarryItem(this);
    }

    public void UpdateTransform(Vector3 position, Quaternion rotation, Transform parent) {
        transform.position = position;
        transform.rotation = rotation;
        transform.parent = parent;
    }

    public void Release() {
        SetCarryPhysics(false);
        transform.SetParent(oldParent);
        Carried = false;
        carryingPlayer = null;
    }

    public override void InteractWithItem(Carriable c) {
        //Nothing
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == interactableTag) {
            var i = other.GetComponent<Interactable>();
            if(i.interactableType == InteractableType.Generator) {
                i.InteractWithItem(this);
            }
        }
    }
}
