using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    Vector2 m_preRotation;


    // Start is called before the first frame update
    void Start()
    {
        m_preRotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetAxis()
    {
        return (GetAxisXZ() + GetAxisY()).normalized;
    }

    public Vector3 GetAxisXZ()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    public Vector3 GetAxisY()
    {
        return Input.GetKeyDown("space") ? Vector3.up  : Vector3.zero;
    }

    public Vector2 GetRotationDelta()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) - m_preRotation;
    }

    public bool GetShoot()
    {
        return Input.GetMouseButton(0);
    }

    public bool GetShootSecondary()
    {
        return Input.GetMouseButton(1);
    }
}
