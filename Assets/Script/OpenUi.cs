using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OpenUi : MonoBehaviour
{
    public GameObject ControlPanel;
    public GameObject MapPanel;
    public GameObject ToolPanel;
    private GameObject clickedObject = null;
    private bool UiActive = false;

    public ToolsController tc;


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

                }

                if (hit.collider.CompareTag("MapPanel"))
                {
                    clickedObject = hit.collider.gameObject;
                }

                if (hit.collider.CompareTag("ToolPanel"))
                {
                    clickedObject = hit.collider.gameObject;
                }

                if (hit.collider.CompareTag("WDBtn"))
                {
                    clickedObject = hit.collider.gameObject;
                }

                if (hit.collider.CompareTag("WDCancel"))
                {
                    clickedObject = hit.collider.gameObject;
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
                        UiActive = true;
                    }
                }

                if (hit.collider.gameObject == clickedObject)
                {
                    if (hit.collider.CompareTag("MapPanel"))
                    {
                        MapPanel.SetActive(true);
                        UiActive = true;
                    }
                }

                if (hit.collider.gameObject == clickedObject)
                {
                    if (hit.collider.CompareTag("ToolPanel"))
                    {
                        ToolPanel.SetActive(true);
                        UiActive = true;
                    }
                }

                if (hit.collider.gameObject == clickedObject)
                {
                    if (hit.collider.CompareTag("WDBtn"))
                    {
                        tc.useWarpdrive();
                    }
                }

                if (hit.collider.gameObject == clickedObject)
                {
                    if (hit.collider.CompareTag("WDCancel"))
                    {
                        tc.cancelWarpdrive();
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
