using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private Vector3 originalPlayerPos;
    private Vector3[] originalEnemyPoses;
 
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());

        /*originalPlayerPos = player.transform.position;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        originalEnemyPoses = new Vector3[enemies.Length];
        int i = 0;
        foreach (GameObject e in enemies)
        {
            originalEnemyPoses[i] = e.transform.position;
            i++;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter(Collider other)
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
        }
        currentWaypointIndex = 0;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        ScoreManager.instance.AddDeath();
        probability += 10;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        //ScoreManager.instance.AddWin();
        PatrolChase.end = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ScoreManager.instance.AddWin();
    }
}
