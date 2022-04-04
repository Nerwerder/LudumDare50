using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    private List<Building> buildings = new List<Building>();

    public List<Image> buildingRepresentative;
    public List<Sprite> buldingStateSprites;

    public void RegisterBuilding(Building b, int id, int stage) {
        buildings.Add(b);
        //Change UI
        buildingRepresentative[id].sprite = buldingStateSprites[stage];
    }

    public void RemoveBuilding(Building b) {
        buildings.Remove(b);
    }

    public void DamageNearestBuilding(Interactable attacker, float damage, float maxDistance) {
        var b = GetNearestBuilding(attacker.transform.position);
        if((Vector3.Distance(attacker.transform.position, b.transform.position) < maxDistance) && b) {
            b.DamageBuilding(damage);
        }
    }

    private Building GetNearestBuilding(Vector3 p) {
        float nearestDistance = Mathf.Infinity;
        Building nearestBuilding = null;
        foreach(var b in buildings) {
            //Cheaper than Vector3.Distance
            Vector3 direction = p - b.transform.position;
            float dSqrt = direction.sqrMagnitude;
            if(dSqrt < nearestDistance) {
                nearestDistance = dSqrt;
                nearestBuilding = b;
            }
        }
        return nearestBuilding;
    }
}
