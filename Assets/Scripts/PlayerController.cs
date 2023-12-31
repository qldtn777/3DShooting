using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] float controlSpeed = 10;
    [SerializeField] float rotationSpeed = 2500;
    [SerializeField] float clampXPos = 3;
    [SerializeField] float clampYMinPos = 3f;
    [SerializeField] float clampYMaxPos = 0;
     float roll;
     float pitch;
     float yaw;
     float h;
     float v;

    [SerializeField] GameObject[] lasers;
    
    private void OnEnable()
    {
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();

        RotatePlayer();

        FireLaser();
    }

    private void FireLaser()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
        }
        else
        {
            SetLaserActive(false);
        }
    }

    private void SetLaserActive(bool isActive)
    {
        foreach(var laser in lasers)
        {
            var particle = laser.GetComponent<ParticleSystem>().emission;
            particle.enabled = isActive;
        }
    }

    private void RotatePlayer()
    {
        roll = -h * Time.deltaTime * rotationSpeed;
        pitch = -v * Time.deltaTime * rotationSpeed;
        yaw = h * Time.deltaTime * rotationSpeed;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

    }

    private void MovePlayer()
    {
        //float hnewInputSystem = movement.ReadValue<Vector2>().x;
        //float vnewInputSystem = movement.ReadValue<Vector2>().y;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float xOffset = h * controlSpeed * Time.deltaTime;
        float newXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(newXPos, -clampXPos, clampXPos);


        float yOffset = v * controlSpeed * Time.deltaTime;
        float newYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(newYPos, clampYMaxPos, clampYMinPos);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, 0);
    }
}
