using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

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
    public Material eyeRed;
    public Material eyeBlue;
    public GameObject eye;

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

    public Image healthBarSprite;
    
    private float diff = 20f;
    public GameObject mainCam;
    public GameObject germinalCenter;
    public GameObject germinalPos2;
    public GameObject clone;

    public Material black;

    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalPlayerPos = player.transform.position;
        
        StartCoroutine(FoVRoutine());
        StartCoroutine(lifelineCount());

        isPatrol = true;
        _caughtPlayer = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);

        _waitTime = startWaitTime;

        sign = new GameObject("enemyAffinity" + gameObject.name);
        sign.transform.rotation = Camera.main.transform.rotation;

        tm = sign.AddComponent<TextMesh>();
        GenerateSignature();
        string activeDistance = PlayerPrefs.GetString("activeDistance");
        if (activeDistance == "J")
            CalculateJaro();
        else if (activeDistance == "L")
            CalculateLevenshtein();
        else
        { 
            CalculateAffinity();
            PlayerPrefs.SetString("activeDistance", "H");
        }

        tm.text = "Affinity: " + Affinity;
        tm.color = new Color(0.8f, 0.8f, 0.8f);
        tm.fontStyle = FontStyle.Bold;
        tm.alignment = TextAlignment.Center;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.characterSize = 0.065f;
        tm.fontSize = 60;

        probability = Random.Range(1, 100);

        originalEnemyPos = transform.position;

        //random fovangle and radius
        fovAngle = Random.Range(70, 180);
        fovRadius = Random.Range(4, 10);

        Health = MaxHealth;
    }

    private IEnumerator FoVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private IEnumerator lifelineCount()
    {
        WaitForSeconds wait = new WaitForSeconds(10);

        while (true)
        {
            yield return wait;
            Health -= 5;
            healthBarSprite.fillAmount = (Health / MaxHealth);
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FieldOfViewCheck()
    {
        //check for colliders
        //from the centre of the enemy with the radius
        //look for object with the specific layermask (the player)
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, targetMask);

        //if there exists a collider with player layermask
        if (rangeChecks.Length != 0)
        {
            //there is only the player there so just get the first one
            Transform target = rangeChecks[0].transform;
            //where the enemy is looking to where the player is
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                //is the player close enough 
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //if (probability <= Affinity)
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
                eye.GetComponent<Renderer>().material = eyeRed;
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
        tm.text = "I am dead";
        gameObject.GetComponent<Renderer>().material = black;
        IsDead = true;
        Health = 0;
        ClonalHelper();
        takeToGerminal();
        //CollisionHelper();
        //StartCoroutine(anotherWaiter());

        IsDead = false;
        
    }

    private void takeToGerminal()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies)
        {
            if(e != gameObject)
            {
                e.SetActive(false);
            }
        }
        player.SetActive(false);
        mainCam.SetActive(true);
        navMeshAgent.SetDestination(germinalCenter.transform.position);
        
        StartCoroutine(waiter());
        
    }

    IEnumerator waiter()
    {
        float time = 6f;
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "BridgeScene")
            time = 20f;
        yield return new WaitForSeconds(time);
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    clone.SetActive(true);
                    clone.GetComponent<NavMeshAgent>().SetDestination(germinalPos2.transform.position);
                }
            }
        }
        yield return new WaitForSeconds(6f);
        clone.SetActive(false);
        clone.transform.position = germinalCenter.transform.position;
        player.SetActive(true);
        mainCam.SetActive(false);
        CollisionHelper();
    }

    IEnumerator anotherWaiter()
    {
        yield return new WaitForSeconds(6f);
        CollisionHelper();
        clone.SetActive(false);
        player.SetActive(true);
        mainCam.SetActive(false);
    }

    public void CollisionHelper()
    {
        isPatrol = true;
        canSeePlayer = false;
        currentWaypoint = 0;
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        navMeshAgent.enabled = false;
        transform.position = originalEnemyPos;
        navMeshAgent.enabled = true;
        //navMeshAgent.Move(originalEnemyPos);
        GetComponent<Renderer>().material = blue;
        eye.GetComponent<Renderer>().material = eyeBlue;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        _waitTime = startWaitTime;

        if (end)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = PlayerController.originalPlayerPos;
            cc.enabled = true;

            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //int i = 0;
            foreach (GameObject e in enemies)
            {
                if (e != gameObject)
                {
                    e.SetActive(true);
                    if (Random.Range(1, 100) <= probability)
                        e.GetComponent<EnemyController>().Affinity = Affinity;
                    e.GetComponent<EnemyController>().CollisionHelper();
                }
            }

            GameObject[] playerGroup = GameObject.FindGameObjectsWithTag("PlayerGroup");
            foreach (GameObject pg in playerGroup)
            {
                pg.GetComponent<FollowPlayer>().CollisionHelper();
            }
        }
        end = false;

        _caughtPlayer = false;
        tm.text = "Affinity: " + Affinity;
        Health = MaxHealth;
        healthBarSprite.fillAmount = 1;
    }

    private void ClonalHelper()
    {
        //get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        //go to each enemy and change values
        foreach (GameObject e in enemies)
        {
            if(e != gameObject)
            {
                //random values in a range to allow for stomatic hypermutation
                var randAngle = Random.Range(fovAngle - (fovAngle * (diff / 100)), fovAngle + diff);
                var randRadius = Random.Range(fovRadius - (fovRadius * (diff / 100)), fovRadius + diff);
                var randMaxHealth = Random.Range(MaxHealth - (MaxHealth * (diff / 100)), MaxHealth + diff);
                var randAffinity = Random.Range(Affinity - (Affinity * (diff / 100)), Affinity + diff);

                e.GetComponent<EnemyController>().fovAngle = randAngle;
                e.GetComponent<EnemyController>().fovRadius = randRadius;
                e.GetComponent<EnemyController>().MaxHealth = randMaxHealth;
                e.GetComponent<EnemyController>().Affinity = randAffinity;
            }
        }

        diff -= Random.Range(0, 5);
    }

    private void Patrolling()
    {
        gameObject.GetComponent<Renderer>().material = blue;
        eye.GetComponent<Renderer>().material = eyeBlue;


        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        Move(speedWalk);

        //if reached desitination
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            //waiting done
            if (_waitTime <= 0)
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
        Affinity = 100 - ((Affinity / 12) * 100);
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
            for (var j = 1; j <= string2.Length; j++)
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

    // Function to calculate the Jaro Similarity of two strings
    static double jaroDistance(string s1, string s2)
    {
        // If the strings are equal
        if (s1 == s2)
            return 1.0;

        // Length of two strings
        int len1 = s1.Length,
            len2 = s2.Length;

        if (len1 == 0 || len2 == 0)
            return 0.0;

        // Maximum distance upto which matching
        // is allowed
        int maxDist = (int)Math.Floor((double)
                        Math.Max(len1, len2) / 2) - 1;

        // Count of matches
        int match = 0;

        // Hash for matches
        int[] hash_s1 = new int[s1.Length];
        int[] hash_s2 = new int[s2.Length];

        // Traverse through the first string
        for (int i = 0; i < len1; i++)
        {

            // Check if there is any matches
            for (int j = Math.Max(0, i - maxDist);
                j < Math.Min(len2, i + maxDist + 1); j++)

                // If there is a match
                if (s1[i] == s2[j] &&
                    hash_s2[j] == 0)
                {
                    hash_s1[i] = 1;
                    hash_s2[j] = 1;
                    match++;
                    break;
                }
        }

        // If there is no match
        if (match == 0)
            return 0.0;

        // Number of transpositions
        double t = 0;

        int point = 0;

        // Count number of occurrences
        // where two characters match but
        // there is a third matched character
        // in between the indices
        for (int i = 0; i < len1; i++)
            if (hash_s1[i] == 1)
            {

                // Find the next matched character
                // in second string
                while (hash_s2[point] == 0)
                    point++;

                if (s1[i] != s2[point++])
                    t++;
            }
        t /= 2;

        // Return the Jaro Similarity
        return (((double)match) / ((double)len1)
                + ((double)match) / ((double)len2)
                + ((double)match - t) / ((double)match))
            / 3.0;
    }

    // Jaro Winkler Similarity
    double CalculateJaro()
    {

        var string1 = PlayerController.tempSignature;
        var string2 = Signature;
        double jaroDist = jaroDistance(string1, string2);

        // If the jaro Similarity is above a threshold
        if (jaroDist > 0.7)
        {

            // Find the length of common prefix
            int prefix = 0;

            for (int i = 0; i < Math.Min(string1.Length,
                                        string2.Length); i++)
            {

                // If the characters match
                if (string1[i] == string2[i])
                    prefix++;

                // Else break
                else
                    break;
            }

            // Maximum of 4 characters are allowed in prefix
            prefix = Math.Min(4, prefix);

            // Calculate jaro winkler Similarity
            jaroDist += 0.1 * prefix * (1 - jaroDist);
        }
        Affinity = (float)jaroDist * 100;
        return jaroDist;
    }
}

/* then clonal expansion of those that did detection and attack
 */