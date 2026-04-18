using System; // needed for EventHandler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            // Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            // kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

        }

        //     if (!HasKitchenObject())
        //     {
        //         Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        //         kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

        //         kitchenObjectTransform.localPosition = Vector3.zero;
        //         kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        //         kitchenObject.SetClearCounter(this);
        //     }
        //     else
        //     {
        //         give the object to the player
        //         kitchenObject.SetKitchenObjectParent(player);
        //         Debug.Log(kitchenObject.GetClearCounter());
        //     }
    }


}
