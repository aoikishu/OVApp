using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DetailsPanel : MonoBehaviour
{
    public static DetailsPanel Instance;
    public GameObject panel;
    [SerializeField]
    private TextMeshProUGUI Title;
    [SerializeField]
    private TMP_InputField Description;
    [SerializeField]
    private TMP_InputField Notes;
    [SerializeField]
    private TextMeshProUGUI LevelOrModifierLabel;
    [SerializeField]
    private TextMeshProUGUI PgValue;
    [SerializeField]
    private TextMeshProUGUI LevelOrModifierValue;
    [SerializeField]
    private Slider LevelOrModifierSlider;
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
        Title.text = (stat.Name);
        Description.text = (stat.Description);
        PgValue.text = stat.Page.ToString();
    }

    public void Open(Ability ability)
    {
        OpenStat(ability);
        LevelOrModifierLabel.text = "Lv";
        SetSlider(ability.Level, 1, 5);
        Debug.Log($"Level {ability.Level}, Levels: {ability.Levels.Length}");
        Describe(ability);
    }

    public void Open(Weakness weakness)
    {
        OpenStat(weakness);
        LevelOrModifierLabel.text = "Lv";
        SetSlider(weakness.Level, 1, 3);
        Describe(weakness);
    }

    public void Open(Perk perk)
    {
        OpenStat(perk);
        LevelOrModifierLabel.text = "Mod";
        SetSlider(perk.Modifier, 0, 20);
        stat = perk;
    }

    public void Open(Flaw flaw)
    {
        OpenStat(flaw);
        LevelOrModifierLabel.text = "Mod";
        SetSlider(flaw.Modifier, 0, 20);
        stat = flaw;
    }

    public void Close()
    {
        panel.SetActive(false);
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
        if (stat is Ability) Describe(stat as Ability);
        else if (stat is Weakness) Describe(stat as Weakness);
    }

    private void Describe(Ability stat)
    {
        if (stat.Levels.Length > 0) {
            if (!string.IsNullOrEmpty(stat.Description)) Description.text = stat.Description + "\n" + stat.Levels[stat.Level - 1];
            else Description.text = stat.Levels[stat.Level - 1];
        }
    }

    private void Describe(Weakness stat)
    {
        if (stat.Levels.Length > 0)
        {
            if (!string.IsNullOrEmpty(stat.Description)) Description.text = stat.Description + "\n" + stat.Levels[stat.Level - 1];
            else Description.text = stat.Levels[stat.Level - 1];
        }
    }
}
