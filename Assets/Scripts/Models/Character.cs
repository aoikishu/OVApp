using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterModel model;
    private int atkCount = 0;

    private void Awake()
    {
        model = new CharacterModel
        {
            Abilities = new Dictionary<string, Ability>(),
            Weaknesses = new Dictionary<string, Weakness>(),
            Attacks = new List<string>()
        };
    }

    public void CreateCharacter()
    {
        model.AssignAbilitiesAndWeaknesses();
        
        for (int i = 0; i < 4; i++)
        {
            atkCount++;
            Attack attack = new Attack(model);
            attack.Name = $"Atk {atkCount}";
            attack.AssignPerksAndFlaws();
            attack.CalculateDX();
            attack.CalculateRoll();
            model.Attacks.Add(attack.ID);
        }
    }
}