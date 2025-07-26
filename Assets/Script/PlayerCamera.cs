using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform PlayerCam;
    public int position = 0;
    public float rotationY = 0;
    public float rotationSpeed = 100;

    public bool turning = false;

    void Start()
    {
        /* rotationY = PlayerCam.transform.rotation.y; */
    }

    void Update()
    {
        if (turning)
        {
            TurnCamera();
        }
    }

    void TurnCamera()
    {
        /* Debug.Log("Turning"); */
        Quaternion start = PlayerCam.transform.rotation;
        Quaternion end = Quaternion.Euler(PlayerCam.transform.rotation.x, rotationY, PlayerCam.transform.rotation.z);

        PlayerCam.transform.rotation = Quaternion.Lerp(start, end, Time.deltaTime * rotationSpeed);
        if (PlayerCam.transform.rotation.y == rotationY)
        {
            turning = false;

        }
    }

    public void Turn()
    {
        turning = true;
    }
    public void TurnLeft()
    {
        if (position == 0)
        {
            rotationY = -90;
            position = -1;
            Turn();
        }
        else if (position == 1)
        {
            rotationY = 0;
            position = 0;
            Turn();
        }
    }

    public void TurnRight()
    {
        if (position == 0)
        {
            rotationY = 90;
            position = 1;
            Turn();
        }
        else if (position == -1)
        {
            rotationY = 0;
            position = 0;
            Turn();
        }
    }

    public void TurnBack()
    {
        if (position < 2)
        {
            rotationY = 180;
            position = 2;
            Turn();
        }
        else
        {
            rotationY = 0;
            position = 0;
            Turn();
        }
    }
}
