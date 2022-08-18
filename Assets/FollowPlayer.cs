using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{

    public Transform target;
    NavMeshAgent nav;
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        originalPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position);
    }

    public void CollisionHelper()
    {
        transform.position = originalPos;
    }
}
