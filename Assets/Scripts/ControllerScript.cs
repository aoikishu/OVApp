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
    public GameObject listItemPrefab;
    public Transform contentPanel;
    public GameObject navPanel;
    public Slider abilitiesSlider;
    public Slider weaknessesSlider;
    public TextMeshProUGUI abilityCount;
    public TextMeshProUGUI weaknessCount;
    public TMP_InputField nameField;

    internal static List<string> names;
    internal static Dictionary<string, Character> characters;
    internal static Dictionary<string, Ability> abilities;
    internal static Dictionary<string, Weakness> weaknesses;
    internal static Dictionary<string, Perk> perks;
    internal static Dictionary<string, Flaw> flaws;

    private void Awake()
    {
        Instance = this;
        Screen.SetResolution(1000, 600, false);
        names = JsonReader.ReadNames(Application.streamingAssetsPath + "/names.txt");
        abilities = JsonReader.ReadAbilities(Application.streamingAssetsPath + "/Abilities.json");
        weaknesses = JsonReader.ReadWeaknesses(Application.streamingAssetsPath + "/Weaknesses.json");
        perks = JsonReader.ReadPerks(Application.streamingAssetsPath + "/Perks.json");
        flaws = JsonReader.ReadFlaws(Application.streamingAssetsPath + "/Flaws.json");
        characters = JsonReader.ReadCharacters();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void OnApplicationQuit()
    {
        List<Character> characterList = characters.Values.ToList();
        foreach(Character character in characterList)
        {
            Debug.Log("Saving Character: " + character.model.Name);
        }
        JsonWrite.SaveCharacters(characterList);
    }

    public void CreateCharacter()
    {
        GameObject characterGO = Instantiate(characterPrefab);
        Character character = characterGO.GetComponent<Character>();
        if (!string.IsNullOrEmpty(nameField.text)) character.model.Name = nameField.text;
        else character.model.Name = GenerateName();
        character.CreateCharacter();

        try
        {
            characters.Add(character.model.Name, character);
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

    public string GenerateName()
    {
        return names[Random.Range(0, names.Count)];
    }

    IEnumerator Wait(Character character)
    {
        yield return new WaitForSeconds(0.1f);
        PopulateContent(character);
    }

    public void PopulateContent(Character character)
    {
        Debug.Log("Populating " + character.model.Name);
        Clear();
        string text;
        foreach (Ability ability in character.model.Abilities.Values)
        {
            SpawnAbility(ability);
        }

        foreach (Weakness weakness in character.model.Weaknesses.Values)
        {
            SpawnWeakness(weakness);
        }

        int i = 0;
        foreach (Attack attack in character.model.Attacks)
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
        Clear();
        foreach (Character character in characters.Values)
        {
            SpawnCharacter(character.model.Name, character);
        }
    }

    public void ListAbilities()
    {
        Clear();
        foreach (Ability ability in abilities.Values)
        {
            SpawnAbility(ability);
        }
    }

    public void ListWeaknesses()
    {
        Clear();
        foreach (Weakness weakness in weaknesses.Values)
        {
            SpawnWeakness(weakness);
        }
    }

    public void ListPerks()
    {
        Clear();
        foreach (Perk perk in perks.Values)
        {
            SpawnPerk(perk);
        }
    }

    public void ListFlaws()
    {
        Clear();
        foreach (Flaw flaw in flaws.Values)
        {
            SpawnFlaw(flaw);
        }
    }

    public void SpawnCharacter(string value, Character character)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Attack";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = value;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { PopulateContent(character); });
    }

    public void SpawnAttack(string value, Attack attack)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Attack";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = value;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(attack); });
    }

    public void SpawnAbility(Ability ability)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Ability";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = ability.Name;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(ability); });
    }

    public void SpawnWeakness(Weakness weakness)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Weakness";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = weakness.Name;

        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(weakness); });
    }

    public void SpawnPerk(Perk perk)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Perk";
        textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t+ {perk.Name} ({perk.Modifier})";

        textGO.GetComponent<Button>().onClick.AddListener(delegate{ OnClick(perk); });
    }

    public void SpawnFlaw(Flaw flaw)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
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
