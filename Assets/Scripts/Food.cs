using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour, IInteractable
{
    private bool isPickedUp = false; // To track if the food is already picked up

    public void PlayerInteract()
    {
        // Carry point has to be available
        Transform carryPoint = GameObject.FindWithTag("CarryPoint").transform;
        if (!isPickedUp)
        {
            PickUp(carryPoint);
        }
        else
        {
            Drop();
        }
    }

    public void PickUp(Transform carryPoint)
    {
        if (!isPickedUp)
        {
            // Move the food to the player's carry point
            transform.position = carryPoint.position;
            transform.SetParent(carryPoint); // Make food a child of the carry point
            isPickedUp = true; // Update the state to picked up

            Collider foodCollider = GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.enabled = false; // Disable collider when food is picked up
            }

            Debug.Log("Food picked up!");
        }
        else
        {
            Debug.Log("Food is already picked up!");
        }
    }

    public void Drop()
    {
        if (isPickedUp)
        {
            // Detach the food from the carry point
            transform.SetParent(null);
            StartCoroutine(DropLerpAnimation());

            Collider foodCollider = GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.enabled = true; // Re-enable collider when food is dropped
            }

            isPickedUp = false; // Update the state to not picked up
            Debug.Log("Food dropped!");
        }
        else
        {
            Debug.Log("Food is not currently being carried!");
        }
    }

    private IEnumerator DropLerpAnimation()
    {
        Transform playerTransform = GameObject.FindWithTag("Player").transform;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = playerTransform.position + playerTransform.forward * 1.5f; // Drop food 1.5 units in front of the player
        float arcHeight = 1.0f; // How high the arc is
        float duration = 0.5f; // Time it takes to complete the drop

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // Calculate the current position
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += arcHeight * Mathf.Sin(Mathf.PI * t); // Arc effect using sine wave

            transform.position = currentPos;
            yield return null;
        }

        // Ensure the object reaches its final position
        transform.position = endPosition;

        // Re-enable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
