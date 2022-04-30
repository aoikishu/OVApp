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
    private TMP_InputField Note;
    [SerializeField]
    private TextMeshProUGUI LevelOrModifierLabel;
    [SerializeField]
    private TextMeshProUGUI PgValue;
    [SerializeField]
    private TextMeshProUGUI LevelOrModifierValue;
    [SerializeField]
    private Slider LevelOrModifierSlider;
    private Attack currentAttack;
    private Stat stat;
    [SerializeField]
    private Sprite addSprite;
    [SerializeField]
    private Sprite deleteSprite;
    [SerializeField]
    private Button addDeleteButton;

    private Character character => ControllerScript.currentCharacter;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenStat(Stat stat)
    {
        currentAttack = null;
        this.stat = stat;
        panel.SetActive(true);
        Title.text = (stat.Name);
        Description.text = (stat.Description);
        PgValue.text = stat.Page.ToString();
        if (stat.Note != null) Note.text = stat.Note;
    }

    public void Open(Ability ability)
    {
        OpenStat(ability);
        LevelOrModifierLabel.text = "Lv";
        SetSlider(ability.Level, 1, 5);
        Describe(ability);
        if (character != null)
        {
            addDeleteButton.gameObject.SetActive(true);
            if (character.model.Abilities.ContainsKey(ability.Name)) SwitchToDelete();
            else SwitchToAdd();
        }
    }

    public void Open(Weakness weakness)
    {
        OpenStat(weakness);
        LevelOrModifierLabel.text = "Lv";
        SetSlider(weakness.Level, 1, 3);
        Describe(weakness);
        if (character != null)
        {
            addDeleteButton.gameObject.SetActive(true);
            if (character.model.Weaknesses.ContainsKey(weakness.Name)) SwitchToDelete();
            else SwitchToAdd();
        }
    }

    private void SwitchToDelete()
    {
        addDeleteButton.GetComponent<Image>().sprite = deleteSprite;
        addDeleteButton.onClick.AddListener(delegate { Delete(); });
    }

    private void SwitchToAdd()
    {
        addDeleteButton.GetComponent<Image>().sprite = addSprite;
        addDeleteButton.onClick.AddListener(delegate { Add(); });
    }

    public void Open(Perk perk)
    {
        addDeleteButton.gameObject.SetActive(false);
        OpenStat(perk);
        LevelOrModifierLabel.text = "Mod";
        SetSlider(perk.Modifier, 0, 40);
        stat = perk;
        currentAttack = perk.parentAttack;
    }

    public void Open(Flaw flaw)
    {
        addDeleteButton.gameObject.SetActive(false);
        OpenStat(flaw);
        LevelOrModifierLabel.text = "Mod";
        SetSlider(flaw.Modifier, -40, 0);
        stat = flaw;
        currentAttack = flaw.parentAttack;
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void Add()
    {
        if (ControllerScript.currentCharacter != null && stat != null)
        {
            if (stat is Ability) character.model.AddAbility(stat as Ability);
            if (stat is Weakness) character.model.AddWeakness(stat as Weakness);
            ControllerScript.Instance.PopulateContent(character);
        }
    }

    public void Delete()
    {
        if (character != null)
        {
            if (currentAttack != null) currentAttack.RemovePerkOrFlaw(stat);
            else character.model.RemoveAbiityOrWeakness(stat);
            ControllerScript.Instance.PopulateContent(character);
        }
        Close();
    }

    public void SetSlider(int value, int min, int max)
    {
        LevelOrModifierSlider.minValue = min;
        LevelOrModifierSlider.maxValue = max;
        LevelOrModifierSlider.value = value;
    }

    public void OnNoteFieldChanged(string value)
    {
        stat.Note = value;
    }

    public void OnSliderChanged(float value)
    {
        if (character != null) return;
        LevelOrModifierValue.text = value.ToString();
        if (stat is Ability || stat is Weakness)
        {
            stat.Level = (int)value;
            if (stat is Ability) Describe(stat as Ability);
            else if (stat is Weakness) Describe(stat as Weakness);
        }
        else if (stat is Perk || stat is Flaw)
        {
            if (stat is Perk)
            {
                (stat as Perk).Modifier = (int)value;
                (stat as Perk).parentAttack.CalculateEnduranceCost();
            }
            else if (stat is Flaw)
            {
                (stat as Flaw).Modifier = (int)value;
                (stat as Flaw).parentAttack.CalculateEnduranceCost();
            }
        }
        ControllerScript.Instance.PopulateContent(character);
    }

    private void Describe(Ability stat)
    {
        if (stat.Levels == null) return;
        if (stat.Levels.Length > 0) {
            if (!string.IsNullOrEmpty(stat.Description)) Description.text = stat.Description + "\n" + stat.Levels[stat.Level - 1];
            else Description.text = stat.Levels[stat.Level - 1];
        }

        var textComponent = Description.textComponent;
        textComponent.rectTransform.anchoredPosition = new Vector2(textComponent.rectTransform.anchoredPosition.x, 0);
    }

    private void Describe(Weakness stat)
    {
        if (stat.Levels == null) return;
        if (stat.Levels.Length > 0) {
            if (!string.IsNullOrEmpty(stat.Description)) Description.text = stat.Description + "\n" + stat.Levels[stat.Level - 1];
            else Description.text = stat.Levels[stat.Level - 1];
        }

        var textComponent = Description.textComponent;
        textComponent.rectTransform.anchoredPosition = new Vector2(textComponent.rectTransform.anchoredPosition.x, 0);
    }
}
