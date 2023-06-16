using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : _BaseCounter, IInteractableObject
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private ClearCounter secondCounter;
    [SerializeField] private bool testing;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("testing...");
            if (secondCounter != null)
            {

                if (secondCounter.GetKitchenObject() == null)
                {
                    CreateKitchenObject(secondCounter.GetKitchenCounterObjectSpawnPoint(), kitchenObjectSO.prefab);
                }
                else
                {                    
                    secondCounter.DestroyKitchenObject();
                    CreateKitchenObject(secondCounter.GetKitchenCounterObjectSpawnPoint(), kitchenObjectSO.prefab);
                }
            }
            else
            {
                Debug.Log("secondCounter is null");
            }
        }
    }
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
                CreateKitchenObject(GetKitchenCounterObjectSpawnPoint(), kitchenObjectSO.prefab);
            }
        }
        else
        {
            if (thePlayerInteractingWithTheObject.GetKitchenObject())
            {
                DestroyKitchenObject();
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
