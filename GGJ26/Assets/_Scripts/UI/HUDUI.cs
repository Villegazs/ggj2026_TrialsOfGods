using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUI : MonoBehaviour
{
    public static HUDUI Instance { get; private set; }
    [SerializeField] private Image healthBar;
    [SerializeField] private Image windMaskImage;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image sprintImage;
    [SerializeField] private Transform windMaskContainer;

    private bool hasMask;
    private float timer;
    private float cooldownMaskTimer;
    private float timerDuration;
    private float cooldownMaskDuration;
    
    
    private int actualHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dashImage.fillAmount = 0;
        sprintImage.fillAmount = 0;
        windMaskContainer.gameObject.SetActive(false);
        StaticEventHandler.OnMaskEquippedTimer += StaticEventHandler_OnStartTimer;
        StaticEventHandler.OnMaskCooldownTimer += StaticEventHandler_OnStartCooldownTimer;
        StaticEventHandler.OnWindMaskUnlocked+= StaticEventHandler_OnWindMaskUnlocked;
        StaticEventHandler.OnDash+= StaticEventHandler_OnRaiseDash;
        Player.Instance.OnApplyDamage += Player_OnApplyDamage;
        UpdateHealthBar();
    }
    
    private void StaticEventHandler_OnRaiseDash(bool isDashing)
    {
        Debug.Log("Dash raised");
        if (!isDashing)
        {
            dashImage.fillAmount = 1;
        }
        else
        {
            dashImage.fillAmount = 0;
        }

    }
    
    private void StaticEventHandler_OnWindMaskUnlocked()
    {
        Debug.Log("Wind Mask unlocked!");
        windMaskContainer.gameObject.SetActive(true);
    }
    private void StaticEventHandler_OnStartTimer(float time)
    {
        dashImage.fillAmount = 1;
        sprintImage.fillAmount = 1;
        timerDuration = time;
        timer = timerDuration;
        Debug.Log("Timer started");
    }
    
    private void StaticEventHandler_OnStartCooldownTimer(float time)
    {
        Debug.Log("Cooldown started");
        cooldownMaskDuration = time;
        cooldownMaskTimer = time;
    }

    private void OnDestroy()
    {
        Player.Instance.OnApplyDamage -= Player_OnApplyDamage;
        StaticEventHandler.OnMaskEquippedTimer -= StaticEventHandler_OnStartTimer;
        StaticEventHandler.OnMaskCooldownTimer -= StaticEventHandler_OnStartCooldownTimer;
        StaticEventHandler.OnWindMaskUnlocked -= StaticEventHandler_OnWindMaskUnlocked;
        StaticEventHandler.OnDash -= StaticEventHandler_OnRaiseDash;
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
            hasMask = true;
            timer -= Time.deltaTime;
            windMaskImage.fillAmount = timer / timerDuration;
        }
        else if(cooldownMaskTimer > 0)
        {
            hasMask = false;
            CooldownMaskLogic(windMaskImage, cooldownMaskDuration);
        }
    }

    private void CooldownMaskLogic(Image imageToMask, float duration)
    {
        imageToMask.fillAmount = 1 - (cooldownMaskTimer / duration);
        cooldownMaskTimer -= Time.deltaTime;
    }
    
    public void DeactivateAbilitiesMask()
    {
        dashImage.fillAmount = 0;
        sprintImage.fillAmount = 0;
    }
    
}
