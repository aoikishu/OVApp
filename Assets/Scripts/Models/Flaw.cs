using System;
using System.Collections.Generic;

[Serializable]
public class Flaw : Stat
{
    [NonSerialized]
    public Attack parentAttack;
    public int Modifier;

    public Flaw(Flaw flaw)
    {
        Name = flaw.Name;
        Description = flaw.Description;
        Modifier = flaw.Modifier;
        Level = 1;
    }

    public Flaw(Flaw flaw, int level)
    {
        Name = flaw.Name;
        Description = flaw.Description;
        Modifier = flaw.Modifier;
        Level = level;
    }

    public bool isUnique()
    {
        if (Description.ToLower().Contains("for each time")) return false;
        return true;
    }
}

[Serializable]
public class FlawCollection
{
    public List<Flaw> Flaws;
}
