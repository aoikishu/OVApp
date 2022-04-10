using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Attack
{
    public int Roll;
    public int DX;
    public int EnduranceCost;

    public List<Perk> perks;
    public List<Flaw> flaws;

    public Attack()
    {
        Roll = 2;
        DX = 1;

        perks = new List<Perk>();
        flaws = new List<Flaw>();
        AssignPerksAndFlaws();

        foreach(Perk perk in perks)
        {
            if (perk.Name == "Accurate") Roll += perk.Level;
            if (perk.Name == "Effective") DX += perk.Level;
        }

        foreach(Flaw flaw in flaws)
        {
            if (flaw.Name == "Inaccurate") Roll -= flaw.Level;
            if (flaw.Name == "Ineffective") Roll -= flaw.Level;
        }
    }

    public void AssignPerksAndFlaws()
    {
        for (int i = 0; i < Random.Range(1, 6); i++)
        {
            Perk perk = Randomizer.RandomPerk();
            while(perks.Contains(perk))
            {
                perk = Randomizer.RandomPerk();
            }
            EnduranceCost += perk.Modifier * perk.Level;
            perks.Add(perk);
        }

        while (EnduranceCost > 40)
        {
            Flaw flaw = Randomizer.RandomFlaw();
            while (flaws.Contains(flaw))
            {
                flaw = Randomizer.RandomFlaw();
            }
            EnduranceCost += flaw.Modifier * flaw.Level;
            flaws.Add(flaw);
        }

        for (int i = 0; i < Random.Range(0, 4); i++)
        {
            Flaw flaw = Randomizer.RandomFlaw();
            while (flaws.Contains(flaw))
            {
                flaw = Randomizer.RandomFlaw();
            }
            int cost = flaw.Modifier * flaw.Level;
            if (EnduranceCost + cost < 0) return;
            else
            {
                EnduranceCost += cost;
                flaws.Add(flaw);
            }
        }
    }
}
