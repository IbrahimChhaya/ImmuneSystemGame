using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyesScript : MonoBehaviour
{

    public GameObject leftEye;
    public GameObject rightEye;
    public GameObject leftEyebrow;
    public GameObject rightEyebrow;

    public Material red;
    public Material white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        bool temp = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies)
        {
            if(e.GetComponent<EnemyController>().canSeePlayer == true)
            {
                temp = true;
            }
        }

        if(temp)
        {
            PlayerController.isBeingChased = true;
            //change material of left and right eyes to red
            leftEye.GetComponent<Renderer>().material = red;
            rightEye.GetComponent<Renderer>().material = red;
            leftEyebrow.SetActive(true);
            rightEyebrow.SetActive(true);
        }
        else
        {
            PlayerController.isBeingChased = false;
            //change material of left and right eyes to default
            leftEye.GetComponent<Renderer>().material = white;
            rightEye.GetComponent<Renderer>().material = white;
            leftEyebrow.SetActive(false);
            rightEyebrow.SetActive(false);
        }
    }
}
