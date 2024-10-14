using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CustomerState {
    Ordering,
    Waiting,
    Eating,
    Happy,
    Angry
}

public class Customers : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    public float friction = 5f;
    public GameObject[] tables = new GameObject[3];

    public GameObject exit; 

    private Rigidbody rb;

    [SerializeField] HappyBar hb;

    private Vector3 table_position;

    private float happinessLevel = 100f; 
    public CustomerState curState;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hb = GetComponentInChildren<HappyBar>();
    }
    void Start()
    {

        int rand_table = Random.Range(0, 3);
        table_position = tables[rand_table].transform.position;
        rb.drag = friction;
        hb.UpdateHappy(happinessLevel);
        curState = CustomerState.Ordering;
    }

// Update is called once per frame
    void FixedUpdate()
    {
        switch (curState) {
            case CustomerState.Ordering:
            SearchForTable();
            break;

            case CustomerState.Waiting:
            WaitingForFood();
            break;

            case CustomerState.Eating:
            break;

            case CustomerState.Happy:
            HeadToExit();
            break;
            
            case CustomerState.Angry:
            HeadToExit();
            break;
        }
    }

    private void SearchForTable() {
        if (Vector3.Distance(transform.position, table_position) >= 1.3f)
        {
            var step = moveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, table_position, step);
            rb.isKinematic = true;
        } else {
            curState = CustomerState.Waiting;
        }
    }

    private void WaitingForFood() {
        if (happinessLevel > 0) 
        {
            happinessLevel -= Time.deltaTime * 10f; 
            Debug.Log(happinessLevel); 
            hb.UpdateHappy(happinessLevel);
        } else {
            curState = CustomerState.Angry;
        }
    }

    private void HeadToExit() {
        if (Vector3.Distance(transform.position, exit.transform.position) >= 1.0f && exit) 
        {
            var step = moveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, exit.transform.position, step);
        }
    }
}
