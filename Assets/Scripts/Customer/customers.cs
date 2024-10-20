using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum CustomerState
{
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
    private bool isEating = false;
    private bool Paid = false;
    private GameObject assignedTable;
    public GameObject exit;

    private Rigidbody rb;

    [SerializeField] HappyBar hb;
    [SerializeField] Canvas ui;

    private Vector3 table_position;
    private float happinessLevel = 100f;
    public CustomerState curState;
    public CustomerOrder customerOrder;
    private CustomerAnimatorController customerAnimatorController; // Animation script

    public event Action OnLeave;

    public void Initialize(GameObject assignedTable, GameObject exitPoint)
    {
        exit = exitPoint;
        table_position = assignedTable.transform.position;

        Transform eatingPointTransform = assignedTable.transform.Find("EatingPoint");
        if (eatingPointTransform != null)
        {
            customerOrder.eatingPoint = eatingPointTransform;
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hb = GetComponentInChildren<HappyBar>();
        ui = GetComponentInChildren<Canvas>();
        customerAnimatorController = GetComponent<CustomerAnimatorController>();
    }

    void Start()
    {
        rb.drag = friction;
        hb.UpdateHappy(happinessLevel);
        curState = CustomerState.Ordering;
        ui.enabled = false;
    }

    void FixedUpdate()
    {
        // Make the UI Canvas always face the camera
        FaceCamera();

        switch (curState)
        {
            case CustomerState.Ordering:
                SearchForTable();
                break;

            case CustomerState.Waiting:
                WaitingForFood();
                break;

            case CustomerState.Eating:
                if (!isEating)
                {
                    StartCoroutine(Eating());
                }
                break;

            case CustomerState.Happy:
                HeadToExit();
                break;

            case CustomerState.Angry:
                HeadToExit();
                break;
        }
    }

    private void FaceCamera()
    {
        if (ui != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                ui.transform.LookAt(ui.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
            }
        }
    }


    private void SearchForTable()
    {
        Vector3 targetPosition = new Vector3(table_position.x, transform.position.y, table_position.z); // Preserve Y position
        if (Vector3.Distance(transform.position, targetPosition) >= 1.3f)
        {
            MoveTowardsTarget(targetPosition);
        }
        else
        {
            curState = CustomerState.Waiting;
            customerOrder.AssignFoodOrder();
        }
    }

    private void HeadToExit()
    {
        customerAnimatorController.SetWalkingAnimation(true);
        ui.enabled = false;
        if (!Paid)
        {
            if (customerOrder.playerNumber == 1)
            {
                IncomeManager.Instance.AddTip(1, happinessLevel);
                Debug.Log("Added Money");
            }
            else if (customerOrder.playerNumber == 2)
            {
                IncomeManager.Instance.AddSalary(2);
                Debug.Log("Added Money");
            }
            Paid = true;
        }

        Vector3 targetPosition = new Vector3(exit.transform.position.x, transform.position.y, exit.transform.position.z); // Preserve Y position
        if (Vector3.Distance(transform.position, targetPosition) >= 1.0f && exit)
        {
            MoveTowardsTarget(targetPosition);
        }
        else
        {
            OnLeave?.Invoke();
            Destroy(gameObject);
        }
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotate the customer to face the direction of movement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }

        rb.isKinematic = true; // Ensure that the Rigidbody doesn't interfere with movement
        customerAnimatorController.SetWalkingAnimation(true);
    }

    private void WaitingForFood()
    {
        customerAnimatorController.SetWalkingAnimation(false);
        ui.enabled = true;
        if (happinessLevel > 0)
        {
            happinessLevel -= Time.deltaTime * happinessLoss;
            hb.UpdateHappy(happinessLevel);
            if (customerOrder.OrderDone)
            {
                curState = CustomerState.Eating;
            }
        }
        else
        {
            curState = CustomerState.Angry;
        }
    }

    private IEnumerator Eating()
    {
        customerAnimatorController.SetEatingAnimation(true);
        isEating = true;
        ui.enabled = false;

        if (customerOrder.eatingPoint != null)
        {
            // Rotate to face the eating point
            Vector3 lookDirection = (customerOrder.eatingPoint.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

            Transform foodTransform = FindFoodTransform();
            if (foodTransform != null)
            {
                foodTransform.position = customerOrder.eatingPoint.position; // Snap food to the EatingPoint position
            }
        }

        yield return new WaitForSeconds(3f);

        Transform foodChild = FindFoodTransform();
        if (foodChild != null)
        {
            Food foodComponent = foodChild.GetComponent<Food>();
            foodComponent.DestroyFood();
            Debug.Log("Food destroyed");
        }

        curState = CustomerState.Happy;
        isEating = false;
        customerAnimatorController.SetEatingAnimation(false);
    }

    private Transform FindFoodTransform()
    {
        if (customerOrder.eatingPoint == null)
        {
            Debug.LogWarning("Eating point not assigned on " + gameObject.name);
            return null;
        }

        foreach (Transform child in customerOrder.eatingPoint)
        {
            if (child.CompareTag("Hamburger") || child.CompareTag("HotDog") || child.CompareTag("Soup"))
            {
                return child;
            }
        }

        return null;
    }
}
