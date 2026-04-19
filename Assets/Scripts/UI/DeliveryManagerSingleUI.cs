using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform IconContainer;
    [SerializeField] private Transform IconTemplate;


    private void Awake()
    {
        IconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        for (int i = IconContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = IconContainer.GetChild(i);
            if (child == IconTemplate) continue;
            PoolManager.Instance.ReturnObject(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            GameObject iconObj = PoolManager.Instance.GetObject(IconTemplate.gameObject, Vector3.zero, Quaternion.identity, IconContainer);
            Transform iconTransform = iconObj.transform;
            iconTransform.localPosition = Vector3.zero;
            iconTransform.localRotation = Quaternion.identity;
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
