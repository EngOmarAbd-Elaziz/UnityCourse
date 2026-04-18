using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animetor;
    private const string IS_WALKING = "isWalking";
    [SerializeField] private PlayerController playerController;
    private void Awake()
    {
        animetor = GetComponent<Animator>();
    }

    private void Update()
    {
        animetor.SetBool(IS_WALKING, playerController.IsWalking());
    }
}
