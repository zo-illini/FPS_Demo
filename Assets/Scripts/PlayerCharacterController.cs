using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacterController : NetworkBehaviour
{

    MovementComponent m_movement;
    InputComponent m_input;

    WeaponController m_weapon;

    Camera m_camera;

    float m_cameraSpeedY;

    float m_curCameraAngleX;

    Health m_health;

    int m_currentWeaponID;

    // Start is called before the first frame update
    void Start()
    {   
        m_movement = GetComponent<MovementComponent>();
        m_input = GetComponent<InputComponent>();
        m_camera = GetComponentInChildren<Camera>();
        m_weapon = GetComponent<WeaponController>();


        m_cameraSpeedY = 360;
        m_curCameraAngleX = 0;

        m_health = GetComponent<Health>();
        m_health.SetOnDeath(OnPlayerDie);

        m_camera.enabled = isLocalPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        //m_movement.CmdHandleMovementXZ(m_input.GetAxisXZ());
        //m_movement.CmdHandleJump(m_input.GetAxisY());

        Vector2 rotation = m_input.GetRotationDelta();
        //m_movement.CmdHandleRotation(rotation);
        m_movement.HandleAllMovement(m_input.GetAxisXZ(), m_input.GetAxisY(), rotation);


        // Rotate and Clamp Camera on X rotation
        m_curCameraAngleX -= rotation.y * m_cameraSpeedY * Time.deltaTime;
        m_curCameraAngleX = Mathf.Clamp(m_curCameraAngleX, -60, 60);
        m_camera.transform.localEulerAngles = new Vector3(m_curCameraAngleX, 0, 0);

        if (m_input.GetShoot())
        {
            m_weapon.Shoot(m_camera.transform.forward, m_currentWeaponID);
        }

        if (m_input.GetSwitchWeapon()) 
        {
            m_currentWeaponID = m_currentWeaponID == 0 ? 1 : 0;
        }

        //if (m_input.GetShootSecondary())
        //{
        //    m_weapon.Shoot(m_camera.transform.forward, 1);
        //}

        if (m_input.GetInteract()) 
        {
            EventManager.Broadcast(Events.EventPlayerInteract);
        }
    }

    public void TakeDamage(float damage)
    {
        m_health.TakeDamage(damage);
    }

    void OnPlayerDie()
    {
        EventManager.Broadcast(Events.EventPlayerDie);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            TakeDamage(20);
        }
    }

    [Command]
    public void CmdInitializePosition(Vector3 pos, Quaternion rot) 
    {
        transform.position = pos;
        transform.rotation = rot;
    }

}
