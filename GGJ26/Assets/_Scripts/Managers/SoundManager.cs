using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "soundEffectVolume";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipsRefSO audioClipsRefSO;
    
    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
    }

    private void Start()
    {
        StaticEventHandler.OnJump += StaticEventHandler_OnJump;
        StaticEventHandler.OnLand +=StaticEventHandler_OnLand;
        StaticEventHandler.OnDash += StaticEventHandler_OnDash;
        StaticEventHandler.OnWindMaskUnlocked +=StaticEventHandler_OnWindMaskUnlocked;
        StaticEventHandler.OnWindMaskActivated += StaticEventHandler_OnWindMaskUnlocked;
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }    
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * volume);
    }

    private void StaticEventHandler_OnWindMaskUnlocked()
    {
        Player player = Player.Instance;
        PlaySound(audioClipsRefSO.windMask, player.transform.position);
    }
    private void StaticEventHandler_OnJump()
    {
        Player player = Player.Instance;
        PlaySound(audioClipsRefSO.jump, player.transform.position);
    }
    
    private void StaticEventHandler_OnLand()
    {
        Player player = Player.Instance;
        PlaySound(audioClipsRefSO.land, player.transform.position);
    }

    private void StaticEventHandler_OnDash(bool isPlayerDashing)
    {
        if (isPlayerDashing)
        {
            Player player = Player.Instance;
            PlaySound(audioClipsRefSO.dash, player.transform.position);

        }
    }
    
    public void PlayStepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsRefSO.steps, position, volume);
    }
    
    private void OnDestroy()
    {
        StaticEventHandler.OnJump -= StaticEventHandler_OnJump;
        StaticEventHandler.OnLand -= StaticEventHandler_OnLand;
        StaticEventHandler.OnDash -= StaticEventHandler_OnDash;
        StaticEventHandler.OnWindMaskUnlocked -= StaticEventHandler_OnWindMaskUnlocked;
        StaticEventHandler.OnWindMaskActivated -= StaticEventHandler_OnWindMaskUnlocked;
    }
}
