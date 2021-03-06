﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    public float cameraMoveSpeed;

    // A vector3 that camera towards to the player position. Standard view
    private Vector3 cameraPlayerVector;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cameraPlayerVector = PlayerController.instance.transform.position - transform.position;
        transform.position = PlayerController.instance.transform.position - cameraPlayerVector;
    }

    void Update()
    {
        // Back to the first view
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = PlayerController.instance.transform.position - cameraPlayerVector;
        }

        CameraMoveByCursor();
        ZoomCameraByScrollWheel();
    }

    void LateUpdate()
    {
        //transform.position = PlayerController.instance.transform.position - cameraPlayerVector;
    }

    /// <summary>
    /// Zoom in or out the camera
    /// </summary>
    private void ZoomCameraByScrollWheel()
    {
        float min = PlayerController.instance.transform.position.y + 2;
        float max = min + 10;
        int directionFlag = Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : 0;
        directionFlag = Input.GetAxis("Mouse ScrollWheel") < 0 ? -1 : directionFlag;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        Debug.DrawRay(transform.position, Input.mousePosition - transform.position, Color.red, 3f, false);

        if (transform.position.y >= min && transform.position.y <= max)
        {
            float zoomDistance = cameraMoveSpeed * directionFlag * Time.deltaTime;
            transform.Translate(ray.direction * zoomDistance, Space.World);
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(transform.position.y, min, max);
            transform.position = pos;
        }

    }

    /// <summary>
    /// Force the camera move if the cursor out of the game screen
    /// </summary>
    private void CameraMoveByCursor()
    {
        if (Input.mousePosition.y >= Screen.height)
        {
            transform.Translate(Vector3.forward * cameraMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.mousePosition.y <= 0)
        {
            transform.Translate(Vector3.back * cameraMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.mousePosition.x <= 0)
        {
            transform.Translate(Vector3.left * cameraMoveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.mousePosition.x >= Screen.width)
        {
            transform.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.World);
        }
    }

}
