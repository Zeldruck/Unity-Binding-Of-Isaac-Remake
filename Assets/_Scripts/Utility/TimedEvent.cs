public class TimedEvent
{
    private bool hasStarted;
    
    private float initialTimer;
    private float timer;

    public delegate void OnTimerEnd();
    private event OnTimerEnd onTimerEnd;

    public TimedEvent(float _timer, OnTimerEnd _onTimerEnd)
    {
        timer = -1f;
        
        initialTimer = _timer;
        onTimerEnd += _onTimerEnd;
    }

    public void ChangeAll(float _timer, OnTimerEnd _onTimerEnd)
    {
        timer = -1f;
        
        initialTimer = _timer;
        onTimerEnd += _onTimerEnd;
    }

    public void ChangeTime(float _timer)
    {
        initialTimer = _timer;
    }

    public void Start()
    {
        timer = initialTimer;
        hasStarted = true;
    }
    
    public void UpdateTimer(float _dt)
    {
        if (!hasStarted || timer <= 0f)
            return;
        
        timer -= _dt;

        if (timer <= 0f)
        {
            onTimerEnd?.Invoke();
            hasStarted = false;
        }
    }
}
