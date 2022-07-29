using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Character
{
    //radius around enemy
    public float fovRadius;
    //angle of enemy sight
    [Range(0, 360)]
    public float fovAngle;
    //player gameobject
    private GameObject player;
    //layermask to look for
    public LayerMask targetMask;
    //layermask can obstructs fov
    public LayerMask obstructionMask;
    //material to change enemy colour upon player sight
    public Material red;
    public bool canSeePlayer;

    public NavMeshAgent navMeshAgent;
    //waypoints for patrolling
    public Transform[] waypoints;
    //current waypoint
    private int currentWaypoint;
    private float startWaitTime = 4;
    public float _waitTime;
    public bool isPatrol;
    public float speedWalk = 3;
    public float speedRun = 4;
    public bool _caughtPlayer;
    private Vector3 playerPosition;
    public float currentSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FoVRoutine());

        isPatrol = true;
        _caughtPlayer = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);

        _waitTime = startWaitTime;
    }

    private IEnumerator FoVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        
        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        //check for colliders
        //from the centre of the enemy with the radius
        //look for object with the specific layermask (the player)
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, targetMask);

        //if there exists a collider with player layermask
        if(rangeChecks.Length != 0)
        {
            //there is only the player there so just get the first one
            Transform target = rangeChecks[0].transform;
            //where the enemy is looking to where the player is
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                //is the player close enough 
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //a raycast from the centre of enemy aimed at the player, limited by distance and obstructions
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    isPatrol = false;
                    playerPosition = player.transform.position;
                }
            }
            if (Vector3.Distance(transform.position, target.position) > fovRadius)
                canSeePlayer = false;
            //fix this can see player thing
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check fov
        FieldOfViewCheck();
        if (isPatrol)
            //run patrol
            Patrolling();
        else
            //else chase
            Chasing();
        currentSpeed = navMeshAgent.speed;
    }

    private void Chasing()
    {
        //if the player hasn't been caught yet
        if (!_caughtPlayer)
        {
            //run to the player's last known position
            Move(speedRun);
            navMeshAgent.SetDestination(playerPosition);
        }

        //if the destination has been reached
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            //haven't caught player, can't see player, and wait time is over, back to patrol
            if (_waitTime <= 0 && !_caughtPlayer && !canSeePlayer)
            {
                isPatrol = true;
                Move(speedWalk);
                _waitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
            }
            else
            {
                //can't see the player so I'll stop and wait for a bit
                if (!canSeePlayer)
                {
                    Stop();
                    _waitTime -= Time.deltaTime;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        _caughtPlayer = true;
        ScoreManager.instance.AddDeath();
    }

    private void Patrolling()
    {
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        //if reached desitination
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            //waiting done
            if(_waitTime <= 0)
            {
                //go to next point
                NextPoint();
                //move at walking speed
                Move(speedWalk);
                //reset waitTime
                _waitTime = startWaitTime;
            }
            //wait here for a bit
            else
            {
                //stop and dec the waitTime counter
                Stop();
                _waitTime -= Time.deltaTime;
            }
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
    }
}
