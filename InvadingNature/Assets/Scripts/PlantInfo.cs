using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInfo
{
    /// <summary>
    /// Bloom Color (also used for leaves)
    /// </summary>
    private Color bloomColor = Color.black;
    public Color BloomColor {
        get { return bloomColor; }
        set { bloomColor = value; }
    }

    /// <summary>
    /// GrowthFactor is used to speed up the growth of ALL plants (mostly for play testing)
    /// </summary>
    private float growthFactor = 1f;
    public float GrowthFactor {
        get { return growthFactor; }
        set { growthFactor = value; }
    }

    /// <summary>
    /// How much Damage has this Plant taken in its earlier phases
    /// </summary>
    private int damage = 0;
    public int Damage {
        get { return damage; }
        set { damage = value; }
    }

    /// <summary>
    /// Creates a new Plant
    /// </summary>
    /// <param name="bc"> Color of the Leaves or Bloom </param>
    /// <param name="gf"> GrowthFactor (>1 will speed up the growth of all plants)</param>
    public PlantInfo(Color bc, float gf) {
        BloomColor = bc;
        GrowthFactor = gf;
        Damage = 0;
    }

    /// <summary>
    /// Creates a new Plant based ona existing Plant
    /// Takes over the Color and GrowthFactor
    /// Will NOT take over the Damage
    /// </summary>
    /// <param name="other"></param>
    public PlantInfo(PlantInfo other) {
        BloomColor = other.BloomColor;
        GrowthFactor = other.GrowthFactor;
        Damage = 0;    //Every new Plant is healthy
    }
}
