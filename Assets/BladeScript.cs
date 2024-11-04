using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    public GameObject blade; // Reference to the first blade GameObject
    public GameObject blade2; // Reference to the second blade GameObject (for double blade)
    public float extendSpeed = 0.1f;

    [Header("Audio Settings")]
    public AudioSource audioSource;  // Reference to AudioSource component
    public AudioClip powerOnSound;   // Power on sound effect
    public AudioClip powerOffSound;  // Power off sound effect
    public AudioClip themeMusic;      // Darth Vader theme music

    private bool weaponActive = true;
    private bool isDoubleBladed = false; // Track whether it's double-bladed
    private float scaleMin = 0f;
    private float scaleMax;
    private float extendDelta;
    private float scaleCurrent;
    private float localScaleX;
    private float localScaleZ;

    void Start()
    {
        localScaleX = blade.transform.localScale.x;
        localScaleZ = blade.transform.localScale.z;
        scaleMax = blade.transform.localScale.y;
        scaleCurrent = scaleMax;
        extendDelta = scaleMax / extendSpeed;
        weaponActive = true;

        // Initially set the second blade to inactive
        blade2.SetActive(false);
    }

    void Update()
    {
        HandleBladeToggle();
        HandleDoubleBladeToggle();
    }

    void HandleBladeToggle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (weaponActive)
            {
                audioSource.PlayOneShot(powerOffSound);
            }
            else
            {
                audioSource.PlayOneShot(powerOnSound);
            }

            // Reverse extendDelta to toggle extend/retract
            extendDelta = weaponActive ? -Mathf.Abs(extendDelta) : Mathf.Abs(extendDelta);
        }

        // Update blade extension/retraction
        scaleCurrent += extendDelta * Time.deltaTime;
        scaleCurrent = Mathf.Clamp(scaleCurrent, scaleMin, scaleMax);
        blade.transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);
        blade.transform.localPosition = new Vector3(0, scaleCurrent / 2, 0);

        // Set active state of the blade
        weaponActive = scaleCurrent > 0;
        blade.SetActive(weaponActive);

        // If the blade is off, hide the second blade too
        if (!weaponActive)
        {
            blade2.SetActive(false);
        }
    }

    void HandleDoubleBladeToggle()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isDoubleBladed = !isDoubleBladed; // Toggle double blade state
            blade2.SetActive(isDoubleBladed && weaponActive); // Show or hide the second blade based on state

            if (isDoubleBladed)
            {
                // Position the second blade at the opposite end of the hilt
                blade2.transform.localPosition = new Vector3(0, -1.5f / 2, 0);
                blade2.transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);

                // Play the theme music
                audioSource.clip = themeMusic;
                audioSource.loop = true; // Set the audio to loop
                audioSource.Play(); // Start playing the theme music
            }
            else
            {
                // Stop the theme music
                audioSource.Stop(); // Stop playing the theme music
            }
        }
    }
}
