using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public ClearCounter clearCounter { get; set; }

    public KitchenObjectSO KitchenObjectSO { get => kitchenObjectSO; private set => kitchenObjectSO = value; }

    public void Clear()
    {
        if (clearCounter != null)
        {
            transform.SetParent(clearCounter.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            clearCounter = null;
        }
    }   
}
