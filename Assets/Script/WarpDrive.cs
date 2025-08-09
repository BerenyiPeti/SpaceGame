using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpDrive : Tool
{
    public bool IsCharging = false;
    public bool IsCharged = false;
    public bool IsTraveling = false;
    public WarpDrive(MonoBehaviour runner, float cooldownTime)
        : base(runner, "warpdrive", cooldownTime)
    {

    }

    public override void Use(float useTime, Action action)
    {
        if (IsCharging || IsCharged)
        {
            return;
        }

        IsCharging = true;

        usageTimer.Start(useTime, () =>
        {
            IsCharging = false;
            IsCharged = true;
            action?.Invoke();
        });
    }

    public override void Cancel()
    {
        if (usageTimer.IsRunning)
        {
            usageTimer.Stop();
        }

        IsCharging = false;
        IsCharged = false;
    }

    public void Launch(Action action)
    {
        IsCharging = false;
        IsCharged = false;
        IsTraveling = true;
        action?.Invoke();
    }
}