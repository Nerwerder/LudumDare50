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

    //Power
    public float powerDamageReduction = 2f;
    bool hasPower = false;
    Generator generator = null;
    Renderer rend = null;
    public int windowMaterialIndex;
    public Material window_off;
    public Material window_on;

    private void Start() {
        //controller (required so plants can damage this building)
        controller = FindObjectOfType<BuildingController>();
        Debug.Assert(controller, "Building was not able to find the BuildingController");
        controller.RegisterBuilding(this);
        //generator (required so the building can react to running generator)
        generator = FindObjectOfType<Generator>();
        Debug.Assert(generator, "Building was not able to find the Generator");
        generator.RegisterPowerChangeCallback(PowerChangeCallback);
        //Renderer for easy access
        rend = GetComponent<Renderer>();
        //Weh have to call the callback once ourselves because some objects will spawn later in the game
        PowerChangeCallback(generator.On);
    }

    public void PowerChangeCallback(bool c) {
        hasPower = c;
        //Change window Material
        Material[] mats = rend.materials;
        if (generator.On) {
            mats[windowMaterialIndex] = window_on;
        } else {
            mats[windowMaterialIndex] = window_off;

        }
        rend.materials = mats;
    }

    private void ReplaceBuilding(GameObject nb) {
        var go = SpawnInPosition(nb);
        var rb = go.GetComponent<Building>();
        rb.currentHealth = currentHealth;
        controller.RemoveBuilding(this);
        generator.DeregisterPowerChangeCallback(PowerChangeCallback);
        Destroy(gameObject);
    }

    public void DamageBuilding(float d) {
        Debug.Assert(d >= 0, "DamageBuilding can only be positive - please use Heal for negative Damage");
        float damage = hasPower ? d/powerDamageReduction : d;
        currentHealth -= damage;
        if(currentHealth < 0) { currentHealth = 0; }
        if(illerBuilding && currentHealth < healthLowerThreshold) {
            ReplaceBuilding(illerBuilding.gameObject);
        }
    }

    public void HealBuilding(float d) {
        Debug.Assert(d >= 0, "HealBuilding can only be positive - please use HealBuilding for negative Healing");
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
        HealBuilding(p.healPower);
    }
}
