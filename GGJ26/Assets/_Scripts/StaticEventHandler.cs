using System;

public static class StaticEventHandler
{
    //-----------------This Is An Example On How To Use It ------------
    // we call the events as StaticEventHandler.RaiseJump();
    // and we recieve it as 
    /*
     *void OnEnable()
    {
        StaticEventHandler.OnJump += PlaySound;
    }

    void OnDisable()
    {
        StaticEventHandler.OnJump -= PlaySound;
    }
     * 
     */
    
    // ---------------- BASIC EVENTS ----------------

    public static event Action OnJump;
    public static event Action OnLand;
    public static event Action OnDeath;
    public static event Action OnWindMaskActivated;
    
    public static event Action OnMaskEquipped;

    // ---------------- PARAMETER EVENTS ----------------

    public static event Action<float> OnHealthChanged;
    public static event Action<int> OnScoreChanged;

    public static event Action<bool> OnPauseChanged;
    
    public static event Action <float> OnMaskEquippedTimer;

    // ---------------- SAFE INVOKERS ----------------

    public static void RaiseJump()
    {
        OnJump?.Invoke();
    }

    public static void RaiseLand()
    {
        OnLand?.Invoke();
    }

    public static void RaiseDeath()
    {
        OnDeath?.Invoke();
    }

    public static void RaiseWindMaskActivated()
    {
        OnWindMaskActivated?.Invoke();
    }

    public static void RaiseHealthChanged(float health)
    {
        OnHealthChanged?.Invoke(health);
    }

    public static void RaiseScoreChanged(int score)
    {
        OnScoreChanged?.Invoke(score);
    }

    public static void RaisePauseChanged(bool paused)
    {
        OnPauseChanged?.Invoke(paused);
    }
    
    public static void RaiseMaskEquipped()
    {
        OnMaskEquipped?.Invoke();
    }
    
    public static void RaiseMaskEquippedTimer(float time)
    {
        OnMaskEquippedTimer?.Invoke(time);
    }
}