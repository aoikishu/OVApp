using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    private int diceCount;
    public int DiceCount
    {
        get => diceCount;
        set
        {
            diceCount = value;
            diceCountField.text = diceCount.ToString();
        }
    }
    public TMP_InputField diceCountField;

    private void Awake()
    {
        DiceCount = 1;
    }

    public void DiceUp()
    {
        DiceCount++;
        if (DiceCount == 0) DiceCount++;
    }

    public void DiceDown()
    {
        DiceCount--;
        if (DiceCount == 0) DiceCount--;
    }

    public void OnRoll()
    {
        int ones = 0;
        int twos = 0;
        int threes = 0;
        int fours = 0;
        int fives = 0;
        int sixes = 0;

        if (DiceCount > 0)
        {
            for (int i = 0; i < DiceCount; i++)
            {
                int result = Random.Range(1, 7);
                HistoryPanel.Instance.AddText(result.ToString(), Color.black);
                Debug.Log($"Roll {i + 1}: {result}");
                switch (result)
                {
                    case 1: ones++; break;
                    case 2: twos++; break;
                    case 3: threes++; break;
                    case 4: fours++; break;
                    case 5: fives++; break;
                    case 6: sixes++; break;
                }
            }

            twos *= 2;
            threes *= 3;
            fours *= 4;
            fives *= 5;
            sixes *= 6;
            int[] numbers =
            {
                ones, twos, threes, fours, fives, sixes
            };

            HistoryPanel.Instance.AddText(LargestPositive(numbers).ToString(), Color.green);
        }
        else
        {
            int lowest = 0;
            for (int i = DiceCount; i < 0; i++)
            {
                int result = Random.Range(-6, 0);
                HistoryPanel.Instance.AddText(result.ToString(), Color.black);
                Debug.Log($"Roll {i + 1}: {result}");
                if (result < lowest) lowest = result;
            }
            HistoryPanel.Instance.AddText(lowest.ToString(), Color.red);
        }
    }

    public static int LargestPositive(int[] numbers)
    {
        int greatest = 0;

        foreach(int x in numbers)
        {
            if (x > greatest) greatest = x;
        }

        return greatest;
    }
}
