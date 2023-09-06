using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    static public string[] names;
    static public string[] infos;
    static private bool isInit;

    public int itemCode;
    public bool isHas; // 현재 플레이어가 가지고 있는가?
    public string name;
    public string info;

    public Item(int _itemcode)
    {
        if (!isInit)
            Init();

        itemCode = _itemcode;
        name = names[itemCode];
        info = infos[itemCode];
    }

    public void Init()
    {
        names = new string[SaveScript.itemNum];
        names[0] = "회색 열쇠";

        infos = new string[SaveScript.itemNum];
        infos[0] = "회색을 띄고 있는 열쇠다. 2학년 1반이라고 쓰여 있다.";
    }
}
