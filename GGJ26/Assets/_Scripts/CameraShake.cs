using System;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    [Header("Shake default")]
    [SerializeField] private float defaultDuration = 0.25f;
    [SerializeField] private float defaultMagnitude = 0.25f;
    [SerializeField] private CinemachineBasicMultiChannelPerlin shakeCamera;
    private Vector3 originalLocalPos;
    private Coroutine shakeCoroutine;
    

    private void Start()
    {
        Player.Instance.OnApplyDamage += Player_OnApplyDamage;
    }
    
    private void Player_OnApplyDamage(object sender, EventArgs e)
    {
        Shake(defaultDuration, defaultMagnitude);
    }
    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float damper = 1f - Mathf.Clamp01(elapsed / duration); // reduce intensidad con el tiempo
            float amplitude = magnitude * damper;
            float frecuency = magnitude * damper;
            
            shakeCamera.AmplitudeGain = amplitude;
            shakeCamera.FrequencyGain = frecuency;

            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeCamera.AmplitudeGain = 0f;
        shakeCamera.FrequencyGain = 0f;
        shakeCoroutine = null;
    }
    
    private void OnDestroy()
    {
        Player.Instance.OnApplyDamage -= Player_OnApplyDamage;
    }
}
