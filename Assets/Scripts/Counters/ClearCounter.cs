using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // [SerializeField] private ClearCounter secondClearCounter;
    // [SerializeField] private bool testing;


    // private void Update()
    // {
    //     if (testing && Input.GetKeyDown(KeyCode.T))
    //     {
    //         if (kitchenObject != null)
    //         {
    //             kitchenObject.SetKitchenObjectParent(secondClearCounter);
    //             Debug.Log(kitchenObject.GetClearCounter());

    //         }
    //     }
    // }
    public override void Interact(PlayerController player)
    {
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here!
            if (player.HasKitchenObject())
            {
                // Player is carrying something 
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player is not carrying anything
            }
        }
        else
        {
            // There is a KitchenObject here!
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    // PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                    }
                }
                else
                {
                    //player is not carrying plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }


        // if (kitchenObject == null)
        // {
        //     Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);

        //     kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);

        // kitchenObjectTransform.localPosition = Vector3.zero;
        // kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        // kitchenObject.SetClearCounter(this);
        // }
        // else
        // {
        //     give the object to the player
        //     kitchenObject.SetKitchenObjectParent(player);
        //     Debug.Log(kitchenObject.GetClearCounter());
        // }
    }


}

