using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float AttackDmg;
    public float MoveSpeed;
    public bool IsDead = false;
    public string Signature;
    public float Affinity = 0;
    
    //private static Random random;

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

    public virtual void GenerateSignature()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_+=/`~<>,.?';:{}[]|";
        Signature = "";
        int signatureLength = 12;
        if(PlayerPrefs.HasKey("signatureLength"))
        { 
            signatureLength = PlayerPrefs.GetInt("signatureLength");
        }
        for (int i = 0; i < signatureLength; i++)
        {
            Signature += chars[Random.Range(0, signatureLength)];
        }
    }

}
