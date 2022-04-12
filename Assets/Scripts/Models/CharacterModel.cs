using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterModel
{
    public string Name;
    public int Defense;
    public int Health;
    public int Endurance;
    public int ThreatValue;
    public int NetLevel;

    public List<Ability> Abilities;
    public List<Weakness> Weaknesses;
    public List<Attack> Attacks;

    public CharacterModel()
    {
        Health = 40;
        Endurance = 40;
        NetLevel = 0;
    }

    public void AssignAbilitiesAndWeaknesses()
    {
        //System.Random random = new System.Random();

        // Up to 5 Abilities
        //int rand = random.Next(1,5);
        int abilityCount = (int)ControllerScript.Instance.abilitiesSlider.value;
        for (int i = 0; i < abilityCount; i++)
        {
            Ability ability = Randomizer.RandomAbility();
            NetLevel += ability.Level;
            Abilities.Add(ability);
        }

        int weaknessCount = (int)ControllerScript.Instance.weaknessesSlider.value;
        for (int i = 0; i < weaknessCount; i++)
        {
            Weakness weakness = Randomizer.RandomWeakness();
            NetLevel -= weakness.Level;
            Weaknesses.Add(weakness);
        }

        // Add Weaknesses until net level is the desired net level.
        //while (NetLevel > DesiredNetLevel)
        //{
        //    Weakness weakness = Randomizer.RandomWeakness();
        //    NetLevel -= weakness.Level;
        //    Weaknesses.Add(weakness);
        //}
    }
}