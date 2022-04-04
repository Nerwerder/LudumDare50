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
    public float buildingInteractionDistance = 6f;
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
    public float healPowerPerHit = 10f;

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
    Building ltIBuilding = null;
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
                    var ret = true;
                    if (ltITree != null) {
                        ret = ltITree.HitTree();
                    }
                    if (ltIBuilding != null) {
                        ret = ltIBuilding.HealBuilding(healPowerPerHit);
                    }
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
        if(!longTimeInteraction) {
            //Get the Screen positions of the object
            Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the Screen position of the mouse
            Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(target);

            //Get the angle between the points
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            //Apply Angle
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180 - angle, 0f));
        }
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public bool InInteractionDistance(Interactable other) {
        float distance = (other.interactableType == Interactable.InteractableType.Building) ? buildingInteractionDistance : interactionDistance;
        return ((other.transform.position - transform.position).magnitude < distance);
    }

    public void Interact(RaycastHit[] hits) {
        List<Interactable> interactables = new List<Interactable>();
        Vector3 hitPoint = Vector3.zero;
        foreach (RaycastHit hit in hits) {
            if (hit.transform.tag == Interactable.interactableTag) {
                var i = hit.transform.gameObject.GetComponent<Interactable>();
                Debug.Assert(i != null, "Object has the Interactable Tag but no Interactable Script?");
                if(InInteractionDistance(i)) {
                    interactables.Add(i);
                }
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
            if(ltIBuilding != null) {
                ltIBuilding.DeregisterBuildingReplacementCallback();
            }
            ltITree = null;
            ltIBuilding = null;
        }
    }

    public void TreeReplacementCallback(Tree nt) {
        Debug.Assert(nt, "TreeReplacementCallback was called without valid Tree");
        ltITree.DeregisterTreeReplacementCallback();
        ltITree = nt;
        ltITree.RegisterTreeReplacementCallback(TreeReplacementCallback);
    }

    public void CutTreeDown(Tree t) {
        playerAnimation.CutTree();
        ltITree = t;
        longTimeInteraction = true;
        ltITimer = 0f;
        ltITree.RegisterTreeReplacementCallback(TreeReplacementCallback);
        ltITree.HitTree();
    }

    public void BuildingReplacementCallback(Building nb) {
        Debug.Assert(ltIBuilding, "BuildingReplacementCallback was called without valid OLD Building");
        Debug.Assert(nb, "BuildingReplacementCallback was called without valid NEW Building");
        ltIBuilding.DeregisterBuildingReplacementCallback();
        ltIBuilding = nb;
        ltIBuilding.RegisterBuildingReplacementCallback(BuildingReplacementCallback);
    }

    public void RepairBuilding(Building b) {
        playerAnimation.HitHouse();
        ltIBuilding = b;
        longTimeInteraction = true;
        ltITimer = 0f;
        ltIBuilding.RegisterBuildingReplacementCallback(BuildingReplacementCallback);
        ltIBuilding.HealBuilding(healPowerPerHit);
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
