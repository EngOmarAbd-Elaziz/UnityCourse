using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeTemplate;
    [SerializeField] private Transform container;


    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveringManager.Instance.OnRecipeSpawned += DeliveringManager_OnRecipeSpawned;
        DeliveringManager.Instance.OnRecipeCompleted += DeliveringManager_OnRecipeCompleted;
        UpdateVisual();
    }

    private void DeliveringManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveringManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            if (child == recipeTemplate) continue;
            PoolManager.Instance.ReturnObject(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveringManager.Instance.GetWaitingRecipeSOList())
        {
            GameObject recipeObj = PoolManager.Instance.GetObject(recipeTemplate.gameObject, Vector3.zero, Quaternion.identity, container);
            Transform recipeTransform = recipeObj.transform;
            recipeTransform.localPosition = Vector3.zero;
            recipeTransform.localRotation = Quaternion.identity;
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
