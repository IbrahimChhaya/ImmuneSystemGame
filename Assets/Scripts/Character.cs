using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float AttackDmg;
    public float MoveSpeed;
    public bool IsDead = false;
    public string Signature;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Move()
    {

    }

    public virtual void Attack()
    {

    }


}
