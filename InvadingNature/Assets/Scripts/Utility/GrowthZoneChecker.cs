using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthZoneChecker
{
    //Which zones are relevant
    const string noGrowthTag = "NoGrowthZone";
    const string noTreeTag = "NoTreeZone";
    int inNonGrowthZone = 0;
    bool checkGrowthZone = false;
    bool checkTreeZone = false;

    public GrowthZoneChecker(bool growthZone, bool treeZone) {
        checkGrowthZone = growthZone;
        checkTreeZone = treeZone;
    }

    private bool CheckZone(Collider other) {
        if ((checkGrowthZone && (other.gameObject.tag == noGrowthTag)) || (checkTreeZone && (other.gameObject.tag == noTreeTag))) {
            return true;
        } else {
            return false;
        }
    }

    public void EnterCollider(Collider other) {
        if(CheckZone(other)) {
            ++inNonGrowthZone;
        }

    }

    public void ExitCollider(Collider other) {
        if(CheckZone(other)) {
            --inNonGrowthZone;
        }
    }

    public bool CanGrow() {
        return (inNonGrowthZone == 0);
    }
}
