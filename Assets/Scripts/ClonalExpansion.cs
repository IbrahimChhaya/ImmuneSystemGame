using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonalExpansion : MonoBehaviour
{
    private GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
            
    }


}
