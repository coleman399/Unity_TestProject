using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;

    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private Vector3 lastInteractionDir;

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 movDir = new Vector3(inputVector.x, .5f, inputVector.y);

        if (movDir != Vector3.zero)
        {
            lastInteractionDir = movDir;
        }
        

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit hit, interactDistance)) 
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("No hit");
        }

    }


    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 movDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = movementSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, movDir, moveDistance);

        transform.forward = Vector3.Slerp(transform.forward, movDir, 0.1f);

        if (!canMove)
        {
            // Cannot move towards movDir
            Vector3 moveDirX = new Vector3(movDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can only move on the X axis
                movDir = moveDirX;
            }
            else
            {
                // Can only move on the Z axis
                Vector3 moveDirZ = new Vector3(0, 0, movDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can only move on the Z axis
                    movDir = moveDirZ;
                }
                else
                {
                    // Cannot move at all
                    movDir = Vector3.zero;
                }

            }
        }

        if (canMove)
        {
            transform.position += movDir * moveDistance;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
