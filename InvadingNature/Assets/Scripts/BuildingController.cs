using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private List<Building> buildings = new List<Building>();

    public void RegisterBuilding(Building b) {
        buildings.Add(b);
    }

    public void RemoveBuilding(Building b) {
        buildings.Remove(b);
    }

    public void DamageNearestBuilding(Interactable attacker, float damage) {
        //TODO: optimize this (safe the building per attacker in a map ...)
        var b = GetNearestBuilding(attacker.transform.position);
        if(b) {
            b.Damage(damage);
        }
    }

    private Building GetNearestBuilding(Vector3 p) {
        float nearestDistance = 10000;
        Building nearestBuilding = null;
        foreach(var b in buildings) {
            float distance = Vector3.Distance(b.transform.position, p);
            if(distance  < nearestDistance) {
                nearestDistance = distance;
                nearestBuilding = b;
            }
        }
        return nearestBuilding;
    }
}
