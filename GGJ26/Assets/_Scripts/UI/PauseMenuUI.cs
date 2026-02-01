using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            Player.Instance.TogglePauseGame();
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start()
    {
        Player.Instance.OnGamePaused += Player_OnGamePaused;
        Player.Instance.OnGameUnpaused += Player_OnGameUnpaused;
        
        Hide();
    }
    
    private void Player_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }
    private void Player_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
