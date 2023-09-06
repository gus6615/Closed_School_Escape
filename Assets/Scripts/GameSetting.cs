using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine("MyCursor");
    }

    public void Update()
    {

    }

    IEnumerator MyCursor()
    {
        yield return new WaitForEndOfFrame();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
