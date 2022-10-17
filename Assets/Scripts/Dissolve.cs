using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private int _id;
    public string shaderVariableName = "_DistanceToCamera";
    private Vector3 _distanceObjectToCamera;
    
    // Start is called before the first frame update
    public void Start()
    {
        _id = Shader.PropertyToID(shaderVariableName);
    }

    // Update is called once per frame
    public void Update()
    {
        _distanceObjectToCamera = Camera.main.transform.position - this.transform.position;
        Shader.SetGlobalFloat(_id, _distanceObjectToCamera.magnitude);
    }
}
