using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 savedPos;
    private Vector3 savedAngle;

    static public float doorDis; // 문의 넓이
    static public float doorAngle; // 문의 각도
    static public float moveSpeed; // 문의 속도

    public bool isCanOpen; // 열 수 있는가?
    public bool isSound; // 사운드가 실행되었는가?
    public bool isOpen; // 여는 중인가?
    public bool isClosed; // 현재 닫혀있는가?
    public int KeyNum; // 해당 문의 열쇠 번호 (열쇠 번호가 -1은 열쇠 없이도 열 수 있다.)
    public int type; // 문의 여닫이인가? (0 은 여닫이, 1은 문고리가 달린 회전형이다.)
    public float currentMoveDis; // 최근 이동한 문의 거리 혹은 문의 회전 각도

    // Start is called before the first frame update
    void Start()
    {
        savedPos = this.transform.position;
        savedAngle = this.transform.localEulerAngles;

        doorDis = 4f;
        doorAngle = 95f;
        moveSpeed = 4f;

        isClosed = true;

        if (KeyNum == -1)
            isCanOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            if (!isSound)
            {
                GetComponent<AudioSource>().clip = GetComponent<ObjectScript>().audios[1];
                GetComponent<AudioSource>().Play();
                isSound = true;
            }
            MoveDoor(type);
        }
    }

    public void MoveDoor(int type)
    {
        if(type == 0)
        {
            if (isClosed)
            {
                if (currentMoveDis <= doorDis)
                {
                    currentMoveDis += Time.deltaTime * moveSpeed;
                    this.transform.position -= this.transform.up * Time.deltaTime * moveSpeed;
                }
                else
                {
                    this.transform.position = savedPos - this.transform.up * doorDis;
                    savedPos = this.transform.position;
                    isOpen = false;
                    currentMoveDis = 0f;
                    isClosed = !isClosed;
                    isSound = false;
                }
            }
            else
            {
                if (currentMoveDis <= doorDis)
                {
                    currentMoveDis += Time.deltaTime * moveSpeed;
                    this.transform.position += this.transform.up * Time.deltaTime * moveSpeed;
                }
                else
                {
                    this.transform.position = savedPos + this.transform.up * doorDis;
                    savedPos = this.transform.position;
                    isOpen = false;
                    currentMoveDis = 0f;
                    isClosed = !isClosed;
                    isSound = false;
                }
            }
        }
        else if(type == 1)
        {
            moveSpeed = 95f;
            if (isClosed)
            {
                if (currentMoveDis <= doorAngle)
                {
                    currentMoveDis += Time.deltaTime * moveSpeed;
                    this.transform.localEulerAngles -= Vector3.up * Time.deltaTime * moveSpeed;
                }
                else
                {
                    this.transform.localEulerAngles = savedAngle - Vector3.up * doorAngle;
                    savedAngle = this.transform.localEulerAngles;
                    isOpen = false;
                    currentMoveDis = 0f;
                    isClosed = !isClosed;
                    isSound = false;
                }
            }
            else
            {
                if (currentMoveDis <= doorAngle)
                {
                    currentMoveDis += Time.deltaTime * moveSpeed;
                    this.transform.localEulerAngles += Vector3.up * Time.deltaTime * moveSpeed;
                }
                else
                {
                    this.transform.localEulerAngles = savedAngle + Vector3.up * doorAngle;
                    savedAngle = this.transform.localEulerAngles;
                    isOpen = false;
                    currentMoveDis = 0f;
                    isClosed = !isClosed;
                    isSound = false;
                }
            }
        }
    }
}
