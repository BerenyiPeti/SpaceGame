using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Tool
{
    public string name;
    public string lampObjectTag;
    public string lampTag;

    public float cooldownTime;
    public float deployDistance;
    public float deployDuration;

    private MonoBehaviour runner;
    private GameObject toolObject;
    private GameObject lampObject;
    private Image lampImage;
    private Timer usageTimer;

    public bool IsDeployed { get; private set; } = false;
    public bool IsDeploying { get; private set; } = false;
    public bool IsBusy { get; private set; } = false;
    public float Cooldown { get; private set; } = 0f;

    public bool IsReady => IsDeployed && !IsDeploying && !IsBusy && Cooldown <= 0;
    public bool IsRetracting => IsDeploying && IsDeployed;
    public float usageProgress => usageTimer.IsRunning ? usageTimer.Progress : 0f;
    public Tool(MonoBehaviour runner, string name, string toolObjectTag, string lampObjectTag, string lampTag, float deployDistance, float deployDuration, float cooldownTime)
    {
        this.runner = runner;
        usageTimer = new Timer(runner);
        this.name = name;
        this.lampObjectTag = lampObjectTag;
        this.lampTag = lampTag;
        this.deployDistance = deployDistance;
        this.deployDuration = deployDuration;
        this.cooldownTime = cooldownTime;

        toolObject = GameObject.FindGameObjectWithTag(toolObjectTag);
        if (toolObject == null) { Debug.LogWarning("toolObject not found"); return; }

        lampObject = GameObject.FindGameObjectWithTag(lampTag);
        if (lampObject == null) { Debug.LogWarning("lampObject not found"); return; }

        lampImage = lampObject.GetComponent<Image>();
        if (lampImage == null) { Debug.LogWarning("lampImage not found"); return; }

    }

    public void UpdateCooldown()
    {
        if (Cooldown > 0f) Cooldown = Mathf.Max(0, Cooldown - Time.deltaTime);
    }

    public void Toggle()
    {
        if (IsBusy) { return; }

        runner.StartCoroutine(DeployRoutine());
    }

    private IEnumerator DeployRoutine()
    {
        IsDeploying = true;
        lampImage.color = Color.yellow;

        Vector3 startPos = toolObject.transform.localPosition;
        Vector3 endPos = startPos + (IsDeployed ? Vector3.down : Vector3.up) * deployDistance;

        float elapsed = 0f;
        while (elapsed < deployDuration)
        {
            toolObject.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / deployDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        IsDeployed = !IsDeployed;
        IsDeploying = false;
        toolObject.transform.localPosition = endPos;
        lampImage.color = IsDeployed ? Color.green : Color.red;
    }

    public void Use(float useTime, Action action)
    {
        if (!IsReady) { return; }

        if (usageTimer.IsRunning)
        {
            usageTimer.Stop();
            IsBusy = false;
        }

        IsBusy = true;

        usageTimer.Start(useTime, () =>
        {
            IsBusy = false;
            TriggerCooldown();
            action?.Invoke();
        });
    }

    public void Cancel()
    {
        if (!usageTimer.IsRunning) { Debug.Log("tool not in use"); return; }

        usageTimer.Stop();
        IsBusy = false;
    }


    public string GetToolState()
    {
        if (IsRetracting) { return $"{name} is retracting"; }
        if (IsDeploying) { return $"{name} is deploying"; }
        if (!IsDeployed) { return $"{name} is not deployed"; }
        if (IsBusy) { return $"{name} is in use"; }
        if (Cooldown > 0) { return $"{name} is on cooldown"; }
        return null;
    }

    public void TriggerCooldown()
    {
        Cooldown = cooldownTime;
    }
}
