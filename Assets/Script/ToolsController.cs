using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ToolsController : MonoBehaviour
{
    private bool camDeployed = false;
    private bool micDeployed = false;
    private bool scanDeployed = false;

    private bool camDeploying = false;
    private bool micDeploying = false;
    private bool scanDeploying = false;

    public float deployDistance = 0.7f;
    public float moveDuration = 2f;


    public float toolCd = 5f;

    private float camCd = 0f;
    private float micCd = 0f;
    private float scanCd = 0f;
    public MapController mc;

    public int maxSignals = 3;

    public float successfulPingRate = 0.9f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (scanCd > 0)
        {
            cdCount(ref scanCd);
        }

    }
    private void cdCount(ref float toolCd)
    {
        if (toolCd > 0)
        {
            toolCd -= Time.deltaTime;
        }

    }
    public void onToggle(string tool)
    {
        switch (tool)
        {
            case "cam":
                if (!camDeploying)
                    StartCoroutine(toggleTool(tool));
                break;
            case "mic":
                if (!micDeploying)
                    StartCoroutine(toggleTool(tool));
                break;
            case "scan":
                if (!scanDeploying)
                    StartCoroutine(toggleTool(tool));
                break;
        }
    }

    private IEnumerator toggleTool(string tool)
    {
        string lampTag = "CameraLamp";
        bool state = false;
        GameObject toolObject = GameObject.FindGameObjectWithTag("Camera"); ;

        if (tool == "cam")
        {
            camDeployed = !camDeployed;
            state = camDeployed;
            camDeploying = true;
        }

        if (tool == "mic")
        {
            lampTag = "MicLamp";
            micDeployed = !micDeployed;
            state = micDeployed;
            micDeploying = true;
            toolObject = GameObject.FindGameObjectWithTag("Microphone");
        }

        if (tool == "scan")
        {
            lampTag = "ScanLamp";
            scanDeployed = !scanDeployed;
            state = scanDeployed;
            scanDeploying = true;
            toolObject = GameObject.FindGameObjectWithTag("Scanner");
        }


        GameObject lamp = GameObject.FindGameObjectWithTag(lampTag);
        Image panelImg = lamp.GetComponent<Image>();
        panelImg.color = Color.yellow;

        if (toolObject != null)
            yield return StartCoroutine(MoveTool(toolObject.transform, state));


        if (state)
        {
            panelImg.color = Color.green;
        }
        else
        {
            panelImg.color = Color.red;
        }

        if (tool == "cam") camDeploying = false;
        if (tool == "mic") micDeploying = false;
        if (tool == "scan") scanDeploying = false;


    }

    private IEnumerator MoveTool(Transform toolTransform, bool deploy)
    {
        Vector3 startLocalPos = toolTransform.localPosition;
        Vector3 endLocalPos = startLocalPos + (deploy ? Vector3.up * deployDistance : Vector3.down * deployDistance);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            toolTransform.localPosition = Vector3.Lerp(startLocalPos, endLocalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        toolTransform.localPosition = endLocalPos; // Ensure final position is exact
    }

    public void useScanner()
    {
        if (!scanDeployed || scanDeploying)
        {
            Debug.Log("scanner is not deployed");
            return;
        }

        if (scanCd > 0f)
        {
            Debug.Log("scanner is on cooldown");
            return;
        }

        scanCd = toolCd;

        if (true)
        {
            
        }
        int signalCount = Random.Range(1, maxSignals + 1);
        for (int i = 0; i < signalCount; i++)
        {
            mc.placeSignal();
            Debug.Log("Found " + signalCount + " signal(s)");
        }
    }

    public bool getCamState()
    {
        return camDeployed;
    }

    public bool getMicState()
    {
        return micDeployed;
    }

    public bool getScanState()
    {
        return scanDeployed;
    }
}
