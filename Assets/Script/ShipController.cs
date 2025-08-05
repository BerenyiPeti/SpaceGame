using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{

    public TMP_Text yawDisplay;
    public TMP_Text pitchDisplay;
    public TMP_Text rollDisplay;
    public TMP_Text offsetValue;
    public TMP_Text azimuthTmp;
    public TMP_Text latitudeTmp;
    public GameObject cockpit;
    public float yawSpeed;
    public float pitchSpeed;
    public float rollSpeed;

    private float azimuth;
    private float latitude;

    public float maxValue = 200;
    public float minValue = -200;
    private Quaternion initialRotation;
    public bool yawActive = false;
    public bool pitchActive = false;
    public bool rollActive = false;

    public float changingValue;

    public GameObject[] axisButtons;

    public float targetAzimuth = 0;
    public float targetLatitude = 0;

    public bool targetLocked = false;

    public float targetTreshold = 0.5f;

    public bool targetSelected = false;
    void Start()
    {
        initialRotation = cockpit.transform.rotation;
    }

    void Update()
    {
        cockpit.transform.Rotate(pitchSpeed * Time.deltaTime, yawSpeed * Time.deltaTime, rollSpeed * Time.deltaTime);

        setStartRotation();

        if (yawActive)
        {
            changeValue(ref yawSpeed, yawDisplay);
        }

        if (pitchActive)
        {
            changeValue(ref pitchSpeed, pitchDisplay);
        }

        if (rollActive)
        {
            changeValue(ref rollSpeed, rollDisplay);
        }

        checkTargetValues();


    }

    private void checkTargetValues()
    {
        bool azimuthValid = false;
        bool latitudeValid = false;
        targetLocked = false;

        if (Math.Abs(targetAzimuth - azimuth) <= targetTreshold)
        {
            azimuthValid = true;
        }

        if (Math.Abs(targetLatitude - latitude) <= targetTreshold)
        {
            latitudeValid = true;
        }

        if (azimuthValid && latitudeValid)
        {
            targetLocked = true;
            Debug.Log("Target Locked");
        }
    }
    public void onButtonDown(float value)
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
        string pressedButtonTag = pressedButton.tag;

        foreach (var button in axisButtons)
        {
            button.GetComponentInChildren<TMP_Text>().color = Color.white;
        }

        bool isSameYaw = yawActive && pressedButtonTag == "Yaw" && changingValue == value;
        bool isSamePitch = pitchActive && pressedButtonTag == "Pitch" && changingValue == value;
        bool isSameRoll = rollActive && pressedButtonTag == "Roll" && changingValue == value;

        if (isSameYaw || isSamePitch || isSameRoll)
        {
            yawActive = false;
            pitchActive = false;
            rollActive = false;

            yawDisplay.color = Color.white;
            pitchDisplay.color = Color.white;
            rollDisplay.color = Color.white;

            return;
        }

        changingValue = value;
        yawDisplay.color = Color.white;
        pitchDisplay.color = Color.white;
        rollDisplay.color = Color.white;

        pressedButton.GetComponentInChildren<TMP_Text>().color = Color.green;

        if (pressedButtonTag == "Yaw")
        {
            yawActive = true;
            pitchActive = false;
            rollActive = false;
            yawDisplay.color = Color.green;
        }

        if (pressedButtonTag == "Pitch")
        {
            yawActive = false;
            pitchActive = true;
            rollActive = false;
            pitchDisplay.color = Color.green;
        }

        if (pressedButtonTag == "Roll")
        {
            yawActive = false;
            pitchActive = false;
            rollActive = true;
            rollDisplay.color = Color.green;
        }
    }

    public void resetButtons()
    {
        foreach (var button in axisButtons)
        {
            button.GetComponentInChildren<TMP_Text>().color = Color.white;
        }

        yawSpeed = 0;
        pitchSpeed = 0;
        rollSpeed = 0;

        yawDisplay.text = "0";
        pitchDisplay.text = "0";
        rollDisplay.text = "0";

        yawDisplay.color = Color.white;
        pitchDisplay.color = Color.white;
        rollDisplay.color = Color.white;

        yawActive = false;
        pitchActive = false;
        rollActive = false;


    }

    public void changeValue(ref float type, TMP_Text field)
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            if ((type + changingValue) <= maxValue)
            {
                type += changingValue;

            }
        }

        if (scroll < 0f)
        {
            if ((type - changingValue) >= minValue)
            {
                type -= changingValue;

            }
        }

        field.text = type.ToString();
    }

    public void setStartRotation()
    {
        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * cockpit.transform.rotation;
        Vector3 euler = relativeRotation.eulerAngles;

        // Normalize for display
        azimuth = euler.y;
        latitude = NormalizeAngle(euler.x);
        float offset = NormalizeAngle(euler.z);

        // Clamp pitch if needed
        latitude = Mathf.Clamp(latitude, -90f, 90f);

        // Display
        if (targetSelected)
        {
            azimuthTmp.text = azimuth.ToString("F1", CultureInfo.InvariantCulture);
            latitudeTmp.text = latitude.ToString("F1", CultureInfo.InvariantCulture);

        }
        offsetValue.text = offset.ToString("F1", CultureInfo.InvariantCulture);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }

    public void setTargetAzimuth(float value)
    {
        targetAzimuth = value;
    }

    public void setTargetLatitude(float value)
    {
        targetLatitude = value;
    }

    public void setTargetSelected(bool value)
    {
        targetSelected = true;
    }

}
