using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The GameController is the only class that is allowed to check for player inputs and will distribute those to the right Elements.
/// </summary>
public class GameController : MonoBehaviour
{
    Player player = null;
    FollowCamera playerCamera = null;

    void Start()
    {
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null, "GameController was not able to find a Player in the Scene");
        playerCamera = FindObjectOfType<FollowCamera>();
        Debug.Assert(playerCamera != null, "GameController was not able to find a Camera in the Scene");
    }

    void Update()
    {
        //PLAYER
        player.Move(Input.GetAxis("Vertical"));
        player.Rotate(Input.mousePosition);
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit[] hits;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 100f);
            player.Interact(hits);
        }

        //CAMERA
        playerCamera.Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
}