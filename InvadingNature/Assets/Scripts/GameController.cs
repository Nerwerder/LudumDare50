using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameController is the only class that is allowed to check for player inputs and will distribute those to the right Elements.
/// </summary>
public class GameController : MonoBehaviour
{
    Player player;
    FollowCamera playerCamera;

    void Start()
    {
        player = FindObjectOfType<Player>();
        playerCamera = FindObjectOfType<FollowCamera>();
    }

    void Update()
    {
        //PLAYER
        player.Move(Input.GetAxis("Vertical"));
        player.Rotate(Input.mousePosition);

        //CAMERA
        playerCamera.Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
}