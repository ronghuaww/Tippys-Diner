using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour, IInteractable
{
    private bool isPickedUp = false; // To track if the food is already picked up

    public bool IsPickedUp => isPickedUp; // Public property to access the picked up state

    public void PlayerInteract()
    {
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
            transform.position = carryPoint.position;
            transform.SetParent(carryPoint);
            isPickedUp = true;

            Collider foodCollider = GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.enabled = false;
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
            transform.SetParent(null);
            StartCoroutine(DropLerpAnimation());

            Collider foodCollider = GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.enabled = true;
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
        Vector3 endPosition = playerTransform.position + playerTransform.forward * 1.5f;
        float arcHeight = 1.0f;
        float duration = 0.5f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            currentPos.y += arcHeight * Mathf.Sin(Mathf.PI * t);
            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPosition;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
