using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollAtivo : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 240f);
    }
}
