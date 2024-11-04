using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    public GameObject blade;
    public GameObject blade2;
    public float extendSpeed = 0.1f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip powerOnSound;
    public AudioClip powerOffSound;
    public AudioClip themeMusic;

    private bool weaponActive = true;
    private bool isDoubleBladed = false;
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

            extendDelta = weaponActive ? -Mathf.Abs(extendDelta) : Mathf.Abs(extendDelta);
        }

        scaleCurrent += extendDelta * Time.deltaTime;
        scaleCurrent = Mathf.Clamp(scaleCurrent, scaleMin, scaleMax);
        blade.transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);
        blade.transform.localPosition = new Vector3(0, scaleCurrent / 2, 0);

        weaponActive = scaleCurrent > 0;
        blade.SetActive(weaponActive);

        if (!weaponActive)
        {
            blade2.SetActive(false);
        }
    }

    void HandleDoubleBladeToggle()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isDoubleBladed = !isDoubleBladed;
            blade2.SetActive(isDoubleBladed && weaponActive);

            if (isDoubleBladed)
            {
                blade2.transform.localPosition = new Vector3(0, -1.5f / 2, 0);
                blade2.transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);

                audioSource.clip = themeMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
