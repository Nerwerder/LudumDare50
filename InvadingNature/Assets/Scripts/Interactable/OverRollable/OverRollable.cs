using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OverRollable : Interactable
{
    //Health
    public float maxHealth = 100f;
    public float curHealth = 100f;
    public float rollOverDamage = 40f;
    public float movementSpeedFactor = 100f;
    //Player
    Player player;
    bool playerOnTop = false;
    public const string playerTag = "Player";

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null, "OverRollable was not able to find a Player in the Scene");
    }

    protected virtual void Update()
    {
        if(playerOnTop && (player.moveSpeed > 0)) {
            float damage = (Time.deltaTime * rollOverDamage * (player.moveSpeed * movementSpeedFactor));
            TakeDamage(damage);
        }
    }

    public override void InteractWithPlayer(Player p) {
        //Nothing
    }

    public override void InteractWithItem(Carriable c) {
        //Nothing
    }

    protected virtual void TakeDamage(float damage) {
        curHealth -= damage;
        if (curHealth <= 0f) {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == playerTag) {
            playerOnTop = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == playerTag) {
            playerOnTop = false;
        }
    }
}
