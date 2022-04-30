using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonWrite
{
    public static void SaveAttacks(List<Attack> attackList)
    {
        foreach (Attack attack in attackList)
        {
            string path = ControllerScript.CONST_PATH + "/Attacks/" + attack.ID + ".json";
            string json = JsonUtility.ToJson(attack, true);
            File.WriteAllText(path, json);
        }
    }

    public static void SaveCharacters(List<Character> characterList)
    {
        foreach(Character character in characterList)
        {
            string path = ControllerScript.CONST_PATH + "/Characters/" + character.model.Name + ".json";
            string json = JsonUtility.ToJson(character.model, true);
            File.WriteAllText(path, json);
        }
    }
}
