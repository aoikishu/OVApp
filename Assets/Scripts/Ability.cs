using System;
using System.Collections.Generic;

[Serializable]
public class Ability : Stat
{

    public Ability(Ability ability)
    {
        Name = ability.Name;
        Description = ability.Description;
        Level = 1;
    }

    public Ability(Ability ability, int level)
    {
        Name = ability.Name;
        Description = ability.Description;
        Level = level;
    }
}

[Serializable]
public class AbilityCollection
{
    public List<Ability> Abilities;
}