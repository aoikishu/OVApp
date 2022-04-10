using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public static JsonReader Instance;
    public Dictionary<string, Ability> abilities;
    public Dictionary<string, Weakness> weaknesses;
    public Dictionary<string, Perk> perks;
    public Dictionary<string, Flaw> flaws;

    private void Awake()
    {
        Instance = this;
        abilities = new Dictionary<string, Ability>();
        weaknesses = new Dictionary<string, Weakness>();
        perks = new Dictionary<string, Perk>();
        flaws = new Dictionary<string, Flaw>();

        ReadAbilities(Application.streamingAssetsPath + "/Abilities.json");
        ReadWeaknesses(Application.streamingAssetsPath + "/Weaknesses.json");
        ReadPerks(Application.streamingAssetsPath + "/Perks.json");
        ReadFlaws(Application.streamingAssetsPath + "/Flaws.json");
    }

    private void ReadAbilities(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return;
        }

        string jsonString = File.ReadAllText(path);
        AbilityCollection abilityCollection = JsonUtility.FromJson<AbilityCollection>(jsonString);
        foreach (Ability ability in abilityCollection.Abilities)
        {
            abilities.Add(ability.Name, ability);
        }
    }

    private void ReadWeaknesses(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return;
        }

        string jsonString = File.ReadAllText(path);
        WeaknessCollection weaknessCollection = JsonUtility.FromJson<WeaknessCollection>(jsonString);
        foreach (Weakness weakness in weaknessCollection.Weaknesses)
        {
            weaknesses.Add(weakness.Name, weakness);
        }
    }

    private void ReadPerks(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return;
        }

        string jsonString = File.ReadAllText(path);
        PerkCollection perkCollection = JsonUtility.FromJson<PerkCollection>(jsonString);
        foreach (Perk perk in perkCollection.Perks)
        {
            perks.Add(perk.Name, perk);
        }
    }

    private void ReadFlaws(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return;
        }

        string jsonString = File.ReadAllText(path);
        FlawCollection flawCollection = JsonUtility.FromJson<FlawCollection>(jsonString);
        foreach (Flaw flaw in flawCollection.Flaws)
        {
            flaws.Add(flaw.Name, flaw);
        }
    }
}
