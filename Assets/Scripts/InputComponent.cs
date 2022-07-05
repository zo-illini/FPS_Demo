using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
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
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public bool GetShoot()
    {
        return Input.GetMouseButton(0);
    }

    public bool GetShootSecondary()
    {
        return Input.GetMouseButton(1);
    }

    public bool GetInteract() 
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool GetSwitchWeapon() 
    {
        return Input.mouseScrollDelta != Vector2.zero;
    }

    public bool GetPause() 
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
}
