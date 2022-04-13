using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterModel model;

    private void Awake()
    {
        model = new CharacterModel
        {
            Abilities = new Dictionary<string, Ability>(),
            Weaknesses = new Dictionary<string, Weakness>(),
            Attacks = new List<Attack>()
        };
    }

    public void CreateCharacter()
    {
        for (int i = 0; i < 4; i++)
        {
            Attack attack = new Attack();
            model.Attacks.Add(attack);
        }

        model.AssignAbilitiesAndWeaknesses();
    }
}