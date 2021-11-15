using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

// INHERITANCE
public class Countdown
{
    protected UnityEvent OnZeroCountdown { get; } = new UnityEvent();

    int m_countdown = 3;

    protected virtual void DoCountdown()
    {
        m_countdown--;
        if (m_countdown <= 0)
        {
            OnZeroCountdown.Invoke();
        }
    }
}
