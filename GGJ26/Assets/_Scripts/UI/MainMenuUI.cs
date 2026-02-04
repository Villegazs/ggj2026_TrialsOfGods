using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartGameButton.onClick.AddListener(() =>
        {
            //Click
            Loader.Load(Loader.Scene.TestingScene);
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        
        Time.timeScale = 1f;
    }
}
