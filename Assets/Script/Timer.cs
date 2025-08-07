using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    private MonoBehaviour runner;
    private Coroutine countdownCoroutine;
    private float elapsed;
    private float duration;

    public float Progress => Mathf.Clamp01(elapsed / duration);
    public bool IsRunning => countdownCoroutine != null;

    public Timer(MonoBehaviour runner)
    {
        this.runner = runner;
    }
    public void Start(float seconds, Action action)
    {
        if (IsRunning)
        {
            Stop();
        }

        duration = seconds;
        elapsed = 0;
        countdownCoroutine = runner.StartCoroutine(Countdown(action));
    }

    private IEnumerator Countdown(Action onComplete)
    {
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            /* Debug.Log(progress); */
            yield return null;
        }

        countdownCoroutine = null;

        onComplete?.Invoke();

    }
    public void Stop()
    {
        if (countdownCoroutine != null && runner != null)
        {
            runner.StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }
}
