using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            //Click
            Loader.Load(Loader.Scene.GameScene);
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
    }

    private void Start()
    {
        StaticEventHandler.OnDeath += StaticEventHandler_OnDeath;
        Hide();
    }

    private void StaticEventHandler_OnDeath()
    {
        Show();
        Debug.Log("Game Over UI");
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
