using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEffect
{
    public List<string> levelEffects;

    public LevelEffect(string level1, string level2, string level3)
    {
        levelEffects = new List<string>
        {
            level1,
            level2,
            level3
        };
    }

    public LevelEffect(string level1, string level2, string level3, string level4, string level5)
    {
        levelEffects = new List<string>
        {
            level1,
            level2,
            level3,
            level4,
            level5
        };
    }
}
