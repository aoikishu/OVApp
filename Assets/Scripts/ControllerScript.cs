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
    internal static Dictionary<string, Attack> attacks;
    internal static Dictionary<string, Ability> abilities;
    internal static Dictionary<string, Weakness> weaknesses;
    internal static Dictionary<string, Perk> perks;
    internal static Dictionary<string, Flaw> flaws;
    internal static Character currentCharacter;
    internal static string CONST_PATH;

    private void Awake()
    {
        Instance = this;
        CONST_PATH = Application.persistentDataPath;
        names = JsonReader.ReadNames(CONST_PATH + "/Names.txt");
        abilities = JsonReader.ReadAbilities(CONST_PATH + "/Abilities.json");
        weaknesses = JsonReader.ReadWeaknesses(CONST_PATH + "/Weaknesses.json");
        perks = JsonReader.ReadPerks(CONST_PATH + "/Perks.json");
        flaws = JsonReader.ReadFlaws(CONST_PATH + "/Flaws.json");
        characters = JsonReader.ReadCharacters();
        attacks = JsonReader.ReadAttacks();
        Screen.SetResolution(1024, 1024, false);
    }

    private void Start()
    {
        ListCharacters();
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

        List<Attack> attackList = attacks.Values.ToList();
        foreach (Attack attack in attackList)
        {
            Debug.Log("Saving Attack: " + attack.Name);
        }
        JsonWrite.SaveAttacks(attackList);
    }

    public void CreateCharacter()
    {
        GameObject characterGO = Instantiate(characterPrefab);
        Character character = characterGO.GetComponent<Character>();
        if (!string.IsNullOrEmpty(nameField.text) && !characters.ContainsKey(nameField.text)) character.model.Name = nameField.text;
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
        nameField.text = character.model.Name;
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
        foreach (string attackID in character.model.Attacks)
        {
            i++;
            Attack attack = ControllerScript.attacks[attackID];
            text = $"{attack.Name}\t({attack.EnduranceCost})";
            SpawnAttack(text, attack);
            foreach (Perk perk in attack.Perks.Values)
            {
                perk.parentAttack = attack;
                SpawnPerk(perk);
            }
            foreach (Flaw flaw in attack.Flaws.Values)
            {
                flaw.parentAttack = attack;
                SpawnFlaw(flaw);
            }
        }

        currentCharacter = character;
    }

    public void ListAttacks()
    {

    }

    public void ListCharacters()
    {
        Clear();
        foreach (Character character in characters.Values)
        {
            SpawnCharacter(character.model.Name, character);
        }
        navPanel.SetActive(false);
    }

    public void ListAbilities()
    {
        Clear();
        foreach (Ability ability in abilities.Values)
        {
            SpawnAbility(ability);
        }
        navPanel.SetActive(false);
    }

    public void ListWeaknesses()
    {
        Clear();
        foreach (Weakness weakness in weaknesses.Values)
        {
            SpawnWeakness(weakness);
        }
        navPanel.SetActive(false);
    }

    public void ListPerks()
    {
        Clear();
        foreach (Perk perk in perks.Values)
        {
            SpawnPerk(perk);
        }
        navPanel.SetActive(false);
    }

    public void ListFlaws()
    {
        Clear();
        foreach (Flaw flaw in flaws.Values)
        {
            SpawnFlaw(flaw);
        }
        navPanel.SetActive(false);
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
        textGO.name = ability.Name;
        TextMeshProUGUI txt = textGO.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = ability.Name + " : " + ability.Level;
        if (!string.IsNullOrEmpty(ability.Note)) txt.text += "\t\t(" + ability.Note + ")";
        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(ability); });
    }

    public void SpawnWeakness(Weakness weakness)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = weakness.Name;
        TextMeshProUGUI txt = textGO.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = weakness.Name + " : " + weakness.Level;
        if (!string.IsNullOrEmpty(weakness.Note)) txt.text += "\t\t(" + weakness.Note + ")";
        textGO.GetComponent<Button>().onClick.AddListener(delegate { OnClick(weakness); });
    }

    public void SpawnPerk(Perk perk)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Perk";
        if (perk.Level > 1) textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t+ {perk.Name} ({perk.Modifier}) x{perk.Level}";
        else textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t+ {perk.Name} ({perk.Modifier})";

        textGO.GetComponent<Button>().onClick.AddListener(delegate{ OnClick(perk); });
    }

    public void SpawnFlaw(Flaw flaw)
    {
        GameObject textGO = Instantiate(listItemPrefab, contentPanel);
        textGO.name = "Flaw";
        if (flaw.Level > 1) textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t- {flaw.Name} ({flaw.Modifier}) x{flaw.Level}";
        else textGO.GetComponentInChildren<TextMeshProUGUI>().text = $"\t+ {flaw.Name} ({flaw.Modifier})";

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
