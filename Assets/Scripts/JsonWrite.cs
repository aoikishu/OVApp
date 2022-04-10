using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonWrite
{
    public static void SaveCharacters(List<Character> characterList)
    {
        foreach(Character character in characterList)
        {
            string path = Application.streamingAssetsPath + "/" + character.Name + ".json";
            string json = JsonUtility.ToJson(character, true);
            File.WriteAllText(path, json);
        }
    }
}
