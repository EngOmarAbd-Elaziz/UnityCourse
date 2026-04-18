using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    private PlayerController playerController;
    private float footstepTimer;
    private float footstepTimerMax = 0.2f;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0.0f)
        {
            footstepTimer = footstepTimerMax;


            if (playerController.IsWalking())
            {
                float volume = 2.0f;
                SoundManager.Instance.PlayFootstepSound(playerController.transform.position, volume);
            }
        }
    }
}
