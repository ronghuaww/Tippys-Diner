using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    public float friction = 5f;
    public GameObject[] tables = new GameObject[3];

    private Rigidbody rb;

    private Vector3 table_position; 
    // Start is called before the first frame update
    void Start()
    {
        int rand_table = Random.Range(0, 3); 
        table_position = tables[rand_table].transform.position; 
        rb = GetComponent<Rigidbody>();
        rb.drag = friction;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        // Move our position a step closer to the target.
        var step =  moveSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, table_position, step);

        // // Check if the position of the cube and sphere are approximately equal.
        // if (Vector3.Distance(transform.position, target.position) < 0.001f)
        // {
        //     // Swap the position of the cylinder.
        //     target.position *= -1.0f;
        // }
    }
}
