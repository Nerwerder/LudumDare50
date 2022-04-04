using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour
{
    private List<Building> buildings = new List<Building>();

    public List<Image> buildingRepresentative;
    public List<Sprite> buldingStateSprites;

    GameController gameController;
    List<int> stageValues = new List<int> {0, 0, 0, 0};

    private void Start() {
        gameController = FindObjectOfType<GameController>();
    }

    public void RegisterBuilding(Building b, int id, int stage) {
        buildings.Add(b);
        //Change UI
        buildingRepresentative[id].sprite = buldingStateSprites[stage];
        stageValues[id] = stage;

        //WORKAROUND: Time is running out
        bool allIsTwo = true;
        foreach(var s in stageValues) {
            if(s != 2) {
                allIsTwo = false;
            }
        }
        if(allIsTwo) {
            gameController.GameOver();
        }
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
