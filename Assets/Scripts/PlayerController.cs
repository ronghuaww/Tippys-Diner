using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    public float friction = 5f;
    private Rigidbody rb;
    public Camera playerCamera;

    // hello
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = friction;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        rb.AddForce(moveDirection * moveSpeed);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (horizontal == 0 && vertical == 0)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, friction * Time.deltaTime);
        }
    }
}
