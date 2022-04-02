using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Player is the GameElement directly controlled by the Player.
/// </summary>
public class Player : MonoBehaviour
{
    public float speed = 1f;

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
}
