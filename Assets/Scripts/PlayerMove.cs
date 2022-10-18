using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float player_Speed = 10.0f;
    public float player_Tilt = 0.1f;
    public Vector3 player_Direction;
    public float player_Turn = 0.0f;
    public float player_TurnSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player_Turn += Input.GetAxis("Mouse X");
        float player_Horizontal = Input.GetAxis("Horizontal");
        float player_Vertical = Input.GetAxis("Vertical");
        
        //gameObject.GetComponent<Rigidbody>().velocity = player_Direction * player_Speed;
        //gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, player_Turn * player_TurnSpeed, gameObject.GetComponent<Rigidbody>().velocity.x * -player_Tilt);


        player_Direction = transform.right * player_Horizontal + transform.forward * player_Vertical;
        player_Direction.y = 0f; // May shorten direction vector!
        gameObject.GetComponent<Rigidbody>().velocity = player_Direction.normalized * player_Speed;
    }
}
