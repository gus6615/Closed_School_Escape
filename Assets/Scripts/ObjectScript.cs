using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour
{
    static private Image mouseCursor;
    static private new Camera camera;
    public Material material;
    public AudioClip[] audios;

    private Vector3 savedSize; // 오브젝트 원시크기

    // Start is called before the first frame update
    void Start()
    {
        Transform[] temps = FindObjectsOfType<Transform>();
        for (int i = 0; i < temps.Length; i++)
        {
            if (temps[i].gameObject.name == "MousePoint")
                mouseCursor = temps[i].GetComponentInChildren<Image>();
        }
        camera = FindObjectOfType<Camera>();

        savedSize = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Detected();

    }

    public void Detected()
    {
        RaycastHit hit;
        if (Physics.BoxCast(camera.transform.position, Vector3.one * 0.2f, camera.transform.forward, out hit, new Quaternion(0, 0, 0, 0), PlayerScript.detectedDis, 256))
        {
            if(hit.transform.parent.gameObject == this.gameObject || hit.transform.gameObject == this.gameObject)
            {
                this.transform.localScale = savedSize * 1.25f;
                material.SetFloat("_Metallic", 0.5f);

                mouseCursor.gameObject.SetActive(true);
            }
        }
        else if(Physics.BoxCast(camera.transform.position, Vector3.one * 0.2f, camera.transform.forward, out hit, new Quaternion(0, 0, 0, 0), PlayerScript.detectedDis, 512))
        {
            if (hit.transform.parent.gameObject == this.gameObject || hit.transform.gameObject == this.gameObject)
            {
                mouseCursor.gameObject.SetActive(true);
            }
        }
        else
        {
            this.transform.localScale = savedSize;
            material.SetFloat("_Metallic", 0f);
            mouseCursor.gameObject.SetActive(false);
        }
    }
}
