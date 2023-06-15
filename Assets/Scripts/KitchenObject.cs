using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public IKitchenObjectParent Host { get; set; }

    public KitchenObjectSO KitchenObjectSO { get => kitchenObjectSO; private set => kitchenObjectSO = value; }

}
