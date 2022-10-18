using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float baseSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintSpeed = 5f;

    float speedBoost = 1f;
    Vector3 velocity;

    public GameObject panel;
    bool paused;
    void Start()
    {
        paused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            if(paused == false)
            {
                panel.gameObject.SetActive(true);
                paused = true;
                Time.timeScale = 0f;
            }
            else
            {
                paused = false;
                panel.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }    

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetButton("Fire3"))
            speedBoost = sprintSpeed;
        else
            speedBoost = 1f;


        //Vector3 move = transform.right * z + transform.forward * x;
        Vector3 move = new Vector3(x, 0, z);

        controller.Move(move * (baseSpeed + speedBoost) * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
