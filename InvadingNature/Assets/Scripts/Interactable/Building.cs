using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    BuildingController controller = null;

    public float currentHealth = 300f;
    public float maxHealth = 300f;
    public Building healthierBuiling = null;
    public Building illerBuilding = null;
    public float healthUpperThreshold = 0f;
    public float healthLowerThreshold = 0f;

    private void Start() {
        controller = FindObjectOfType<BuildingController>();
        Debug.Assert(controller, "Building was not able to find the BuildingController");
        controller.RegisterBuilding(this);
    }

    private void ReplaceBuilding(GameObject nb) {
        var go = SpawnInPosition(nb);
        var rb = go.GetComponent<Building>();
        rb.currentHealth = currentHealth;
        controller.RemoveBuilding(this);
        Destroy(gameObject);
    }

    public void Damage(float d) {
        currentHealth -= d;
        if(currentHealth < 0) { currentHealth = 0; }
        if(illerBuilding && currentHealth < healthLowerThreshold) {
            ReplaceBuilding(illerBuilding.gameObject);
        }
    }

    public void Heal(float d) {
        currentHealth += d;
        if (currentHealth > maxHealth) { currentHealth = maxHealth; }
        if(healthierBuiling && currentHealth > healthUpperThreshold) {
            ReplaceBuilding(healthierBuiling.gameObject);
        }
    }

    public override void InteractWithItem(Carriable c) {
        //Nothing
    }

    public override void InteractWithPlayer(Player p) {
        //Nothing
    }
}
