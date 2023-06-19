using System;
using UnityEngine;

public class CuttingCounter : _BaseCounter, IInteractableObject
{
    [SerializeField] private KitchenObjectSO[] kitchenObjectSO;

    private readonly ProgressBarUI progressBar;

    public event EventHandler<OnChopEventArgs> ChopEvent;

    public class OnChopEventArgs : EventArgs
    {
        public KitchenObject kitchenObject;
        public CuttingCounter counter;
    }


    public void Interact(PlayerController thePlayerInteractingWithTheObject)
    {

        Debug.Log("Interact!");

        KitchenObject kitchenObject = GetKitchenObject();

        if (!kitchenObject)
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
                Debug.Log("Player is not holding a kitchen object");
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

    public void InteractAlternate(PlayerController thePlayerInteractingWithTheObject)
    {
        Debug.Log("Interact Alternate!");

        KitchenObject kitchenObject = GetKitchenObject();

        if (!kitchenObject)
        {
            Debug.Log("Nothing to Chop.");
        }
        else
        {
            switch (kitchenObject.KitchenObjectSO.prefab.name)
            {
                case KitchenObjectType.CABBAGE:
                    Debug.Log(thePlayerInteractingWithTheObject.name + " Chopped Cabbage");
                    Chop(kitchenObject, thePlayerInteractingWithTheObject, 0);
                    break;
                case KitchenObjectType.TOMATO:
                    Debug.Log(thePlayerInteractingWithTheObject.name + " Chopped Tomato");
                    Chop(kitchenObject, thePlayerInteractingWithTheObject, 1);
                    break;
                case KitchenObjectType.CHEESE_BLOCK:
                    Debug.Log(thePlayerInteractingWithTheObject.name + " Chopped Cheese Block");
                    Chop(kitchenObject, thePlayerInteractingWithTheObject, 2);
                    break;
            }
        }
    }

    public void Chop(KitchenObject kitchenObject, PlayerController thePlayerInteractingWithTheObject, int kitchenObjectType)
    {
        int chopProgress = kitchenObject.GetChopProgress();
        int maxChopProgress = kitchenObject.GetMaxChopProgress();
        kitchenObject.SetChopProgress(chopProgress += 1);

        if (chopProgress >= maxChopProgress)
        {
            DestroyKitchenObject();
            CreateKitchenObject(GetKitchenCounterObjectSpawnPoint(), kitchenObjectSO[kitchenObjectType].prefab);
        }

        ChopEvent?.Invoke(this, new OnChopEventArgs { kitchenObject = kitchenObject });

    }
}
