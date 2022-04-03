using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is the parent class for all classes/objects that the player can interact with.
/// The Player has three ways to interact with an object:
/// - Interact (left click on object if player is carrying nothing)
/// - InteractWith (left clock whil carring object) - only relevant for carriables
/// - RollOver (move over something - will damage some plants)
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Interact with the Player
    /// Player guarantees he carries nothing!
    /// </summary>
    /// <param name="p"></param>
    public abstract void Interact(Player p);

    /// <summary>
    /// Interact with another interactable
    /// </summary>
    /// <param name="o"></param>
    public abstract void InteractWith(Interactable o);

    /// <summary>
    /// Spawn a Object in the position of this Object with the given parent
    /// </summary>
    /// <param name="g"> The GameObject to instantiate </param>
    /// <param name="parentOverride"> Per default the parent of the parent of this element will be the parent of the new one </param>
    /// <param name="rotationOverride"> Per default the rotation of the current object will be used for the new one </param>
    /// <returns></returns>
    protected GameObject SpawnInPosition(GameObject g, Transform parentOverride = null, Transform rotationOverride = null) {
        Vector3 nPos = new Vector3(transform.position.x, g.transform.position.y, transform.position.z);
        Transform rt = rotationOverride ? rotationOverride : transform;
        Transform pt = parentOverride ? parentOverride : transform.parent.parent;
        GameObject gm = Instantiate(g, nPos, rt.rotation, pt);
        return gm;
    }
}
