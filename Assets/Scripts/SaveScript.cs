using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    static private SaveScript instance;
    static public int itemNum; // 총 아이템 수
    static public int eventNum; // 총 이벤트 수

    static public Item[] items; // 모든 아이템의 집합
    static public bool[] eventBools; // 각 이벤트를 모두 진행한 적이 있는가?

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        itemNum = 10;
        eventNum = 10;

        items = new Item[itemNum];
        items[0] = new Item(0);
        eventBools = new bool[eventNum];
    }
}
