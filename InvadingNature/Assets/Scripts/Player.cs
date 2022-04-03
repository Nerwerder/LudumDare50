using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Player is the GameElement directly controlled by the Player.
/// </summary>
public class Player : MonoBehaviour
{
    public const string interactableTag = "Interactable";

    public float speed = 1f;
    public float interactionDistance = 3f;
    public float throwPower = 5f;
    public float throwUpFactor = 4f;
    private bool highSpeed;
    public bool HighSpeed {
        set { highSpeed = value; }
    }
    public float highSpeedFactor = 2f;

    /// <summary>
    /// The current Movement Speed
    /// </summary>
    [HideInInspector] public float moveSpeed = 0f;

    //TODO: Remove after final Model is here
    public Transform carryPosition;

    /// <summary>
    /// The object that is currently carried
    /// </summary>
    private Carriable carry = null;

    private void Start() {
        highSpeed = false;
    }

    public void Move(float vertical) {
        if(vertical != 0f) {
            moveSpeed = vertical * speed * Time.deltaTime;
            if(highSpeed) {
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
            if (hit.transform.tag == interactableTag && InInteractionDistance(hit.transform)) {
                interactables.Add(hit.transform.gameObject.GetComponent<Interactable>());
            } else {
                hitPoint = hit.point;
            }
        }

        if (interactables.Count != 0) {
            foreach(Interactable i in interactables) {
                if(carry) {
                    i.InteractWith(carry);
                } else {
                    i.Interact(this);
                }
            }
        } else {
            if(carry) {
                Throw(hitPoint);
            }
        }
    }

    public void CarryMe(Carriable c) {
        Debug.Assert(!carry, "Cannot carry multiple things");
        c.GetComponent<Rigidbody>().isKinematic = true;
        c.transform.position = carryPosition.transform.position;
        c.transform.rotation = Quaternion.identity;
        c.transform.parent = carryPosition;
        carry = c;
        carry.Carried = true;
    }

    public void Throw(Vector3 target) {
        Debug.Assert(carry, "Throwing requires a carry");

        carry.GetComponent<Rigidbody>().isKinematic = false;
        carry.transform.SetParent(carry.oldParent);
        Vector3 throwVector = ((target - transform.position) * throwPower);
        //Add some up so we get a nice arch
        throwVector += Vector3.up * throwVector.magnitude * throwUpFactor;
        //TODO: Maybe use a fixed arch instead of a Force
        carry.GetComponent<Rigidbody>().AddForce(throwVector);
        carry.Carried = false;
        carry = null;
    }
}
