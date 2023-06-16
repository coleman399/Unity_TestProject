using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class _BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    private Transform kitchenCounterObjectSpawnPoint;
    private KitchenObject kitchenObject;

    private void Start()
    {
        SetKitchenCounterObjectSpawnPoint(transform.Find("KitchenCounterObjectSpawnPoint"));
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
        Destroy(GetKitchenObject().gameObject);
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

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public Transform GetKitchenCounterObjectSpawnPoint()
    {
        return kitchenCounterObjectSpawnPoint;
    }

    private void SetKitchenCounterObjectSpawnPoint(Transform kitchenCounterObjectSpawnPoint)
    {
        this.kitchenCounterObjectSpawnPoint = kitchenCounterObjectSpawnPoint;
    }
}