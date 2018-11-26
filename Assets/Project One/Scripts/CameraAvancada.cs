using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAvancada : MonoBehaviour
{
    public Vector3 cameraMoveVel = Vector3.zero;
    public GameObject segueOBJ;
    public float limiteAng = 65.0f;
    public float inputSensit = 155.0f;
    public float MoveX, MoveY;
    public float rotY = 0.0f, rotX = 0.0f;
    public Vector3 rot;
    private Quaternion LocalRot;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Atualizacao();
    }
    void Init()
    {
        rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Atualizacao()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveY = Input.GetAxis("Vertical");

        rotY += MoveX * inputSensit * Time.deltaTime;
        rotX += MoveY * inputSensit * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -limiteAng, limiteAng);
        LocalRot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = LocalRot;
    }
    private void LateUpdate()
    {
        
    }
}
