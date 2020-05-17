using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    [Tooltip("In ms^-1")][SerializeField] float xSpeed = 4f;
    [Tooltip("In m")][SerializeField] float xRange = 5f;
    [Tooltip("In ms^-1")][SerializeField] float ySpeed = 4f;

    [SerializeField] float positionPitchFactor= -5f;
    [SerializeField] float controlPitchFactor = -20f;

    [SerializeField] float positionYawFactor = 5f;

    [SerializeField] float controlRollFactor = -20f;

    float xThrow, yThrow;
  

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        ProcessTranslation();
        ProcessRotation();
    }

    private void ProcessRotation()
    {

        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        float xOffset = xThrow * xSpeed * Time.deltaTime;
        float yOffset = yThrow * ySpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float xPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float rawYPos = transform.localPosition.y + yOffset;
        float yPos = Mathf.Clamp(rawYPos, -2.75f, 3.33f);


        transform.localPosition = new Vector3(xPos, yPos, transform.localPosition.z);
    }
}
