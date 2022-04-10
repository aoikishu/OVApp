using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Randomizer
{

    public static Ability RandomAbility()
    {
        List<Ability> abilities = JsonReader.Instance.abilities.Values.ToList();
        int i = Random.Range(0, abilities.Count);
        return new Ability(abilities[i]);
    }

    public static Weakness RandomWeakness()
    {
        List<Weakness> weaknesses = JsonReader.Instance.weaknesses.Values.ToList();
        int i = Random.Range(0, weaknesses.Count);
        return new Weakness(weaknesses[i]);
    }

    public static Perk RandomPerk()
    {
        List<Perk> perks = JsonReader.Instance.perks.Values.ToList();
        int i = Random.Range(0, perks.Count);
        return new Perk(perks[i]);
    }

    public static Flaw RandomFlaw()
    {
        List<Flaw> flaws = JsonReader.Instance.flaws.Values.ToList();
        int i = Random.Range(0, flaws.Count);
        return new Flaw(flaws[i]);
    }
}
