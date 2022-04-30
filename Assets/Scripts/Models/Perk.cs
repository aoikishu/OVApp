using System;
using System.Collections.Generic;

[Serializable]
public class Perk : Stat
{
    [NonSerialized]
    public Attack parentAttack;
    public int Modifier;

    public Perk(Perk perk)
    {
        Name = perk.Name;
        Description = perk.Description;
        Modifier = perk.Modifier;
        Level = 1;
    }

    public Perk(Perk perk, int level)
    {
        Name = perk.Name;
        Description = perk.Description;
        Modifier = perk.Modifier;
        Level = level;
    }

    public bool isUnique()
    {
        if (Description.ToLower().Contains("for each time")) return false;
        return true;
    }
}

[Serializable]
public class PerkCollection
{
    public List<Perk> Perks;
}