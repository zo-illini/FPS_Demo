using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

    MovementComponent m_movement;
    InputComponent m_input;

    WeaponController m_weapon;

    Camera m_camera;

    float m_cameraSpeedY;

    float m_curCameraAngleX;

    // Start is called before the first frame update
    void Start()
    {   
        m_movement = GetComponent<MovementComponent>();
        m_input = GetComponent<InputComponent>();
        m_camera = GetComponentInChildren<Camera>();
        m_weapon = GetComponent<WeaponController>();


        m_cameraSpeedY = 360;
        m_curCameraAngleX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_movement.HandleMovementXZ(m_input.GetAxisXZ());
        m_movement.HandleJump(m_input.GetAxisY());

        Vector2 rotation = m_input.GetRotationDelta();
        m_movement.HandleRotation(rotation);

        // Rotate and Clamp Camera on X rotation
        m_curCameraAngleX -= rotation.y * m_cameraSpeedY * Time.deltaTime;
        m_curCameraAngleX = Mathf.Clamp(m_curCameraAngleX, -60, 60);
        m_camera.transform.localEulerAngles = new Vector3(m_curCameraAngleX, 0, 0);

        if (m_input.GetShoot())
        {
            m_weapon.Shoot(m_camera.transform.forward);
        }
    }
}
