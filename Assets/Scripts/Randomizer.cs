using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Randomizer
{
    private const string RANDOM_CHARACTERS = "abcdefghijklmnopqrstuvwxyz0123456789";

    public static Ability RandomAbility()
    {
        List<Ability> abilities = ControllerScript.abilities.Values.ToList();
        int i = Random.Range(0, abilities.Count);
        return new Ability(abilities[i]);
    }

    public static Weakness RandomWeakness()
    {
        List<Weakness> weaknesses = ControllerScript.weaknesses.Values.ToList();
        int i = Random.Range(0, weaknesses.Count);
        return new Weakness(weaknesses[i]);
    }

    public static Perk RandomPerk()
    {
        List<Perk> perks = ControllerScript.perks.Values.ToList();
        int i = Random.Range(0, perks.Count);
        return new Perk(perks[i]);
    }

    public static Flaw RandomFlaw()
    {
        List<Flaw> flaws = ControllerScript.flaws.Values.ToList();
        int i = Random.Range(0, flaws.Count);
        return new Flaw(flaws[i]);
    }

    public static string RandomID()
    {
        string s = "";
        for(int i = 0; i < 12; i++)
        {
            s += RANDOM_CHARACTERS[Random.Range(0, RANDOM_CHARACTERS.Length)];
        }
        return s;
    }
}
