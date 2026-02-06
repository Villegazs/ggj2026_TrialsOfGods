using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private bool canSkip = false;
    private float timer = 2f;
    

    private void Start()
    {
        StaticEventHandler.OnWindMaskUnlocked += StaticEventHandler_OnWindMaskUnlocked;
        Hide();
    }

    IEnumerator CanSkipTutorial()
    {
        yield return new WaitForSeconds(timer);
        canSkip = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && canSkip)
        {
            Hide();
        }
    }

    private void StaticEventHandler_OnWindMaskUnlocked()
    {
        Show();
        StartCoroutine(CanSkipTutorial());
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
