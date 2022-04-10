using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailsPanel : MonoBehaviour
{
    public static DetailsPanel Instance;
    public GameObject panel;
    public TextMeshProUGUI Title;
    public TMP_InputField Description;
    public TextMeshProUGUI LevelOrModifierLabel;
    public TextMeshProUGUI LevelOrModifierValue;
    public Slider LevelOrModifierSlider;
    private Stat stat;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenStat(Stat stat)
    {
        this.stat = stat;
        panel.SetActive(true);
        SetTitle(stat.Name);
        SetDescription(stat.Description);
        LevelOrModifierLabel.text = "Level";
    }

    public void Open(Ability ability)
    {
        OpenStat(ability);
        LevelOrModifierLabel.text = "Level";
        SetSlider(ability.Level, 1, 5);
    }

    public void Open(Weakness weakness)
    {
        OpenStat(weakness);
        LevelOrModifierLabel.text = "Level";
        SetSlider(weakness.Level, 1, 3);
    }

    public void Open(Perk perk)
    {
        OpenStat(perk);
        LevelOrModifierLabel.text = "Modifier";
        SetSlider(perk.Modifier, 0, 20);
        stat = perk;
    }

    public void Open(Flaw flaw)
    {
        OpenStat(flaw);
        LevelOrModifierLabel.text = "Modifier";
        SetSlider(flaw.Modifier, 0, 20);
        stat = flaw;
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void SetDescription(string value)
    {
        Description.text = value;
    }

    public void SetTitle(string value)
    {
        Title.text = value;
    }

    public void SetSlider(int value, int min, int max)
    {
        LevelOrModifierSlider.minValue = min;
        LevelOrModifierSlider.maxValue = max;
        LevelOrModifierSlider.value = value;
    }

    public void OnSliderChanged(float value)
    {
        LevelOrModifierValue.text = value.ToString();
        stat.Level = (int)value;
    }
}
