using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple FollowCamera, will simply follow the player with statuc rotation
/// </summary>
public class FollowCamera : MonoBehaviour
{
    Transform player;
    public float smoothSpeed = 1f;
    Vector3 offset;

    //ZOOM
    public float minDistance = 25;
    public float maxDistance = 60;
    private float offsetLength = 1f;

    void Start()
    {
        offset = transform.position;
        player = FindObjectOfType<Player>().transform;
        transform.position = player.position + offset;
        transform.LookAt(player);
    }

    void FixedUpdate() {
        var desPosition = player.position + (offset * offsetLength);
        var smoPosition = Vector3.Lerp(transform.position, desPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoPosition;
    }

    public void Zoom(float zoomDelta) {
        if (zoomDelta != 0) {
            offsetLength -= zoomDelta;
            if ((offset * offsetLength).magnitude < minDistance) {
                offsetLength = minDistance / offset.magnitude;
            }

            if ((offset * offsetLength).magnitude > maxDistance) {
                offsetLength = maxDistance / offset.magnitude;
            }
        }
    }
}
