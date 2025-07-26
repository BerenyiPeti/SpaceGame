using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnHoldTest : MonoBehaviour
{
    private bool isHeld = false;
    private int value = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(value);

        if (isHeld)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                value += 1;
            }

            if (scroll < 0f)
            {
                value -= 1;
            }
        }
    }
    
    public void OnPointerDown()
    {
        isHeld = true;
    }

    public void OnPointerUp()
    {
        isHeld = false;
    }
}
