using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : _BaseCounter, IInteractableObject
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler playerWithdrewKitchenObject;

    public void Interact(PlayerController thePlayerInteractingWithTheObject)
    {
        Debug.Log("Interact!");

        KitchenObject kitchenObject = GetKitchenObject();

        if (kitchenObject == null)
        {
            if (thePlayerInteractingWithTheObject.GetKitchenObject())
            {
                KitchenObject playerHeldKitchenObject = thePlayerInteractingWithTheObject.GetKitchenObject();
                SetKitchenObject(playerHeldKitchenObject);
                playerHeldKitchenObject.transform.SetParent(GetKitchenCounterObjectSpawnPoint());
                playerHeldKitchenObject.transform.localPosition = Vector3.zero;
                playerHeldKitchenObject.Host = this;
                thePlayerInteractingWithTheObject.SetKitchenObject(null);
            }
            else
            {
                thePlayerInteractingWithTheObject.CreateKitchenObject(GetKitchenCounterObjectSpawnPoint(), kitchenObjectSO.prefab);
                playerWithdrewKitchenObject?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if (thePlayerInteractingWithTheObject.GetKitchenObject())
            {
                Debug.Log("Player is already holding a kitchen object");
            }
            else
            {
                kitchenObject.transform.SetParent(thePlayerInteractingWithTheObject.GetKitchenObjectHoldPoint());
                kitchenObject.transform.localPosition = Vector3.zero;
                kitchenObject.Host = thePlayerInteractingWithTheObject;
                thePlayerInteractingWithTheObject.SetKitchenObject(kitchenObject);
                SetKitchenObject(null);
            }
        }
    }

    public KitchenObjectSO GetCounterKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetCounterKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        this.kitchenObjectSO = kitchenObjectSO;
    }
}
