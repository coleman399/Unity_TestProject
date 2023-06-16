using UnityEngine;

public interface IKitchenObjectParent
{
    public void CreateKitchenObject(Transform host, Transform prefab);
    public void DestroyKitchenObject();
}
 