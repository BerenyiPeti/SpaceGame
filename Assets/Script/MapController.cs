using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public RectTransform panel;
    public float xRange = 850f;
    public float yRange = 360f;
    public float deadZoneRadius = 200f;
    
    public void placeSignal()
    {
        GameObject newButton = Instantiate(buttonPrefab, panel);
        RectTransform rt = newButton.GetComponent<RectTransform>();

        Vector2 spawnPos;

        // Try generating a point outside the dead zone
        int attempts = 0;
        do
        {
            float x = Random.Range(-xRange, xRange);
            float y = Random.Range(-yRange, yRange);
            spawnPos = new Vector2(x, y);
            attempts++;
        }
        while (spawnPos.magnitude < deadZoneRadius && attempts < 100);

        rt.anchoredPosition = spawnPos;
        rt.localScale = Vector3.one;

        
    }
}
