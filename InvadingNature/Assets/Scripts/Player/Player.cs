using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Player is the GameElement directly controlled by the Player.
/// </summary>
public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float interactionDistance = 3f;
    public float throwPower = 5f;
    public float throwUpFactor = 4f;
    private bool highSpeed = false;
    public bool HighSpeed {
        set { highSpeed = value; }
    }
    public float highSpeedFactor = 2f;

    /// <summary>
    /// The current Movement Speed
    /// </summary>
    [HideInInspector] public float moveSpeed = 0f;

    /// <summary>
    /// How much can the player heal a damaged Entitiy per click (TODO: per second?)
    /// </summary>
    public float healPower = 10f;

    /// <summary>
    /// Parent for carried objects
    /// </summary>
    public Transform carryPosition;

    /// <summary>
    /// The object that is currently carried
    /// </summary>
    Carriable carry = null;

    /// <summary>
    /// The Generator provides the Player with Energy for highSpeed
    /// </summary>
    Generator generator = null;

    /// <summary>
    /// Script that will execute all the Animations
    /// </summary>
    PlayerAnimation playerAnimation = null;

    //TEST
    bool longTimeInteraction = false;
    Tree ltITree = null;
    float ltITimer = 0f;
    public float lumberjackHitsPerSecond = 0.5f;


    private void Start() {
        generator = FindObjectOfType<Generator>();
        Debug.Assert(generator, "Player was not able to find the Generator");
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update() {
        //Do Something
        if(longTimeInteraction) {
            if(moveSpeed > 0) {
                StopInteracting();
            } else {
                ltITimer += Time.deltaTime;
                if (ltITimer >= lumberjackHitsPerSecond) {
                    ltITimer = 0f;
                    var ret = ltITree.HitTree();
                    if (ret) {
                        StopInteracting();
                    }
                }
            }
        }
    }

    public void Move(float vertical) {
        if(vertical != 0f) {
            moveSpeed = vertical * speed * Time.deltaTime;
            if(highSpeed) { //TODO: && generator.On
                moveSpeed *= highSpeedFactor;
            }
            transform.Translate(transform.worldToLocalMatrix.MultiplyVector(transform.forward) * moveSpeed);
        } else {
            moveSpeed = 0f;
        }
    }

    public void Rotate(Vector3 target) {
        //Get the Screen positions of the object
        Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(target);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Apply Angle
        transform.rotation = Quaternion.Euler(new Vector3(0f,180-angle,0f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public bool InInteractionDistance(Transform other) {
        return ((other.position - transform.position).magnitude < interactionDistance);
    }

    public void Interact(RaycastHit[] hits) {
        List<Interactable> interactables = new List<Interactable>();
        Vector3 hitPoint = Vector3.zero;
        foreach (RaycastHit hit in hits) {
            if (hit.transform.tag == Interactable.interactableTag && InInteractionDistance(hit.transform)) {
                interactables.Add(hit.transform.gameObject.GetComponent<Interactable>());
            } else {
                hitPoint = hit.point;
            }
        }

        //Interact with the first interactable
        if (interactables.Count != 0) {
            if(carry) {
                interactables[0].InteractWithItem(carry);
            } else {
                interactables[0].InteractWithPlayer(this);
            }
        } else {
            if(carry) {
                ThrowItem(hitPoint);
            }
        }
    }

    public void StopInteracting() {
        if(longTimeInteraction) {
            longTimeInteraction = false;
            playerAnimation.Drop();
            if(ltITree != null) {
                ltITree.DeregisterTreeReplacementCallback();
            }
            ltITree = null;
        }
    }

    public void TreeReplacementCallback(Tree nt) {
        ltITree.DeregisterTreeReplacementCallback();
        ltITree = nt;
        ltITree.RegisterTreeReplacementCallback(TreeReplacementCallback);
    }

    public void CutTreeDown(Tree t) {
        playerAnimation.CutTree();
        t.RegisterTreeReplacementCallback(TreeReplacementCallback);
        if (t) {
            t.HitTree();
        }
        longTimeInteraction = true;
        ltITimer = 0f;
        ltITree = t;
    }

    public void RepairBuilding(Building b) {

    }

    public void CarryItem(Carriable c) {
        Debug.Assert(!carry, "Cannot carry multiple things");
        c.UpdateTransform(carryPosition.transform.position, Quaternion.identity, carryPosition);
        playerAnimation.Lift();
        carry = c;
    }

    public void ThrowItem(Vector3 target) {
        Debug.Assert(carry, "Throwing requires a carry");
        carry.Release();
        //Add some up so we get a nice arch
        Vector3 throwVector = ((target - transform.position) * throwPower);
        throwVector += Vector3.up * throwVector.magnitude * throwUpFactor;
        carry.GetComponent<Rigidbody>().AddForce(throwVector);
        playerAnimation.Throw();
        carry = null;
    }

    public void ReleaseItem() {
        Debug.Assert(carry, "Releasing requires a carry");
        carry.Release();
        playerAnimation.Drop();
        carry = null;
    }
}
