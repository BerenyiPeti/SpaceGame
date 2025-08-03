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
    public TMP_Text azimuthValue;
    public TMP_Text latitudeValue;
    public GameObject cockpit;
    public float yawSpeed;
    public float pitchSpeed;
    public float rollSpeed;

    public float maxValue = 200;
    public float minValue = -200;

    public float startAzimuth; //y

    public float startLatitude; //x

    private Quaternion initialRotation;
    public bool yawActive = false;
    public bool pitchActive = false;
    public bool rollActive = false;

    public float changingValue;

    public GameObject[] axisButtons;
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
        /* Vector3 euler = cockpit.transform.rotation.eulerAngles;

        // Azimuth (yaw) = rotation around Y axis
        startAzimuth = euler.y;

        // Latitude (pitch) = rotation around X axis
        startLatitude = NormalizeAngle(euler.x);

        // Clamp latitude to -90 to 90
        startLatitude = Mathf.Clamp(startLatitude, -90f, 90f);
        azimuthValue.text = startAzimuth.ToString("F1", CultureInfo.InvariantCulture);
        latitudeValue.text = startLatitude.ToString("F1", CultureInfo.InvariantCulture);
        offsetValue.text = NormalizeAngle(euler.z).ToString("F1", CultureInfo.InvariantCulture); */

        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * cockpit.transform.rotation;
        Vector3 euler = relativeRotation.eulerAngles;

        // Normalize for display
        float azimuth = euler.y;
        float latitude = NormalizeAngle(euler.x);
        float offset = NormalizeAngle(euler.z);

        // Clamp pitch if needed
        latitude = Mathf.Clamp(latitude, -90f, 90f);

        // Display
        azimuthValue.text = azimuth.ToString("F1", CultureInfo.InvariantCulture);
        latitudeValue.text = latitude.ToString("F1", CultureInfo.InvariantCulture);
        offsetValue.text = offset.ToString("F1", CultureInfo.InvariantCulture);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;
        return angle;
    }

    void changeDisplay(TMP_Text value, float speed)
    {
        value.text = speed.ToString();
    }
}
