using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private Vector3 originalPlayerPos;
    private Vector3[] originalEnemyPoses;
    public static bool end = false;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //ScoreManager.instance.AddWin();
        CollisionHelper();
        ScoreManager.instance.AddWin();
    }

    private void CollisionHelper()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = PlayerController.originalPlayerPos;
        cc.enabled = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //int i = 0;
        foreach (GameObject e in enemies)
        {
            e.GetComponent<EnemyController>().probability = Random.Range(1, 100);
            e.GetComponent<EnemyController>().CollisionHelper();
        }

        GameObject[] playerGroup = GameObject.FindGameObjectsWithTag("PlayerGroup");
        foreach(GameObject pg in playerGroup)
        {
            pg.GetComponent<FollowPlayer>().CollisionHelper();
        }
    }
}
