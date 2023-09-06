using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageObejct : MonoBehaviour
{
    static private ImageObejct instance;

    public GameObject itemObject;
    static public SpriteRenderer[] itemImageObject;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            itemImageObject = itemObject.GetComponentsInChildren<SpriteRenderer>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
