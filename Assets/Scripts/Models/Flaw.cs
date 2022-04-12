using System;
using System.Collections.Generic;

[Serializable]
public class Flaw : Stat
{
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
}

[Serializable]
public class FlawCollection
{
    public List<Flaw> Flaws;
}
