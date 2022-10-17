using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get cutout location using WorldToViewportPoint which converts world space to screen space
        //Returns position of object in screen space
        //Format can be compared with screen space UVs
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        //aspect ratio
        cutoutPos.y /= (Screen.width / Screen.height);

        //ray cast from camera and target
        Vector3 offset = targetObject.position - transform.position;
        //array of objects hit by raycast
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        //iterate through list of raycast hits
        for(int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for(int j = 0; j < materials.Length; j++)
            {
                materials[j].SetVector("_CutoutPos", cutoutPos);
                materials[j].SetFloat("_CutoutSize", 0.1f);
                materials[j].SetFloat("_Falloff", 0.05f);
            }
        }
    }
}
