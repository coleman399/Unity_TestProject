using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform kitchenCounterObjectSpawnPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;

    private KitchenObject kitchenObject { get; set; }
    public ClearCounter clearCounter { get; set; }

    ClearCounter(ClearCounter clearCounter)
    {
        this.clearCounter = clearCounter;
    }

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


    public void Interact()
    {
        Debug.Log("Interact!");

        //Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, kitchenCounterObjectSpawnPoint);
        //kitchenObjectTransform.localPosition = Vector3.zero;
        //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().KitchenObjectSO.name);

        if (kitchenObject == null)
        {
            CreateKitchenObject(clearCounter);
        }
        else
        {
            Destroy(kitchenObject.gameObject);
            kitchenObject = null;
            Debug.Log("kitchenObject has been destroyed");
            CreateKitchenObject(clearCounter);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    { 
        return kitchenCounterObjectSpawnPoint;
    }

    public void CreateKitchenObject(ClearCounter hostClearCounter)
    {
        hostClearCounter.kitchenObject = Instantiate(kitchenObjectSO.prefab, hostClearCounter.kitchenCounterObjectSpawnPoint).GetComponent<KitchenObject>();
        hostClearCounter.kitchenObject.clearCounter = hostClearCounter;
        Debug.Log("interacting with " + hostClearCounter.kitchenObject.clearCounter.name + " | kitchenObject equals " + hostClearCounter.kitchenObject);
    }

    public bool HasKitchenObject()
    {
        return kitchenObject? true: false;
    }    
}
