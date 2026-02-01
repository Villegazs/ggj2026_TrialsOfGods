using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    
    
    private int actualHealth;

    private void Start()
    {
        Player.Instance.OnApplyDamage += Player_OnApplyDamage;
        UpdateHealthBar();
    }
    
    private void Player_OnApplyDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthNormalized = Player.Instance.GetHealthNormalized();
        healthBar.fillAmount = healthNormalized;
    }
}
