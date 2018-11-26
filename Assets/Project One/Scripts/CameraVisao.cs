using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVisao : MonoBehaviour
{
    public static CameraVisao instance;

    public GameObject[] Cabeça;
    public GameObject[] pos;
    public Camera[] cam;

    bool Cmenabled = false;

    [SerializeField]
    private int ID = 0;

    float V = -2.0f, H = 2.0f;

    private RaycastHit hit;

    private void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        Segue();
        Rotacao();
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ID < 1)
            {
                ID++;
            }
            else if (ID > 0)
            {
                ID = 0;
            }
        }
    }
    void Segue()
    {
      
       transform.LookAt(Cabeça[ID].transform);

       if (!Physics.Linecast(Cabeça[ID].transform.position, pos[ID].transform.position))
       {
            transform.position = pos[ID].transform.position;
            transform.SetParent(pos[ID].transform);
              
       }
       else if (Physics.Linecast(Cabeça[ID].transform.position, pos[ID].transform.position, out hit))
       {
            transform.position = hit.point;
       }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!Cmenabled)
            {
                cam[0].enabled = false;
                pos[2].gameObject.SetActive(false);
                cam[1].enabled = true;
                Cmenabled = true;
            }
            else
            {
                pos[2].gameObject.SetActive(true);
                cam[1].enabled = false;
                cam[0].enabled = true;
                Cmenabled = false;
            }
            
        }
    }

    private void Rotacao()
    {
        float Hor = H * Input.GetAxis("Mouse X");
        float Ver = V * Input.GetAxis("Mouse Y");

        Cabeça[ID].transform.Rotate(Ver, 0, 0);
    }
}
