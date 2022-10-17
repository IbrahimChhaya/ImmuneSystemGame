using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTriggerOff : MonoBehaviour
{
    public GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(GameObject gO in objects)
        {
            gO.layer = LayerMask.NameToLayer("Default");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
