using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitMedico : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().GetComponent<PlayerHealth>())
        {
            PlayerHealth ph = other.GetComponent<Collider>().GetComponent<PlayerHealth>();
            if (ph.CurrentHealth < 100)
            {
                ph.CurrentHealth += 30;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().GetComponent<PlayerHealth>())
        {
            PlayerHealth ph = other.GetComponent<Collider>().GetComponent<PlayerHealth>();
        }
    }
}
