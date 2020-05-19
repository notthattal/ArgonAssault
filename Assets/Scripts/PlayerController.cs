using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [Tooltip("In ms^-1")][SerializeField] float controlSpeed = 4f;
    [Tooltip("In m")][SerializeField] float xRange = 5f;
    [Tooltip("In ms^-1")][SerializeField] float ySpeed = 4f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    [SerializeField] float positionPitchFactor= -5f;
    [SerializeField] float positionYawFactor = 5f;

    [Header("Control-throw Based")]
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float controlRollFactor = -20f;

    float xThrow, yThrow;
    bool isControlEnabled = true;


    // Update is called once per frame
    void Update()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
    }

    void OnPlayerDeath() //called by string reference
    {
        isControlEnabled = false;
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
        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float yOffset = yThrow * ySpeed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float xPos = Mathf.Clamp(rawXPos, -xRange, xRange);
        float rawYPos = transform.localPosition.y + yOffset;
        float yPos = Mathf.Clamp(rawYPos, -2.75f, 3.33f);


        transform.localPosition = new Vector3(xPos, yPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            var bulletEmissions = gun.GetComponent<ParticleSystem>().emission;
            bulletEmissions.enabled = isActive;
        }
    }

}
