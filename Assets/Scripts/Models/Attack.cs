using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Attack : ISerializationCallbackReceiver
{
    public string ID;
    public string Name;
    [NonSerialized]
    public int Roll;
    [NonSerialized]
    public int DX;
    [NonSerialized]
    public int EnduranceCost;
    [NonSerialized]
    private CharacterModel model;

    public List<Perk> PerksList;
    public List<Flaw> FlawsList;
    public Dictionary<string, Perk> Perks;
    public Dictionary<string, Flaw> Flaws;

    public Attack(CharacterModel model)
    {
        ID = Randomizer.RandomID();
        while (ControllerScript.attacks.ContainsKey(ID)) ID = Randomizer.RandomID();
        ControllerScript.attacks.Add(ID, this);
        Perks = new Dictionary<string, Perk>();
        Flaws = new Dictionary<string, Flaw>();
        this.model = model;
    }

    public void AssignPerksAndFlaws()
    {
        //if (Perks.Count >= ControllerScript.perks.Count) return;
        if (isWill()) Perks.Add("Will Attack", new Perk(ControllerScript.perks["Will Attack"]));
        for (int i = 0; i < Random.Range(1, 6); i++) AddRandomPerk();

        //if (Flaws.Count >= ControllerScript.flaws.Count) return;

        while (EnduranceCost > 40) AddRandomFlaw();
        for (int i = 0; i < Random.Range(0, 4); i++) AddRandomFlaw();
    }

    private void AddRandomPerk()
    {
        bool isNew = true;
        Perk perk = Randomizer.RandomPerk();
        if (perk.Modifier > 5) perk = Randomizer.RandomPerk(); // Greater chance perk will be 5 or less.
        if (perk.Modifier > 15) perk = Randomizer.RandomPerk(); // Greater chance perk will be 15 or less.
        while (PickANewPerk(perk)) perk = Randomizer.RandomPerk(); // If perk is already added and is unique, pick a new perk.
        if (Perks.ContainsKey(perk.Name))
        {
            Debug.Log($"Doubling perk {perk.Name}");
            perk = Perks[perk.Name];
            perk.Level++;
            isNew = false;
        }
        int cost = perk.Modifier * perk.Level;
        if (EnduranceCost + cost > 40)
        {
            perk.Level--;
            return;
        }
        EnduranceCost += cost;

        if (isNew)
        {
            perk.parentAttack = this;
            Perks.Add(perk.Name, perk);
        }
    }

    private void AddRandomFlaw()
    {
        bool isNew = true;
        Flaw flaw = Randomizer.RandomFlaw();
        while (PickANewFlaw(flaw)) flaw = Randomizer.RandomFlaw(); // If random flaw is already chosen, choose another.
        if (Flaws.ContainsKey(flaw.Name))
        {
            Debug.Log($"Doubling flaw {flaw.Name}");
            flaw = Flaws[flaw.Name];
            flaw.Level++;
            isNew = false;
        }

        int cost = flaw.Modifier * flaw.Level;
        if (EnduranceCost + cost < 0) return;
        EnduranceCost += cost;
        
        if (isNew)
        {
            flaw.parentAttack = this;
            Flaws.Add(flaw.Name, flaw);
        }
    }

    internal void CalculateDX()
    {
        DX = 1;
        foreach (Perk perk in Perks.Values)
        {
            if (perk.Name == "Accurate") Roll += perk.Level;
            if (perk.Name == "Effective") DX += perk.Level;
        }
    }

    internal void CalculateEnduranceCost()
    {
        EnduranceCost = 0;
        foreach (Perk perk in Perks.Values) EnduranceCost += perk.Modifier * perk.Level;
        foreach (Flaw flaw in Flaws.Values) EnduranceCost += flaw.Modifier * flaw.Level;
    }

    internal void CalculateRoll()
    {
        Roll = 2;
        foreach (Flaw flaw in Flaws.Values)
        {
            if (flaw.Name == "Inaccurate") Roll -= flaw.Level;
            if (flaw.Name == "Ineffective") Roll -= flaw.Level;
        }
    }

    public void RemovePerkOrFlaw(Stat stat)
    {
        if (stat is Perk) Perks.Remove(stat.Name);
        else if (stat is Flaw) Flaws.Remove(stat.Name);
        CalculateEnduranceCost();
    }

    public void OnAfterDeserialize()
    {
        Perks = new Dictionary<string, Perk>();
        foreach (Perk perk in PerksList) Perks.Add(perk.Name, perk);

        Flaws = new Dictionary<string, Flaw>();
        foreach (Flaw flaw in FlawsList) Flaws.Add(flaw.Name, flaw);

        CalculateDX();
        CalculateEnduranceCost();
        CalculateRoll();
    }

    public void OnBeforeSerialize()
    {
        PerksList = new List<Perk>(Perks.Values);
        FlawsList = new List<Flaw>(Flaws.Values);
    }

    private bool PickANewPerk(Perk perk)
    {
        if (Perks.ContainsKey(perk.Name)) {
            if (perk.isUnique()) return true; // If you can't take a perk multiple times, get a new one.
            else Debug.Log($"{perk.Name} isn't unique");
        }
        if (perk.Name == "Redirectable" &&  !isRanged()) return true; // Redirectable only works with ranged.
        if (perk.Name == "Ranged" && !isRanged()) return true;
        if (perk.Name == "Ranged, Strength Powered" && !isRanged()) return true;
        if (perk.Name == "Will Attack" && !isWill()) return true;

        return false;
    }

    private bool PickANewFlaw(Flaw flaw)
    {
        if (Flaws.ContainsKey(flaw.Name))
        {
            if (flaw.isUnique()) return true; // If you can't take a flaw multiple times, get a new one.
            else Debug.Log($"{flaw.Name} isn't unique");
        }
        if (flaw.Name == "Ranged, Strength Powered" && !Flaws.ContainsKey("Strong")) return true; // Strength powered pointless without Strong

        return false;
    }

    private bool isRanged()
    {
        if (Perks.ContainsKey("Ranged") || Perks.ContainsKey("Ranged, Strength Powered")) return true;
        return false;
    }

    private bool isWill()
    {
        if (model.Abilities.ContainsKey("Iron Willed")) return true;
        return false;
    }
}
