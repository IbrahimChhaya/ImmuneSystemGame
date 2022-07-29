using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    public float enemyDistanceRun = 4.0f;
    public Transform[] waypoints;

    private NavMeshAgent enemy;
    private GameObject player;
    private int destPoint = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        enemy.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //bool chasing = false;
        player = GameObject.FindGameObjectWithTag("Player");
        //distance to player from enemy
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance < enemyDistanceRun)
        {
            //chasing = true;
            doChase();
        }
        else
        {
            GoToNextPoint();
        }

        if (enemy.remainingDistance < 0.5f)
            GoToNextPoint();
    }

    void doChase()
    {
        Vector3 dirToPlayer = transform.position - player.transform.position;

        Vector3 newPos = transform.position - dirToPlayer;

        enemy.SetDestination(newPos);
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0)
            return;

        enemy.destination = waypoints[destPoint].position;
        destPoint = (destPoint + 1) % waypoints.Length;
    }
}
