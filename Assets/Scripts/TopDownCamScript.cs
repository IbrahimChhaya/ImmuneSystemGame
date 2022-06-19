using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamScript : MonoBehaviour
{
    private Transform target;
    public float distance = 5;
    public float angle = 43;
    public float height = 9.5f;
    public float smoothness = 0.6f;
    private Vector3 referenceVelocity;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        topDownCam();
    }

    private void topDownCam()
    {
        if (!target)
            return;

        Vector3 worldPos = (Vector3.forward * -distance) + (Vector3.up * height);
        Vector3 angleVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPos;
        Vector3 flatPos = target.position;
        flatPos.y = 0;
        Vector3 finalPos = flatPos + angleVector;

        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref referenceVelocity, smoothness);
        transform.LookAt(flatPos);
    }

    // Update is called once per frame
    void Update()
    {
        topDownCam();
    }
}
