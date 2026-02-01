using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUI : MonoBehaviour
{
    public HUDUI Instance { get; private set; }
    [SerializeField] private Image healthBar;
    [SerializeField] private Image windMaskImage;
    [SerializeField] private Transform windMaskContainer;
    
    private float timer;
    private float cooldownTimer;
    private float timerDuration;
    private float cooldownDuration;
    
    
    private int actualHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        windMaskContainer.gameObject.SetActive(false);
        StaticEventHandler.OnMaskEquippedTimer += StaticEventHandler_OnStartTimer;
        StaticEventHandler.OnMaskCooldownTimer += StaticEventHandler_OnStartCooldownTimer;
        StaticEventHandler.OnWindMaskUnlocked+= StaticEventHandler_OnWindMaskUnlocked;
        Player.Instance.OnApplyDamage += Player_OnApplyDamage;
        UpdateHealthBar();
    }
    
    private void StaticEventHandler_OnWindMaskUnlocked()
    {
        Debug.Log("Wind Mask unlocked!");
        windMaskContainer.gameObject.SetActive(true);
    }
    private void StaticEventHandler_OnStartTimer(float time)
    {
        timerDuration = time;
        timer = timerDuration;
        Debug.Log("Timer started");
    }
    
    private void StaticEventHandler_OnStartCooldownTimer(float time)
    {
        Debug.Log("Cooldown started");
        cooldownDuration = time;
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
            windMaskImage.fillAmount = 1 - (cooldownTimer / cooldownDuration);
            cooldownTimer -= Time.deltaTime;
            
        }
    }
    
}
