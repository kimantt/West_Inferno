using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Off : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Invoke("TurnOff", 0.1f);
    }

    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
