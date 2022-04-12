using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public static Dictionary<string, Ability> ReadAbilities(string path)
    {
        Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return null;
        }
        string jsonString = File.ReadAllText(path);
        AbilityCollection abilityCollection = JsonUtility.FromJson<AbilityCollection>(jsonString);
        foreach (Ability ability in abilityCollection.Abilities)
        {
            ability.Level = 1;
            abilities.Add(ability.Name, ability);
        }
        return abilities;
    }

    public static Dictionary<string, Weakness> ReadWeaknesses(string path)
    {
        Dictionary<string, Weakness> weaknesses = new Dictionary<string, Weakness>();
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return null;
        }
        string jsonString = File.ReadAllText(path);
        WeaknessCollection weaknessCollection = JsonUtility.FromJson<WeaknessCollection>(jsonString);
        foreach (Weakness weakness in weaknessCollection.Weaknesses)
        {
            weakness.Level = 1;
            weaknesses.Add(weakness.Name, weakness);
        }
        return weaknesses;
    }

    public static Dictionary<string, Perk> ReadPerks(string path)
    {
        Dictionary<string, Perk> perks = new Dictionary<string, Perk>();
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return null;
        }
        string jsonString = File.ReadAllText(path);
        PerkCollection perkCollection = JsonUtility.FromJson<PerkCollection>(jsonString);
        foreach (Perk perk in perkCollection.Perks)
        {
            perks.Add(perk.Name, perk);
        }
        return perks;
    }

    public static Dictionary<string, Flaw> ReadFlaws(string path)
    {
        Dictionary<string, Flaw> flaws = new Dictionary<string, Flaw>();
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return null;
        }
        string jsonString = File.ReadAllText(path);
        FlawCollection flawCollection = JsonUtility.FromJson<FlawCollection>(jsonString);
        foreach (Flaw flaw in flawCollection.Flaws)
        {
            flaws.Add(flaw.Name, flaw);
        }
        return flaws;
    }

    public static Dictionary<string, Character> ReadCharacters()
    {
        Dictionary<string, Character> characters = new Dictionary<string, Character>();
        string path = Application.streamingAssetsPath + "/Characters/";
        string[] files = Directory.GetFiles(path, "*.json");
        foreach (string file in files)
        {
            Debug.Log(file);
            GameObject characterGO = Instantiate(ControllerScript.Instance.characterPrefab);
            Character character = characterGO.GetComponent<Character>();
            character.model = ReadCharacter(file);
            characters.Add(character.model.Name, character);
        }

        return characters;
    }

    public static CharacterModel ReadCharacter(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return null;
        }

        string jsonString = File.ReadAllText(path);
        return JsonUtility.FromJson<CharacterModel>(jsonString);
    }

    public static List<string> ReadNames(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File {path} doesn't exist.");
            return null;
        }
        string allNames = File.ReadAllText(path);
        string[] names = allNames.Split('\n');

        return new List<string>(names);
    }
}
