using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject InvenPanel;
    [SerializeField] private GameObject itemPrefab;
    static private GameObject invenObject;
    static private GameObject invenInfo;
    public AudioClip[] audios;

    static public bool isInventoryOn; // 인벤토리가 열려있는가?

    private void Start()
    {
        Transform[] temps = FindObjectsOfType<Transform>();
        for (int i = 0; i < temps.Length; i++)
        {
            if (temps[i].gameObject.name == "InvenObject")
                invenObject = temps[i].gameObject;
            else if (temps[i].gameObject.name == "Infos")
                invenInfo = temps[i].gameObject;
        }
        invenObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            InventoryOn();
    }

    public void InventoryOn()
    {
        if (!isInventoryOn)
        {
            PlayerScript.isControlOn = false;
            invenObject.SetActive(true);
            invenInfo.SetActive(false);
            PlayerScript.mouseImage.SetActive(false);
            FindObjectOfType<PlayerScript>().GetComponent<AudioSource>().Stop();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            for (int i = 0; i < SaveScript.itemNum; i++)
            {
                GameObject temp = Instantiate(itemPrefab, InvenPanel.transform);

                if (SaveScript.items[i] != null && SaveScript.items[i].isHas)
                {
                    temp.GetComponentsInChildren<Image>()[2].sprite = ImageObejct.itemImageObject[i].sprite;
                }
                else
                    temp.GetComponentsInChildren<Image>()[2].sprite = null;
            }
        }
        else
        {
            ItemButton[] items = InvenPanel.GetComponentsInChildren<ItemButton>();
            for (int i = 0; i < items.Length; i++)
                Destroy(items[i].gameObject);

            Time.timeScale = 1f;
            PlayerScript.isControlOn = true;
            invenObject.SetActive(false);
            PlayerScript.mouseImage.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            FindObjectOfType<PlayerScript>().GetComponent<AudioSource>().Play();
        }

        isInventoryOn = !isInventoryOn;
    }

    public void SetSound(int index)
    {
        this.GetComponent<AudioSource>().clip = audios[index];
        this.GetComponent<AudioSource>().Play();
    }
}
