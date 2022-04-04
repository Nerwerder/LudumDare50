using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactable
{
    public float fuel = 0f;
    public float fuelConsumptionPerSecond = 0.2f;
    private bool oldOn = false;
    private bool on = false;

    public bool On {
        get { return on; }
    }

    public Material onMaterial;
    public Material offMaterial;
    private ParticleSystem[] particleSystems;
    //PowerChange
    public delegate void PowerChangeCallback(bool c);
    private List<PowerChangeCallback> powerChangeCallbacks = new List<PowerChangeCallback>();

    private void Start() {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        RegisterPowerChangeCallback(PowerChange);
    }

    public void RegisterPowerChangeCallback(PowerChangeCallback c) {
        powerChangeCallbacks.Add(c);
    }

    public void DeregisterPowerChangeCallback(PowerChangeCallback c) {
        powerChangeCallbacks.Remove(c);
    }

    private void CallAllPowerChangeCallbacks(bool s) {
        foreach(var c in powerChangeCallbacks) {
            c(s);
        }
    }

    public void PowerChange(bool c) {
        if (on) {
            //gameObject.GetComponent<Renderer>().material = onMaterial;
            foreach (var p in particleSystems) {
                p.Play();
            }
        } else {
            //gameObject.GetComponent<Renderer>().material = offMaterial;
            foreach (var p in particleSystems) {
                p.Stop();
            }
        }
    }

    private void Update() {
        if(fuel > 0) {
            fuel -= fuelConsumptionPerSecond * Time.deltaTime;
            on = true;
        } else {
            on = false;
        }

        if(on != oldOn) {
            oldOn = on;
            CallAllPowerChangeCallbacks(on);
            if(on) {
                GetComponent<AudioSource>().Play();
            } else {
                GetComponent<AudioSource>().Pause();
            }
        }
    }

    public override void InteractWithItem(Carriable c) {
        if(c.burnable) {
            fuel += c.burnValue;
            c.Burn();
        }
    }

    public override void InteractWithPlayer(Player p) {
        //Nothing
    }
}
