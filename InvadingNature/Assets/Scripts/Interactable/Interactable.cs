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
    /// Roll over an interactable
    /// </summary>
    public abstract void RollOver();
}
