using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : _BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private ContainerCounter secondCounter;
    [SerializeField] private Transform kitchenCounterObjectSpawnPoint;
    [SerializeField] private bool testing;

    private KitchenObject kitchenObject;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("testing...");
            if (secondCounter != null)
            {

            }
        }
    }
    public void Interact(PlayerController thePlayerInteractingWithTheObject)
    {
        Debug.Log("Interact!");

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

    private KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    private void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    private Transform GetKitchenCounterObjectSpawnPoint()
    {
        return kitchenCounterObjectSpawnPoint;
    }

    private void SetKitchenCounterObjectSpawnPoint(Transform kitchenCounterObjectSpawnPoint)
    {
        this.kitchenCounterObjectSpawnPoint = kitchenCounterObjectSpawnPoint;
    }


    public KitchenObjectSO GetCounterKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetCounterKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        this.kitchenObjectSO = kitchenObjectSO;
    }

    public void CreateKitchenObject(Transform host, Transform prefab)
    {
        SetKitchenObject(Instantiate(prefab, GetKitchenCounterObjectSpawnPoint()).GetComponent<KitchenObject>());
        kitchenObject.Host = this;
        Debug.Log(kitchenObject.Host.ToString() + " is hosting kitchenObject " + kitchenObject);
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
}
