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
    public float happinessLoss = 10f;
    public GameObject[] tables = new GameObject[3];

    public GameObject exit; 

    private Rigidbody rb;

    [SerializeField] HappyBar hb;
    [SerializeField] Canvas ui;


    private Vector3 table_position;

    private float happinessLevel = 100f; 
    public CustomerState curState;
    public CustomerOrder customerOrder;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hb = GetComponentInChildren<HappyBar>();
        ui = GetComponentInChildren<Canvas>();

    }
    void Start()
    {

        int rand_table = Random.Range(0, 3);
        table_position = tables[rand_table].transform.position;
        rb.drag = friction;
        hb.UpdateHappy(happinessLevel);
        curState = CustomerState.Ordering;
        ui.enabled = false; 
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
            StartCoroutine(Eating());
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
            customerOrder.AssignFoodOrder();
        }
    }

    private void WaitingForFood() {
        ui.enabled = true; 
        if (happinessLevel > 0) 
        {
            happinessLevel -= Time.deltaTime * happinessLoss; 
            Debug.Log(happinessLevel); 
            hb.UpdateHappy(happinessLevel);
            if(customerOrder.OrderDone)
            {
                curState = CustomerState.Eating;
            }
        } else {
            curState = CustomerState.Angry;
        }
    }

    private void HeadToExit() {
        if (customerOrder.playerNumber == '1')
        {
            IncomeManager.Instance.AddTip(1, happinessLevel);
        }
        else if (customerOrder.playerNumber == '2')
        {
            IncomeManager.Instance.AddSalary(2);
        }
        ui.enabled = false; 
        if (Vector3.Distance(transform.position, exit.transform.position) >= 1.0f && exit) 
        {
            var step = moveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, exit.transform.position, step);
        }
    }

    private IEnumerator Eating()
    {
        ui.enabled = false;
        // Play rat eating animation here

        yield return new WaitForSeconds(3f);
        curState = CustomerState.Happy;
    }
}
