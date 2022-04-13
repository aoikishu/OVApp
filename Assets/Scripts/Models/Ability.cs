using System;
using System.Collections.Generic;

[Serializable]
public class Ability : Stat
{
    public string[] Levels;

    public Ability(Ability ability)
    {
        Copy(ability);
    }

    public Ability(Ability ability, int level)
    {
        Copy(ability);
        Level = level;
    }

    public void Copy(Ability ability)
    {
        Name = ability.Name;
        Description = ability.Description;
        Page = ability.Page;
        Levels = ability.Levels;
        Level = 1;
    }
}

[Serializable]
public class AbilityCollection
{
    public List<Ability> Abilities;
}