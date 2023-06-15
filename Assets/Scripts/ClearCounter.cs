using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform kitchenCounterObjectSpawnPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;

    private KitchenObject kitchenObject;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("testing...");
            if (secondClearCounter != null)
            {

                if (secondClearCounter.kitchenObject == null)
                {
                    CreateKitchenObject(secondClearCounter);
                }
                else
                {
                    Destroy(secondClearCounter.kitchenObject.gameObject);
                    secondClearCounter.kitchenObject = null;
                    Debug.Log("secondClearCounter.kitchenObject has been destroyed");
                    CreateKitchenObject(secondClearCounter);
                }


                //kitchenObject.Clear();
                //kitchenObject.clearCounter = secondClearCounter;
                //Instantiate(kitchenObject.KitchenObjectSO.prefab, secondClearCounter.GetKitchenObjectFollowTransform());
                //this.clearCounter = secondClearCounter;

            }
        }
    }


    public void Interact(PlayerController player)
    {
        Debug.Log("Interact!");

        //Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, kitchenCounterObjectSpawnPoint);
        //kitchenObjectTransform.localPosition = Vector3.zero;
        //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().KitchenObjectSO.name);

        if (kitchenObject == null)
        {
            if (player.GetKitchenObject())
            {
                KitchenObject playerHeldKitchenObject = player.GetKitchenObject();
                SetKitchenObject(playerHeldKitchenObject);
                playerHeldKitchenObject.transform.SetParent(kitchenCounterObjectSpawnPoint);
                playerHeldKitchenObject.transform.localPosition = Vector3.zero;
                playerHeldKitchenObject.Host = this;
                player.SetKitchenObject(null);
            }
            else
            {
                CreateKitchenObject(this);
            }
        }
        else
        {
            if (player.GetKitchenObject())
            {
                Destroy(kitchenObject.gameObject);
                SetKitchenObject(null);
                Debug.Log("kitchenObject has been destroyed");
            }
            else
            {
                kitchenObject.transform.SetParent(player.GetKitchenObjectHoldPoint());
                kitchenObject.transform.localPosition = Vector3.zero;
                kitchenObject.Host = player;
                player.SetKitchenObject(kitchenObject);
                SetKitchenObject(null);
            }
        }
    }

    //public Transform GetKitchenObjectFollowTransform()
    //{ 
    //    return kitchenCounterObjectSpawnPoint;
    //}

    public void CreateKitchenObject(ClearCounter hostClearCounter)
    {
        hostClearCounter.kitchenObject = Instantiate(kitchenObjectSO.prefab, hostClearCounter.kitchenCounterObjectSpawnPoint).GetComponent<KitchenObject>();
        hostClearCounter.kitchenObject.Host = hostClearCounter;
        Debug.Log(hostClearCounter.kitchenObject.Host.ToString() + " is hosting kitchenObject " + hostClearCounter.kitchenObject);
    }

    //public bool HasKitchenObject()
    //{
    //    return kitchenObject? true: false;
    //}    

    public KitchenObjectSO GetCounterKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetCounterKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        this.kitchenObjectSO = kitchenObjectSO;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }  
}
