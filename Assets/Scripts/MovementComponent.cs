using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovementComponent : NetworkBehaviour
{
    // Start is called before the first frame update

    float m_speed;

    float m_jumpForce;

    public float m_rotationSpeed;

    Rigidbody m_rb;

    CapsuleCollider m_collider;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();

        m_speed = 5;
        m_rotationSpeed = 360;
        m_jumpForce = 200;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleAllMovement(Vector3 movement, Vector3 jumpVector, Vector2 rotation) 
    {
        if (isLocalPlayer) 
        {
            CmdHandleMovementXZ(movement);
            CmdHandleRotation(rotation);
            CmdHandleJump(jumpVector);
        }
    }

    [Command]
    public void CmdHandleMovementXZ(Vector3 movement)
    {
        transform.Translate(movement * m_speed * Time.deltaTime, Space.Self);
    }

    [Command]
    public void CmdHandleJump(Vector3 vector)
    {
        if (IsGrounded())
        {
            m_rb.AddForce(vector * m_jumpForce, ForceMode.Acceleration);
        }
    }

    [Command]
    public void CmdHandleRotation(Vector2 vector)
    {
        transform.Rotate(new Vector3(0, vector.x, 0) * m_rotationSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        Vector3 start = m_collider.transform.TransformPoint(m_collider.center + Vector3.up * m_collider.height);
        Vector3 end = m_collider.transform.TransformPoint(m_collider.center + Vector3.down * 0.5f * m_collider.height);

        return Physics.CheckSphere(end + Vector3.down * 0.1f, m_collider.radius, LayerMask.GetMask("Ground"));
    }
}
