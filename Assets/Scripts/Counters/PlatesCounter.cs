using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateRemove;
    public event EventHandler OnPlateSpawned;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimerMax = 3.0f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Start()
    {
        StartCoroutine(SpawnPlateCoroutine());
    }

    private System.Collections.IEnumerator SpawnPlateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnPlateTimerMax);
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            //player is empty handed!
            if (platesSpawnedAmount > 0)
            {
                //there's at least one plate here!
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemove?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
