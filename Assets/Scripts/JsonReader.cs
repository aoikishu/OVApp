using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public static Dictionary<string, Ability> ReadAbilities(string path)
    {
        Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
        AbilityCollection abilityCollection = JsonUtility.FromJson<AbilityCollection>(Read(path));
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
        WeaknessCollection weaknessCollection = JsonUtility.FromJson<WeaknessCollection>(Read(path));
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
        PerkCollection perkCollection = JsonUtility.FromJson<PerkCollection>(Read(path));
        foreach (Perk perk in perkCollection.Perks)
        {
            perks.Add(perk.Name, perk);
        }
        return perks;
    }

    public static Dictionary<string, Flaw> ReadFlaws(string path)
    {
        Dictionary<string, Flaw> flaws = new Dictionary<string, Flaw>();
        FlawCollection flawCollection = JsonUtility.FromJson<FlawCollection>(Read(path));
        foreach (Flaw flaw in flawCollection.Flaws)
        {
            flaws.Add(flaw.Name, flaw);
        }
        return flaws;
    }

    public static Dictionary<string, Character> ReadCharacters()
    {
        Dictionary<string, Character> characters = new Dictionary<string, Character>();
        string path = ControllerScript.CONST_PATH + "/Characters/";
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
        return JsonUtility.FromJson<CharacterModel>(Read(path));
    }

    public static Dictionary<string, Attack> ReadAttacks()
    {
        Dictionary<string, Attack> attacks = new Dictionary<string, Attack>();
        string path = ControllerScript.CONST_PATH + "/Attacks/";
        string[] files = Directory.GetFiles(path, "*.json");
        foreach (string file in files)
        {
            Debug.Log(file);
            Attack attack = ReadAttack(file);
            attacks.Add(attack.ID, attack);
        }

        return attacks;
    }

    public static Attack ReadAttack(string path)
    {
        return JsonUtility.FromJson<Attack>(Read(path));
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

    private static string Read(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            return sr.ReadToEnd();
        }
    }
}
