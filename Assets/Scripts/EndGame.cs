using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private Vector3 originalPlayerPos;
    private Vector3[] originalEnemyPoses;
    public static bool end = false;

    private float diff = 20f;
 
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
        //ClonalHelper();
        ScoreManager.instance.AddWin();
    }

    private void CollisionHelper()
    {
        //find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //reset player position
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = PlayerController.originalPlayerPos;
        cc.enabled = true;

        //get enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //reset enemies
        foreach (GameObject e in enemies)
        {
            e.GetComponent<EnemyController>().probability = Random.Range(1, 100);
            e.GetComponent<EnemyController>().CollisionHelper();
        }

        //reset player group
        GameObject[] playerGroup = GameObject.FindGameObjectsWithTag("PlayerGroup");
        foreach(GameObject pg in playerGroup)
        {
            pg.GetComponent<FollowPlayer>().CollisionHelper();
        }
    }

    private void ClonalHelper()
    {
        //get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //store clonal enemies
        List<GameObject> clonalEnemies = new List<GameObject>();

        //get all dead enemies
        foreach (GameObject e in enemies)
        {
            if(e.GetComponent<EnemyController>().IsDead)
            {
                clonalEnemies.Add(e);
            }
        }

        //get a random dead enemy
        int rand = Random.Range(0, clonalEnemies.Count - 1);
        var enemyToClone = clonalEnemies[rand].GetComponent<EnemyController>();

        var fovAngle = enemyToClone.fovAngle;
        var fovRadius = enemyToClone.fovRadius;
        var maxHealth = enemyToClone.MaxHealth;
        var affinity = enemyToClone.Affinity;
        
        //go to each enemy and change values
        foreach (GameObject e in enemies)
        {
            //random values in a range to allow for somatic hypermutation
            var randAngle = Random.Range(fovAngle - (fovAngle * (diff/100)), fovAngle + diff);
            var randRadius = Random.Range(fovRadius - (fovRadius * (diff / 100)), fovRadius + diff);
            var randMaxHealth = Random.Range(maxHealth - (maxHealth * (diff / 100)), maxHealth + diff);
            var randAffinity = Random.Range(affinity - (affinity * (diff / 100)), affinity + diff);

            e.GetComponent<EnemyController>().fovAngle = randAngle;
            e.GetComponent<EnemyController>().fovRadius = randRadius;
            e.GetComponent<EnemyController>().MaxHealth = randMaxHealth;
            e.GetComponent<EnemyController>().Affinity = randAffinity;
            e.GetComponent<EnemyController>().IsDead = false;
        }

        diff -= Random.Range(0, 5);
    }
}
