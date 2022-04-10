using UnityEngine;
using UnityEngine.UI;

public class HistoryPanel : MonoBehaviour
{
    public static HistoryPanel Instance;
    public Transform contentPanel;
    public GameObject textPrefab;
    public bool isShowing = true;
    public Image toggleImage;
    public Sprite isShowingSprite;
    public Sprite isHiddenSprite;

    private void Awake()
    {
        Instance = this;
    }

    public void AddText(string text, Color color)
    {
        GameObject textGO = Instantiate(textPrefab, contentPanel);
        TMPro.TextMeshProUGUI t = textGO.GetComponent<TMPro.TextMeshProUGUI>();
        t.text = text;// + " : " + System.DateTime.Now.ToShortTimeString();
        t.color = color;
    }

    public void TogglePanel()
    {
        if (!isShowing)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(225, 500);
            toggleImage.sprite = isShowingSprite;
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(55, 500);
            toggleImage.sprite = isHiddenSprite;
        }
        isShowing = !isShowing;
    }

    public void ClearPanel()
    {
        for(int i = contentPanel.childCount; i > 0; i--)
        {
            DestroyImmediate(contentPanel.GetChild(i - 1).gameObject);
        }
    }
}
