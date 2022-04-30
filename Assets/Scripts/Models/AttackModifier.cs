using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackModifier
{
    public readonly int enduranceCostPerRank;
    public readonly string description;

    public AttackModifier(string description, int enduranceCostPerRank)
    {
        this.description = description;
        this.enduranceCostPerRank = enduranceCostPerRank;
    }
}
