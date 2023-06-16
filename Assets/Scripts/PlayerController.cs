using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{

    public static PlayerController Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public _BaseCounter selectedCounter;
    }

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractionDir;
    private _BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

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
        switch (selectedCounter)
        {
            case ClearCounter clearCounter:
                clearCounter.Interact(this);
                break;
            case ContainerCounter containerCounter:
                containerCounter.Interact(this);
                break;
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
            if (hit.transform.TryGetComponent(out _BaseCounter counter))
            {
                // Has ClearCounter
                if (counter != this.selectedCounter)
                {
                    SetSelectedCounter(counter);
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

    private void SetSelectedCounter(_BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public Transform GetKitchenObjectHoldPoint()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObjectHoldPoint(Transform kitchenObjectHoldPoint)
    {
        this.kitchenObjectHoldPoint = kitchenObjectHoldPoint;
    }

    public void CreateKitchenObject(Transform hostObject)
    {
        throw new NotImplementedException();
    }

    public void DestroyKitchenObject()
    {
        KitchenObject kitchenItemToBeDestroyed = kitchenObject;
        Destroy(kitchenObject.gameObject);
        SetKitchenObject(null);
        if (!kitchenObject)
        {
            Debug.Log(kitchenItemToBeDestroyed + " has been destroyed");
        }
        else
        {
            Debug.Log(kitchenObject + " has not been destroyed");
        }
    }

    public void CreateKitchenObject(Transform host, Transform prefab)
    {
        SetKitchenObject(Instantiate(prefab, kitchenObjectHoldPoint).GetComponent<KitchenObject>());
        kitchenObject.Host = this;
        Debug.Log(kitchenObject.Host.ToString() + " is hosting kitchenObject " + kitchenObject);
    }
}
