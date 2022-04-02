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
    public float throwUpPower = 4f;


    //TODO: Remove after Modell is there
    public Transform carryPosition;

    /// <summary>
    /// The object that is currently carried
    /// </summary>
    private Carriable carry = null;

    void Start()
    {
        //Nothing
    }

    void Update()
    {
        //Nothing
    }

    public void Move(float vertical) {
        float moveSpeed = vertical * speed * Time.deltaTime;
        transform.Translate(transform.worldToLocalMatrix.MultiplyVector(transform.forward) * moveSpeed);
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

    public void Interact(List<Interactable> interactables, Vector3 hitPoint) {
        if(interactables.Count != 0) {
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
    }

    public void Throw(Vector3 target) {
        Debug.Assert(carry, "Throwing requires a carry");

        carry.GetComponent<Rigidbody>().isKinematic = false;
        carry.transform.SetParent(carry.oldParent);
        Vector3 ThrowVector = ((target - transform.position) * throwPower) + (Vector3.up * throwUpPower);
        carry.GetComponent<Rigidbody>().AddForce(ThrowVector);

        carry = null;
    }
}
