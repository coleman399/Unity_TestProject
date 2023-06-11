using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { get; private set; }


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private Vector3 lastInteractionDir;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be two player controllers.");
        }
        Instance = this;
    }


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        
        if (selectedCounter != null)
        {
            this.selectedCounter.clearCounter = selectedCounter;
            this.selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 movDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (movDir != Vector3.zero)
        {
            lastInteractionDir = movDir;
        }
        

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit hit, interactDistance, counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                if (clearCounter != this.selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                // Does not have ClearCounter
                SetSelectedCounter(null);
            }
        }
        else
        {
            // Does not have ClearCounter
            SetSelectedCounter(null);
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

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {

        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });        
    }   
}
