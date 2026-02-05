using UnityEngine;

public class StepSound : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private float footStepTimer;
    private float footStepTimerMax = 0.4f;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer < 0f)
        {
            footStepTimer = footStepTimerMax;

            if (characterMovement.IsWalking())
            {
                float volume = 1f;
            
                SoundManager.Instance.PlayStepsSound(characterMovement.transform.position, volume);
            }
        }
    }
}
