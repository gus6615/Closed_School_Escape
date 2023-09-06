using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Canvas canvas;
    static public new Camera camera;
    private new Rigidbody rigidbody;
    private Light flashLight;
    static public GameObject mouseImage;
    private GameObject detectedObject;
    private RaycastHit objectHit;

    static public bool isControlOn; // 컨트롤이 가능한가?
    static public bool isMove; // 현재 움직이는 중인가?
    static public float moveSpeed; // 이동속도
    static public float detectedDis;
    private float maxMouseAngle; // 마우스Y의 최대 각도
    static public float mouseSensitivity; // 마우스 감도
    private float currentCameraRotationX, currentCameraRotationY; 

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        camera = GetComponentInChildren<Camera>();
        rigidbody = GetComponent<Rigidbody>();
        flashLight = GetComponentInChildren<Light>();

        canvas.gameObject.SetActive(true);
        Transform[] temp = FindObjectsOfType<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gameObject.name == "MousePoint")
                mouseImage = temp[i].gameObject;
        }

        isControlOn = true;
        moveSpeed = 3f;
        maxMouseAngle = 85f;
        mouseSensitivity = 5f;
        detectedDis = 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isControlOn)
        {
            MoveCtrl();
            MouseCtrl();
            SitDown();
        }
    }

    public void MoveCtrl()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        transform.position += (transform.right * moveX + transform.forward * moveZ) * moveSpeed * Time.deltaTime;

        if((moveX != 0f || moveZ != 0f) && !isMove)
        {
            GetComponent<AudioSource>().volume = 1f;
            isMove = true;
        }
        else if ((moveX == 0f && moveZ == 0f) && isMove)
        {
            GetComponent<AudioSource>().volume = 0f;
            isMove = false;
        }
    }

    public void MouseCtrl()
    {
        // 마우스 움직임에 대한 화면 이동
        float mouseRotationX = Input.GetAxisRaw("Mouse Y");
        currentCameraRotationX -= mouseRotationX * mouseSensitivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -maxMouseAngle, maxMouseAngle);

        float mouseRotationY = Input.GetAxisRaw("Mouse X");
        currentCameraRotationY += mouseRotationY * mouseSensitivity;

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        this.transform.localEulerAngles = new Vector3(0, currentCameraRotationY, 0);
        flashLight.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);

        // 물건 집기 기능
        GrabObject();

        // 마우스 클릭시 일정 오브젝트 감지
        Interact();
    }

    public void SitDown()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            flashLight.transform.localPosition = camera.transform.localPosition = new Vector3(0f, 0.25f, 0f);
            moveSpeed = 1.5f;
        }
        else
        {
            flashLight.transform.localPosition = camera.transform.localPosition= new Vector3(0f, 1f, 0f);
            moveSpeed = 3f;
        }
    }

    public void GrabObject()
    {
        if (Input.GetMouseButton(0))
        {
            if (detectedObject == null && Physics.BoxCast(camera.transform.position, Vector3.one * 0.2f, camera.transform.forward, out objectHit, new Quaternion(0, 0, 0, 0), detectedDis, 1024))
            {
                maxMouseAngle = 45f;
                detectedObject = objectHit.collider.GetComponentInParent<Rigidbody>().gameObject;
            }
            else if(detectedObject != null)
            {
                detectedObject.GetComponent<Rigidbody>().useGravity = false;
                detectedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                detectedObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                float distance = detectedDis - detectedObject.GetComponentInParent<ObjectInfo>().DetectedDis;
                while (Physics.BoxCast(camera.transform.position, Vector3.one * 0.2f, camera.transform.forward, new Quaternion(0, 0, 0, 0), distance, 2560))
                {
                    distance -= detectedObject.GetComponentInParent<ObjectInfo>().DetectedDis;
                }

                detectedObject.transform.position = camera.transform.position + camera.transform.forward * (distance - 0.1f);
            }
        }
        else if (detectedObject != null)
        {
            maxMouseAngle = 85f;
            detectedObject.GetComponent<Rigidbody>().useGravity = true;
            detectedObject = null;
        }
    }

    public void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit objectHit;

            if (Physics.BoxCast(camera.transform.position, Vector3.one * 0.2f, camera.transform.forward, out objectHit, new Quaternion(0, 0, 0, 0), detectedDis, 768))
            {
                // 해당 오브젝트가 문인가?
                Door doorScript = objectHit.collider.GetComponentInParent<Door>();
                if (doorScript != null)
                {
                    if (doorScript.isCanOpen)
                    {
                        doorScript.isOpen = true;
                    }
                    else
                    {
                        if (SaveScript.items[doorScript.KeyNum].isHas)
                        {
                            FindObjectOfType<Inventory>().SetSound(0);
                            SaveScript.items[doorScript.KeyNum].isHas = false;
                            SystemText.SetInfo("'" + SaveScript.items[doorScript.KeyNum].name + "' 을(를) 사용했다.");
                            doorScript.isCanOpen = true;
                        }
                        else
                        {
                            objectHit.collider.GetComponentInParent<AudioSource>().clip = objectHit.collider.GetComponentInParent<ObjectScript>().audios[0];
                            objectHit.collider.GetComponentInParent<AudioSource>().Play();
                            SystemText.SetInfo("닫혀있다.");
                        }
                    }
                }

                // 해당 오브젝트가 아이템인가?
                ItemCode itemCode = objectHit.collider.GetComponentInParent<ItemCode>();
                if (itemCode != null)
                {
                    FindObjectOfType<Inventory>().SetSound(0);
                    SaveScript.items[itemCode.itemCode].isHas = true;
                    Destroy(objectHit.collider.gameObject);
                    SystemText.SetInfo("'" + SaveScript.items[itemCode.itemCode].name + "' 을(를) 휙득");
                }
            }
        }
    }
}
