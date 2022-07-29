using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolChase : MonoBehaviour
{
    public static PatrolChase instance;

    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1.0f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int currentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 playerPosition;

    public float _waitTime;
    float _timeToRotate;
    public bool _playerInRange;
    public bool _playerNear;
    public bool _isPatrol;
    public bool _caughtPlayer;
    public float tempDistance;
    public bool collided = false;

    private int probability = 20;
    private Vector3 originalPlayerPos;
    private Vector3[] originalEnemyPoses;

    public Material blue;
    public static bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = Vector3.zero;
        _isPatrol = true;
        _caughtPlayer = false;
        _playerInRange = false;
        _playerNear = false;
        _waitTime = startWaitTime;
        _timeToRotate = timeToRotate;

        currentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        originalPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        originalEnemyPoses = new Vector3[enemies.Length];
        int i = 0;
        foreach(GameObject e in enemies)
        {
            originalEnemyPoses[i] = e.transform.position;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnvironmentView();
        if(_isPatrol)
        {
            Patrolling();
        }
        else
        {
            Chasing();
        }

        if (end)
            CollisionHelper();
    }

    private void Chasing()
    {
        _playerNear = false;
        playerLastPosition = Vector3.zero;

        if (!_caughtPlayer)
        {
            if (Random.Range(1, 100) < probability)
            {
                _playerInRange = true;
                _playerNear = true;
                Move(speedRun);
                navMeshAgent.SetDestination(playerPosition);
            }
        }
        //if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            tempDistance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            //if (_waitTime <= 0 && !_caughtPlayer
            if (_waitTime <= 0 && !_caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 7f)
            {
                _isPatrol = true;
                _playerNear = false;
                Move(speedWalk);
                _timeToRotate = timeToRotate;
                _waitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 7f)
                {
                    Stop();
                }
                _waitTime -= Time.deltaTime;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        collided = true;

        /*GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = originalPlayerPos;
        cc.enabled = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        foreach (GameObject e in enemies)
        {
            e.transform.position = originalEnemyPoses[i];
            i++;
            e.GetComponent<Renderer>().material = blue;
        }
        currentWaypointIndex = 0;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);*/

        //CollisionHelper();

        ScoreManager.instance.AddDeath();
        probability += 10;
    }

    private void CollisionHelper()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = originalPlayerPos;
        cc.enabled = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        foreach (GameObject e in enemies)
        {
            e.transform.position = originalEnemyPoses[i];
            i++;
            e.GetComponent<Renderer>().material = blue;
        }
        currentWaypointIndex = 0;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        if (end)
            ScoreManager.instance.AddWin();
        end = false;
    }

    private void Patrolling()
    {
        if(_playerNear)
        {
            if(_timeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerPosition);
            }
            else
            {
                Stop();
                _timeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            //_playerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if(_waitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    _waitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    _waitTime -= Time.deltaTime;
                }
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
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        _caughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(_waitTime <= 0)
            {
                _playerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
                _waitTime = startWaitTime;
                _timeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                _waitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    _playerInRange = true;
                    _isPatrol = false;
                }
                else
                {
                    _playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                _playerInRange = false;
            }
            if (_playerInRange)
            {
                playerPosition = player.transform.position;
            }
        }
    }
}
