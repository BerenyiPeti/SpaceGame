using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class SelectSignal : MonoBehaviour
{
    public TMP_Text azimuthTMP;
    public TMP_Text latitudeTMP;
    public TMP_Text distanceTMP;

    public float azimuth = 0;
    public float latitude = 0;
    public float distance = 0;

    public float azimuthRange = 360;
    public float latitudeRange = 90;
    public float distanceRange = 99.9f;
    void Start()
    {
        GameObject azTemp = GameObject.FindGameObjectWithTag("TargetAzimuth");
        GameObject latTemp = GameObject.FindGameObjectWithTag("TargetLatitude");
        GameObject disTemp = GameObject.FindGameObjectWithTag("TargetDistance");

        if (azTemp != null && latTemp != null && disTemp != null)
        {
            azimuthTMP = azTemp.GetComponent<TMP_Text>();
            latitudeTMP = latTemp.GetComponent<TMP_Text>();
            distanceTMP = disTemp.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.Log("Couldnt find gameObject");
        }

        azimuth = Random.Range(-azimuthRange, azimuthRange);
        latitude = Random.Range(-latitudeRange, latitudeRange);
        distance = Random.Range(0, distanceRange);
    }
    public void onSignalSelect()
    {
        azimuthTMP.text = azimuth.ToString("F1", CultureInfo.InvariantCulture);
        latitudeTMP.text = latitude.ToString("F1", CultureInfo.InvariantCulture);
        distanceTMP.text = distance.ToString("F1", CultureInfo.InvariantCulture) + " lyr";
    }
}
