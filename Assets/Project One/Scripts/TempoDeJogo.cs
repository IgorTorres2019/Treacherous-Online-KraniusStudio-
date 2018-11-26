using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoDeJogo : MonoBehaviour
{
    float Tempo;
    
    private void Update()
    {
        Tempo += Time.deltaTime;
    }
}
