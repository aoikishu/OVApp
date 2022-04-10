using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPanel : MonoBehaviour
{
    public Transform content;
    public GameObject listItemPrefab;
    private List<GameObject> list;

    private void Awake()
    {
        list = new List<GameObject>();
    }

    public void Show(bool show)
    {
        content.gameObject.SetActive(show);
    }

    public void PopulateList<T>(Dictionary<string, T>.ValueCollection values)
    {
        DeleteList();
        
    }

    public void DeleteList()
    {
        foreach (GameObject go in list)
        {
            Destroy(go);
            list.Clear();
        }
    }
}
