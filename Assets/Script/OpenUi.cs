using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUi : MonoBehaviour
{
    public GameObject ControlPanel;
    public GameObject MapPanel;
    private GameObject clickedObject = null;
    private bool UiActive = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // On mouse down, remember what we clicked on
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && Cursor.visible && !UiActive)
            {
                if (hit.collider.CompareTag("ControlPanel"))
                {
                    clickedObject = hit.collider.gameObject;
                    UiActive = true;
                }

                if (hit.collider.CompareTag("MapPanel"))
                {
                    clickedObject = hit.collider.gameObject;
                    UiActive = true;
                }
            }
        }

        // On mouse up, check if we're releasing on the same object
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && Cursor.visible)
            {
                if (hit.collider.gameObject == clickedObject)
                {
                    if (hit.collider.CompareTag("ControlPanel"))
                    {
                        ControlPanel.SetActive(true);
                        Debug.Log(hit.collider.tag);
                    }
                }

                if (hit.collider.gameObject == clickedObject)
                {
                    if (hit.collider.CompareTag("MapPanel"))
                    {
                        MapPanel.SetActive(true);
                    }
                }

            }

            // Reset clickedObject
            clickedObject = null;
        }

    }

    public void closeUi(GameObject ui)
    {
        ui.SetActive(false);
        UiActive = false;
    }
}
