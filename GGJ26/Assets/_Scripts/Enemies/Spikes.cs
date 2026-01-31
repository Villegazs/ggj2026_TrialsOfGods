using UnityEngine;
using System.Collections;

public class Spikes : Hazard
{
    [Header("Spike Settings")]
    [SerializeField] private bool continuousDamage = true;
    
    private Coroutine damageCoroutine;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other) && continuousDamage)
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ContinuousDamage(other));
            }
        }
        else
        {
            base.OnTriggerEnter(other);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other) && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    
    private IEnumerator ContinuousDamage(Collider player)
    {
        while (true)
        {
            DamagePlayer(player);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}