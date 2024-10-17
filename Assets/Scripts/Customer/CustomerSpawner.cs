using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public int MaxCustomers = 3;
    public GameObject customerPrefab; // Reference to the customer prefab
    public List<GameObject> tables; // List of tables in the scene
    private List<GameObject> availableTables; // Track available tables
    public GameObject exitPoint; // Exit point for customers

    [SerializeField] private int remainingCustomers;

    void Start()
    {
        remainingCustomers = MaxCustomers;
        // Initialize the available tables list
        availableTables = new List<GameObject>(tables);

        // Optionally, start spawning customers at the beginning
        StartCoroutine(SpawnCustomers());
    }

    void FixedUpdate()
    {
        // Only check for active customers if there are no remaining customers
        if (remainingCustomers == 0 && !AreCustomersActive())
        {
            GameManager.Instance.LoadScene("Results");
        }
    }

    private IEnumerator SpawnCustomers()
    {
        while (remainingCustomers >= 1) // Change this condition to control when to spawn customers
        {
            SpawnCustomer();
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // Adjust spawn rate as necessary
        }
    }

    private void SpawnCustomer()
    {
        if (customerPrefab != null && availableTables.Count > 0)
        {
            // Choose a random available table
            GameObject assignedTable = availableTables[Random.Range(0, availableTables.Count)];

            // Instantiate the customer prefab at the spawner's position
            GameObject newCustomer = Instantiate(customerPrefab, transform.position, Quaternion.identity);

            // Initialize the customer with the assigned table and exit point
            Customers customerScript = newCustomer.GetComponent<Customers>();
            customerScript.Initialize(assignedTable, exitPoint);

            // Mark the table as occupied
            availableTables.Remove(assignedTable);

            // Subscribe to the customer's OnLeave event to free the table when they leave
            customerScript.OnLeave += () => FreeTable(assignedTable);

            remainingCustomers -= 1;
        }
    }

    private void FreeTable(GameObject table)
    {
        // Add the table back to the available list
        if (!availableTables.Contains(table))
        {
            availableTables.Add(table);
        }
    }

    private bool AreCustomersActive()
    {
        // Check if there are any active customer instances in the scene
        return GameObject.FindGameObjectsWithTag("Customer").Length > 0;
    }
}
