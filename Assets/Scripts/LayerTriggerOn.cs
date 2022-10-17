using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTriggerOn : MonoBehaviour
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
            gO.layer = LayerMask.NameToLayer("TransparentWalls");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
