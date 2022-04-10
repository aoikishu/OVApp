using System;
using System.Collections.Generic;

[Serializable]
public class Weakness : Stat
{

    public Weakness(Weakness weakness)
    {
        Name = weakness.Name;
        Description = weakness.Description;
        Level = 1;
    }

    public Weakness(Weakness weakness, int level)
    {
        Name = weakness.Name;
        Description = weakness.Description;
        Level = level;
    }
}

[Serializable]
public class WeaknessCollection
{
    public List<Weakness> Weaknesses;
}