using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterModel : ISerializationCallbackReceiver
{
    public string Name;
    public int Defense;
    public int Health;
    public int Endurance;
    public int ThreatValue;
    public int NetLevel;

    public List<Ability> AbilityList;
    public List<Weakness> WeaknessList;

    public Dictionary<string, Ability> Abilities;
    public Dictionary<string, Weakness> Weaknesses;
    public List<Attack> Attacks;

    public CharacterModel()
    {
        Health = 40;
        Endurance = 40;
        NetLevel = 0;
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
        int threat = 0;
        int atk = 2;
        int def = 2;
        int dx = 1;
        int armored = 0;
        int hp = 0;
        int endurance = 0;

        if (Abilities.ContainsKey("Agile")) atk += Abilities["Agile"].Level;
        if (Abilities.ContainsKey("Combat Expert")) atk += Abilities["Combat Expert"].Level;
        if (Weaknesses.ContainsKey("Impaired")) atk -= Weaknesses["Impaired"].Level;
        if (Weaknesses.ContainsKey("Clumsy")) atk -= Weaknesses["Clumsy"].Level;
        
        if (Abilities.ContainsKey("Evasive")) def += Abilities["Evasive"].Level;
        if (Abilities.ContainsKey("Quick")) def += Abilities["Quick"].Level;
        if (Weaknesses.ContainsKey("Impaired")) def -= Weaknesses["Impaired"].Level;
        if (Weaknesses.ContainsKey("Slow")) def -= Weaknesses["Slow"].Level;

        if (Abilities.ContainsKey("Attack")) dx += Abilities["Attack"].Level;
        if (Abilities.ContainsKey("Armored")) armored = Abilities["Armored"].Level;

        if (Abilities.ContainsKey("Tough")) hp += Abilities["Tough"].Level;
        if (Abilities.ContainsKey("Vigorous")) endurance += Abilities["Vigorous"].Level;
        if (Abilities.ContainsKey("Frail")) hp += Abilities["Frail"].Level;
        if (Abilities.ContainsKey("Languid")) endurance += Abilities["Languid"].Level;

        threat = atk + def + dx + armored + hp + endurance;
        Debug.Log(threat);
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
    }

    public void OnBeforeSerialize()
    {
        AbilityList = Abilities.Values.ToList();
        WeaknessList = Weaknesses.Values.ToList();
    }
}