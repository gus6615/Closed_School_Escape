using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    static private GameObject itemInfoObject;
    static private Image itemImage;
    static private Text itemText;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] temp = FindObjectsOfType<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gameObject.name == "Infos")
                itemInfoObject = temp[i].gameObject;
            else if (temp[i].gameObject.name == "ItemInfoImage")
                itemImage = temp[i].gameObject.GetComponent<Image>();
            else if (temp[i].gameObject.name == "ItemInfoText")
                itemText = temp[i].gameObject.GetComponent<Text>();
        }
    }

    public void ItemeButtonOn()
    {
        itemInfoObject.SetActive(true);
    }
}
