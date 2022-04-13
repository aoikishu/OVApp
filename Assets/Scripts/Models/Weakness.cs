using System;
using System.Collections.Generic;

[Serializable]
public class Weakness : Stat
{
    public string[] Levels;

    public Weakness(Weakness weakness)
    {
        Copy(weakness);
    }

    public Weakness(Weakness weakness, int level)
    {
        Copy(weakness);
        Level = level;
    }

    public void Copy(Weakness weakness)
    {
        Name = weakness.Name;
        Description = weakness.Description;
        Page = weakness.Page;
        Levels = weakness.Levels;
        Level = 1;
    }
}

[Serializable]
public class WeaknessCollection
{
    public List<Weakness> Weaknesses;
}