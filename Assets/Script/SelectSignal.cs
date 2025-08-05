using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSignal : MonoBehaviour
{
    public TMP_Text azimuthTMP;
    public TMP_Text latitudeTMP;
    public TMP_Text distanceTMP;

    public float azimuth = 0;
    public float latitude = 0;
    public float distance = 0;

    public float azimuthRange = 358;
    public float latitudeRange = 88;
    public float distanceRange = 99.9f;

    private GameObject scGameObject;

    private ShipController sc;

    

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

        azimuth = Random.Range(2, azimuthRange);
        latitude = Random.Range(-latitudeRange, latitudeRange);
        distance = Random.Range(0.1f, distanceRange);

        scGameObject = GameObject.Find("ShipControls");
        sc = scGameObject.GetComponent<ShipController>();
    }
    public void onSignalSelect()
    {
        GameObject[] signals = GameObject.FindGameObjectsWithTag("Signal");
        foreach (var signal in signals)
        {
            Image signalImg = signal.GetComponent<Image>();
            signalImg.color = new Color(1f, 1f, 1f);
        }

        azimuthTMP.text = azimuth.ToString("F1", CultureInfo.InvariantCulture);
        latitudeTMP.text = latitude.ToString("F1", CultureInfo.InvariantCulture);
        distanceTMP.text = distance.ToString("F1", CultureInfo.InvariantCulture) + " lyr";
        Image img = GetComponent<Image>();
        img.color = new Color(1, 150f / 255f, 0f, 1f);

        sc.setTargetAzimuth(azimuth);
        sc.setTargetLatitude(latitude);
        sc.setTargetSelected(true);
    }

    public float getAzimuth()
    {
        return azimuth;
    }

    public float getLatitude()
    {
        return latitude;
    }
}
