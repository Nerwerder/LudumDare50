using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverRollable : Interactable
{
    //Health
    public float health = 100f;
    public float rollOverDamage = 40f;
    public float movementSpeedFactor = 100f;
    //Player
    Player player;
    bool playerOnTop = false;
    public const string playerTag = "Player";

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null, "OverRollable was not able to find a Player in the Scene");
    }

    protected override void Update()
    {
        base.Update();
        if(playerOnTop && (player.moveSpeed > 0)) {
            float damage = (Time.deltaTime * rollOverDamage * (player.moveSpeed * movementSpeedFactor));
            health -= damage;
            if (health <= 0f) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == playerTag) {
            playerOnTop = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == playerTag) {
            playerOnTop = false;
        }
    }
}