using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ToolsController : MonoBehaviour
{
    private bool signalsLocated = false;
    private Tool scanner;
    private WarpDrive warpDrive;
    public float wdChargeTime = 10f;
    public float toolCd = 5f;
    public MapController mc;
    public ShipController sc;
    public ScreenUI scui;
    public int maxSignals = 3;
    public float successfulScanRate = 0.9f;
    public bool wdInitiated = false;
    public float minScanWait = 2f;
    public float maxScanWait = 5f;
    public bool wdReady = false;
    
    void Start()
    {
        scanner = new Tool(this, "scanner", "Scanner", "ScanLamp", "ScanLamp", 0.7f, 2f, 5f);
        warpDrive = new WarpDrive(this, 60f);
    }
    void Update()
    {
        scanner.UpdateCooldown();

        if (warpDrive.IsCharging || warpDrive.IsCharged)
        {
            if (!sc.targetLocked)
            {
                scui.displayMessage("warp drive canceled:", "target is not locked");
                scui.CancelLoading();
                sc.showCoordinateResults();
                warpDrive.Cancel();
                return;
            }
        }

    }
    public void onToggle(string tool)
    {
        switch (tool)
        {
            /* case "cam":
                if (!camDeploying)
                    StartCoroutine(toggleTool(tool));
                break;
            case "mic":
                if (!micDeploying)
                    StartCoroutine(toggleTool(tool));
                break; */
            case "scan":
                if (!scanner.IsDeploying)
                {
                    if (scanner.IsBusy)
                    {
                        scui.displayMessage("tool error:", "cannot move tool while in use");
                        return;
                    }
                    scanner.Toggle();
                }
                break;

        }
    }

    public void useWarpdrive()
    {
        if (warpDrive.IsCharging)
        {
            return;
        }

        sc.showCoordinateResults();

        if (!sc.targetLocked)
        {
            scui.displayMessage("warp drive error:", "target is not locked");
            return;
        }

        if (warpDrive.IsCharged)
        {
            warpDrive.Launch(launchWarpdrive);
            return;
        }

        InitiateWarpDrive();

    }

    private void InitiateWarpDrive()
    {
        scui.displayMessage("attention:", "initiating warp drive");
        scui.displayWarpdrive(wdChargeTime);
        warpDrive.Use(wdChargeTime, () => { scui.warpdriveMessage.text = "warp drive ready"; });
    }
    private void launchWarpdrive()
    {
        Debug.Log("launching... at least will be launching");
    }

    public void cancelWarpdrive()
    {
        if (!warpDrive.IsCharging && !warpDrive.IsCharged)
        {
            scui.displayMessage("error:", "warp drive is not active");
            return;
        }

        scui.displayMessage("warp drive canceled:", "warp drive aborted by user");
        scui.CancelLoading();
        warpDrive.Cancel();
    }
    public void useScanner()
    {
        if (!scanner.IsReady)
        {
            scui.displayMessage("scanner error", scanner.GetToolState());
            return;
        }

        float waitTime = Random.Range(minScanWait, maxScanWait + 1);
        float successfulScanValue = Random.value;

        scui.displayMessage("", "searching for signals...");

        if (successfulScanValue > successfulScanRate)
        {
            scanner.Use(waitTime, scanFailed);
        }
        else
        {
            scanner.Use(waitTime, scanSuccessful);
        }
    }

    private void scanSuccessful()
    {
        if (signalsLocated)
        {
            scui.displayMessage("scan complete:", "no new signals found");
            return;
        }

        int signalCount = Random.Range(1, maxSignals + 1);
        for (int i = 0; i < signalCount; i++)
        {
            mc.placeSignal();
        }

        sc.setInitialRotation();
        scui.displayMessage("scan complete:", $"{signalCount} signal(s) was found");
        signalsLocated = true;
    }

    private void scanFailed()
    {
        scui.displayMessage("error:", "failed to locate signals. Try again");
    }

}
