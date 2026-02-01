using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDUI : MonoBehaviour
{
    public HUDUI Instance { get; private set; }
    [SerializeField] private Image healthBar;
    [SerializeField] private Image windMaskImage;
    float timer;
    private float timerDuration;
    
    
    private int actualHealth;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StaticEventHandler.OnMaskEquippedTimer += StaticEventHandler_OnStartTimer;
        Player.Instance.OnApplyDamage += Player_OnApplyDamage;
        UpdateHealthBar();
    }
    private void StaticEventHandler_OnStartTimer(float time)
    {
        timerDuration = time;
        timer = timerDuration;
        Debug.Log("Timer started");
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
    }
    
}
