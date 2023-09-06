using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemText : MonoBehaviour
{
    static private Text text;
    static private Color color;
    static private string info;
    static public bool isBackOn;
    static private float backTime;
    static private float currentBackTime;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        color = Color.white;
        backTime = 3f;
        info = "";
        text.text = info;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBackOn)
        {
            currentBackTime += Time.deltaTime;
            
            if(currentBackTime >= backTime / 2f)
            {
                color = new Color(1, 1, 1, color.a - Time.deltaTime * 2f / backTime);
                text.color = color;
            }

            if(currentBackTime >= backTime)
            {
                isBackOn = false;
                currentBackTime = 0f;
                color = Color.white;
                info = "";
                text.text = info;
                text.color = color;
            }
        }
    }

    static public void SetInfo(string data)
    {
        info = data;
        text.text = info;
        currentBackTime = 0f;
        color = Color.white;
        text.color = color;
        isBackOn = true;
    }
}
