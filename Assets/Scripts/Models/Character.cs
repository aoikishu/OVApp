using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterModel model;

    private void Awake()
    {
        model = new CharacterModel();
        model.Abilities = new List<Ability>();
        model.Weaknesses = new List<Weakness>();
        model.Attacks = new List<Attack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Attack attack = new Attack();
            model.Attacks.Add(attack);
        }

        model.AssignAbilitiesAndWeaknesses();
    }
}