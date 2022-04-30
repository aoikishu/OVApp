using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterModel : ISerializationCallbackReceiver
{
    public string Name;
    [NonSerialized]
    public int Armored;
    [NonSerialized]
    public int Atk;
    [NonSerialized]
    public int Defense;
    [NonSerialized]
    public int DX;
    [NonSerialized]
    public int Health;
    [NonSerialized]
    public int Endurance;
    [NonSerialized]
    public int ThreatValue;
    [NonSerialized]
    public int NetLevel;

    public List<Ability> AbilityList;
    public List<Weakness> WeaknessList;

    public Dictionary<string, Ability> Abilities;
    public Dictionary<string, Weakness> Weaknesses;
    public List<string> Attacks;

    public CharacterModel()
    {
        Health = 40;
        Endurance = 40;
        NetLevel = 0;
    }

    public void AddAbility(Ability ability)
    {
        if (Abilities.ContainsKey(ability.Name))
        {
            if (Abilities[ability.Name].Level < 5) Abilities[ability.Name].Level++;
        }
        else Abilities.Add(ability.Name, new Ability(ability));
    }

    public void AddWeakness(Weakness weakness)
    {
        if (Weaknesses.ContainsKey(weakness.Name))
        {
            if (Weaknesses[weakness.Name].Level < 3) Weaknesses[weakness.Name].Level++;
        }
        else Weaknesses.Add(weakness.Name, new Weakness(weakness));
    }

    public void AssignAbilitiesAndWeaknesses()
    {
        int abilityCount = (int)ControllerScript.Instance.abilitiesSlider.value;
        for (int i = 0; i < abilityCount; i++)
        {
            Ability ability = Randomizer.RandomAbility();
            ability.Level = 1;
            if (Abilities.ContainsKey(ability.Name))
            {
                if (Abilities[ability.Name].Level < 5) Abilities[ability.Name].Level++;
                else abilityCount--;
            }
            else Abilities.Add(ability.Name, ability);
        }

        int weaknessCount = (int)ControllerScript.Instance.weaknessesSlider.value;
        for (int i = 0; i < weaknessCount; i++)
        {
            Weakness weakness = Randomizer.RandomWeakness();
            weakness.Level = 1;
            if (Weaknesses.ContainsKey(weakness.Name))
            {
                if (Weaknesses[weakness.Name].Level < 5) Weaknesses[weakness.Name].Level++;
                else weaknessCount--;
            }
            else Weaknesses.Add(weakness.Name, weakness);
        }

        CalculateThreat();
    }

    public void CalculateThreat()
    {
        ThreatValue = 0;
        Armored = 0;
        Atk = 2;
        Defense = 2;
        DX = 1;
        Endurance = 0;
        Health = 0;

        if (Abilities.ContainsKey("Agile")) Atk += Abilities["Agile"].Level;
        if (Abilities.ContainsKey("Combat Expert")) Atk += Abilities["Combat Expert"].Level;
        if (Weaknesses.ContainsKey("Impaired")) Atk -= Weaknesses["Impaired"].Level;
        if (Weaknesses.ContainsKey("Clumsy")) Atk -= Weaknesses["Clumsy"].Level;
        
        if (Abilities.ContainsKey("Evasive")) Defense += Abilities["Evasive"].Level;
        if (Abilities.ContainsKey("Quick")) Defense += Abilities["Quick"].Level;
        if (Weaknesses.ContainsKey("Impaired")) Defense -= Weaknesses["Impaired"].Level;
        if (Weaknesses.ContainsKey("Slow")) Defense -= Weaknesses["Slow"].Level;

        if (Abilities.ContainsKey("Attack")) DX += Abilities["Attack"].Level;
        if (Abilities.ContainsKey("Armored")) Armored = Abilities["Armored"].Level;

        if (Abilities.ContainsKey("Tough")) Health += Abilities["Tough"].Level;
        if (Abilities.ContainsKey("Vigorous")) Endurance += Abilities["Vigorous"].Level;
        if (Abilities.ContainsKey("Frail")) Health += Abilities["Frail"].Level;
        if (Abilities.ContainsKey("Languid")) Endurance += Abilities["Languid"].Level;

        ThreatValue = Atk + Defense + DX + Armored + Health + Endurance;
        Debug.Log("TV: " + ThreatValue);
    }

    public void OnAfterDeserialize()
    {
        Abilities = new Dictionary<string, Ability>();
        foreach(Ability ability in AbilityList)
        {
            Abilities.Add(ability.Name, ability);
        }

        Weaknesses = new Dictionary<string, Weakness>();
        foreach (Weakness weakness in WeaknessList)
        {
            Weaknesses.Add(weakness.Name, weakness);
        }

        CalculateThreat();
    }

    public void OnBeforeSerialize()
    {
        AbilityList = new List<Ability>(Abilities.Values);
        WeaknessList = new List<Weakness>(Weaknesses.Values);
    }

    public void RemoveAbiityOrWeakness(Stat stat)
    {
        if (stat is Ability) Abilities.Remove(stat.Name);
        else if (stat is Weakness) Weaknesses.Remove(stat.Name);
        CalculateThreat();
    }
}