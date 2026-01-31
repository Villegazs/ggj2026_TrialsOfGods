using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(() =>
        {
            //Click
            Loader.Load(Loader.Scene.GameScene);
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        
        Time.timeScale = 1f;
    }
}
