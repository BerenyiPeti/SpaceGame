using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    public TMP_Text time;
    public TMP_Text MainFeedback;
    public TMP_Text FeedbackType;

    public TMP_Text warpdriveMessage;

    public Transform loadingBar;

    public TMP_Text countdown;
    private Coroutine messageCoroutineInstance;
    public float messageDisplayTime = 3f;
    public GameObject wdui;
    private Image[] panels;

    private Coroutine loadingCoroutine;

    void Start()
    {
        fillPanels();


    }


    // Update is called once per frame
    void Update()
    {
        showTime();

    }
    private void fillPanels()
    {
        Image parentImage = loadingBar.GetComponent<Image>();

        panels = loadingBar.GetComponentsInChildren<Image>()
            .Where(img => img != parentImage)
            .ToArray();

    }

    public void displayMessage(string messageType, string message)
    {
        if (messageCoroutineInstance != null)
        {
            StopCoroutine(messageCoroutineInstance);
        }

        messageCoroutineInstance = StartCoroutine(messageCoroutine(message, messageType));
    }


    private IEnumerator messageCoroutine(string message, string messageType)
    {
        MainFeedback.text = message;
        FeedbackType.text = messageType;
        yield return new WaitForSeconds(messageDisplayTime);
        MainFeedback.text = "";
        FeedbackType.text = "";
        messageCoroutineInstance = null;
    }
    void showTime()
    {
        DateTime future = DateTime.Now.AddYears(100);
        string dateString = future.ToString("yyyy-MM-dd HH:mm:ss");
        time.text = dateString;
    }

    public void displayWarpdrive(float seconds)
    {
        wdui.SetActive(true);
        warpdriveMessage.text = "warp drive charging";
        loadingCoroutine = StartCoroutine(FillBar(seconds));
    }

    IEnumerator FillBar(float delay)
    {
        float fillDelay = delay / (panels.Length - 1);
        foreach (Image panel in panels)
        {
            Color color = panel.color;
            color.a = 1f;
            panel.color = color;
            yield return new WaitForSeconds(fillDelay);
        }
    }

    public void CancelLoading()
    {
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingCoroutine = null;
        }

        resetWarpdriveDisplay();
    }
    public void resetWarpdriveDisplay()
    {
        resetLoadingBar();
        wdui.SetActive(false);
    }

    private void resetLoadingBar()
    {
        foreach (Image panel in panels)
        {
            Color color = panel.color;
            color.a = 0f;
            panel.color = color;
        }
    }
}
