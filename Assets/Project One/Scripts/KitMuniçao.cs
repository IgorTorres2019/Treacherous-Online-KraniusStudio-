using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitMuniçao : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<Collider>().GetComponentInChildren<Shooting>())
        {
            Shooting sh = other.GetComponentInChildren<Collider>().GetComponentInChildren<Shooting>();
            if (sh.Municao < 100)
            {
                sh.Municao += 30;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInChildren<Collider>().GetComponentInChildren<Shooting>())
        {
            Shooting sh = other.GetComponentInChildren<Collider>().GetComponentInChildren<Shooting>();
        }
    }
}
