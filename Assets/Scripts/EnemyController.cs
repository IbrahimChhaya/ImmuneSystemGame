using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Material blue;
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
    
    public TextMesh tm;
    GameObject sign;

    public int probability;

    private Vector3 originalPlayerPos;
    private Vector3[] originalEnemyPoses;
    private Vector3 originalEnemyPos;
    public bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalPlayerPos = player.transform.position;

        StartCoroutine(FoVRoutine());

        isPatrol = true;
        _caughtPlayer = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);

        _waitTime = startWaitTime;

        sign = new GameObject("enemyAffinity");
        sign.transform.rotation = Camera.main.transform.rotation;

        tm = sign.AddComponent<TextMesh>();
        GenerateSignature();
        //CalculateAffinity();
        CalculateLevenshtein();
        tm.text = "Affinity: " + Affinity;
        tm.color = new Color(0.8f, 0.8f, 0.8f);
        tm.fontStyle = FontStyle.Bold;
        tm.alignment = TextAlignment.Center;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.characterSize = 0.065f;
        tm.fontSize = 60;

        probability = UnityEngine.Random.Range(1, 100);

        originalEnemyPos = transform.position;
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

                if (probability <= Affinity)
                { 
                    //a raycast from the centre of enemy aimed at the player, limited by distance and obstructions
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        canSeePlayer = true;
                        isPatrol = false;
                        playerPosition = player.transform.position;
                    }
                }
            }
            if (Vector3.Distance(transform.position, target.position) > fovRadius)
                canSeePlayer = false;
            //fix this can see player thing
        }
    }

    // Update is called once per frame
    private void Update()
    {
        sign.transform.position = transform.position + Vector3.up * 3f;
        /*if (EndGame.end)
        {
            CollisionHelper();
        }*/
    }


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
            //if (probability <= Affinity)
            {
                //run to the player's last known position
                Move(speedRun);
                navMeshAgent.SetDestination(playerPosition);
                gameObject.GetComponent<Renderer>().material = red;
            }
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
        Affinity += 10;
        end = true;
        CollisionHelper();
    }

    public void CollisionHelper()
    {
        isPatrol = true;
        canSeePlayer = false;
        currentWaypoint = 0;
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        transform.position = originalEnemyPos;
        GetComponent<Renderer>().material = blue;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        _waitTime = startWaitTime;

        if (end)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = PlayerController.originalPlayerPos;
            cc.enabled = true;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //int i = 0;
            foreach (GameObject e in enemies)
            {
                if(e != gameObject)
                {
                    if (UnityEngine.Random.Range(1, 100) <= probability)
                        e.GetComponent<EnemyController>().Affinity = Affinity;
                    e.GetComponent<EnemyController>().CollisionHelper();
                }
            }
        }
        end = false;

        _caughtPlayer = false;
        tm.text = "Affinity: " + Affinity;
    }

    private void Patrolling()
    {
        gameObject.GetComponent<Renderer>().material = blue;

        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        Move(speedWalk);

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

    public void CalculateAffinity()
    {
        var playerSignature = PlayerController.tempSignature;

        Affinity = playerSignature.Zip(Signature, (ps, s) => ps != s).Count(f => f);
        Affinity = 100 - ((Affinity/12)*100);
    }

    public void CalculateLevenshtein()
    {
        var string1 = PlayerController.tempSignature;
        var string2 = Signature;

        //2D array
        var matrix = new int[string1.Length + 1, string2.Length + 1];

        //initialise the matrix with row size string1.length and column size string2.length
        for (var i = 0; i <= string1.Length; matrix[i, 0] = i++)
        {

        }
        for (var j = 0; j <= string2.Length; matrix[0, j] = j++)
        {

        }

        for (var i = 1; i <= string1.Length; i++)
        {
            for ( var j = 1; j <= string2.Length; j++)
            {
                var cost = (string2[j - 1] == string1[i - 1]) ? 0 : 1;

                matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
            }
        }
        Affinity = matrix[string1.Length, string2.Length];
        Affinity = 100 - ((Affinity / 12) * 100);
    }
}

/* ok so you managed to fix endgame by reaching the end room and by collision
 * and you managed to fix chasing
 * problem rn is the bot still moves after being reset, has to be a property of this navmesh agent
 * next up is probabilistic detection and attack
 * do a more complicated string distance metric
 * then clonal expansion of those that did detection and attack
 */