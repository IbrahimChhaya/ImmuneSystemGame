using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    public static PlayerController instance;
    public static Vector3 originalPlayerPos;

    public static string tempSignature;

    public static bool isBeingChased;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateSignature();
        tempSignature = Signature;
        originalPlayerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getSignature()
    {
        return Signature;
    }
}
