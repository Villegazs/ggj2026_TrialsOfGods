using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUI : MonoBehaviour
{
    public HUDUI Instance { get; private set; }
    [SerializeField] private Image healthBar;
    [SerializeField] private Image windMaskImage;
    
    private float timer;
    private float cooldownTimer;
    private float timerDuration;
    
    
    private int actualHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StaticEventHandler.OnMaskEquippedTimer += StaticEventHandler_OnStartTimer;
        StaticEventHandler.OnMaskCooldownTimer += StaticEventHandler_OnStartCooldownTimer;
        Player.Instance.OnApplyDamage += Player_OnApplyDamage;
        UpdateHealthBar();
    }
    private void StaticEventHandler_OnStartTimer(float time)
    {
        timerDuration = time;
        timer = timerDuration;
        Debug.Log("Timer started");
    }
    
    private void StaticEventHandler_OnStartCooldownTimer(float time)
    {
        timerDuration = time;
        cooldownTimer = time;
    }

    private void Update()
    {
        MaskUI();
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
    
    private void MaskUI()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            windMaskImage.fillAmount = timer / timerDuration;
        }
        else if(cooldownTimer > 0)
        {
            windMaskImage.fillAmount = 1 - (cooldownTimer / timerDuration);
            cooldownTimer -= Time.deltaTime;
            
        }
    }
    
}
