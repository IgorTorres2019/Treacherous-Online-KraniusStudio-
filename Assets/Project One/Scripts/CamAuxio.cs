using UnityEngine;

public class CamAuxio : MonoBehaviour
{
    public static CamAuxio instance;

    public GameObject visaoT,CabecaM;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        Procura();   
    }

    void Procura()
    {
        transform.position = visaoT.transform.position;
        transform.SetParent(visaoT.transform, false);
        transform.LookAt(CabecaM.transform);
    }
}
