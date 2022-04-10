using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class ControllerScript : MonoBehaviour
{
    public static ControllerScript Instance;
    public GameObject characterPrefab;
    public DetailsPanel detailsPanel;
    public GameObject textPrefab;
    public GameObject listItemPrefab;
    public Transform contentPanel;
    public GameObject navPanel;
    public Slider abilitiesSlider;
    public Slider weaknessesSlider;
    public TextMeshProUGUI abilityCount;
    public TextMeshProUGUI weaknessCount;
    public TMP_InputField nameField;

    public Dictionary<string, Character> Characters;

    private void Awake()
    {
        Instance = this;
        Screen.SetResolution(1000, 600, false);
        Characters = new Dictionary<string, Character>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void OnApplicationQuit()
    {
        List<Character> characterList = Characters.Values.ToList();
        JsonWrite.SaveCharacters(characterList);
    }

    public void CreateCharacter()
    {
        GameObject characterGO = Instantiate(characterPrefab);
        Character character = characterGO.GetComponent<Character>();
        character.Name = nameField.text;

        try
        {
            Characters.Add(character.Name, character);
            // TODO: Not sure if coroutine is needed.
            Clear();
            StartCoroutine(Wait(character));
        }
        catch
        {
            Debug.LogError("Character already exists. Choose a new name.");
            Destroy(characterGO);
        }
    }

    IEnumerator Wait(Character character)
    {
        yield return new WaitForSeconds(0.1f);
        PopulateContent(character);
    }

    public void PopulateContent(Character character)
    {
        string text;
        foreach (Ability ability in character.Abilities)
        {
            SpawnAbility(ability);
        }

        foreach (Weakness weakness in character.Weaknesses)
        {
            SpawnWeakness(weakness);
        }

        int i = 0;
        foreach (Attack attack in character.Attacks)
        {
            i++;
            text = $"Attack {i}\t({attack.EnduranceCost})";
            SpawnAttack(text, attack);
            foreach (Perk perk in attack.perks)
            {
                text = $"\t+ {perk.Name}\t({perk.Modifier})";
                SpawnPerk(perk);
            }
            foreach (Flaw flaw in attack.flaws)
            {
                text = $"\t- {flaw.Name}\t({flaw.Modifier})";
                SpawnFlaw(flaw);
            }
        }
    }

    public void ListCharacters()
    {

    }

    public void ListAbilities()
    {
        Clear();
        foreach (Ability ability in JsonReader.Instance.abilities.Values)
        {
            SpawnAbility(ability);
        }
    }

    public void ListWeaknesses()
    {
        Clear();
        foreach (Weakness weakness in JsonReader.Instance.weaknesses.Values)
        {
            SpawnWeakness(weakness);
        }
    }

    public void ListPerks()
    {
        Clear();
        foreach (Perk perk in JsonReader.Instance.perks.Values)
        {
            SpawnPerk(perk);
        }
    }

    public void ListFlaws()
    {
        Clear();
        foreach (Flaw flaw in JsonReader.Instance.flaws.Values)
        {
            SpawnFlaw(flaw);
        }
    }

    public void SpawnAttack(string value, Attack attack)
    {
        GameObject textGO = Instantiate(textPrefab, contentPanel);
        textGO.name = "Attack";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = value;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(attack); });
    }

    public void SpawnAbility(Ability ability)
    {
        GameObject textGO = Instantiate(textPrefab, contentPanel);
        textGO.name = "Ability";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = ability.Name;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(ability); });
    }

    public void SpawnWeakness(Weakness weakness)
    {
        GameObject textGO = Instantiate(textPrefab, contentPanel);
        textGO.name = "Weakness";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = weakness.Name;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(weakness); });
    }

    public void SpawnPerk(Perk perk)
    {
        GameObject textGO = Instantiate(textPrefab, contentPanel);
        textGO.name = "Perk";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t+ {perk.Name} ({perk.Modifier})";

        textGO.GetComponent<Button>().onClick.AddListener(delegate{ OnClick(perk); });
    }

    public void SpawnFlaw(Flaw flaw)
    {
        GameObject textGO = Instantiate(textPrefab, contentPanel);
        textGO.name = "Flaw";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t- {flaw.Name} ({flaw.Modifier})";

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(flaw); });
    }

    public void Clear()
    {
        for(int i = contentPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
    }

    public void OnClick(Attack attack)
    {
        //detailsPanel.Open(attack);
    }

    public void OnClick(Ability ability)
    {
        detailsPanel.Open(ability);
    }

    public void OnClick(Weakness weakness)
    {
        detailsPanel.Open(weakness);
    }

    public void OnClick(Perk perk)
    {
        detailsPanel.Open(perk);
    }

    public void OnClick(Flaw flaw)
    {
        detailsPanel.Open(flaw);
    }

    public void ToogleNavPanel()
    {
        navPanel.SetActive(!navPanel.activeSelf);
    }

    public void OnAbilitySliderChanged(float value)
    {
        abilityCount.text = "Abilities: " + value.ToString();
    }

    public void OnWeaknessSliderChanged(float value)
    {
        weaknessCount.text = "Weaknesses: " + value.ToString();
    }
}
