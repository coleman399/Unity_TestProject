using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private int MaxChopProgress;

    private int ChopProgress;

    public IKitchenObjectParent Host { get; set; }

    public KitchenObjectSO KitchenObjectSO { get => kitchenObjectSO; private set => kitchenObjectSO = value; }

    public int GetMaxChopProgress()
    {
        return MaxChopProgress;
    }

    public int GetChopProgress()
    {
        return ChopProgress;
    }

    public void SetChopProgress(int chopProgress)
    {
        ChopProgress = chopProgress;
    }

}
