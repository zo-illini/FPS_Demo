using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_speed;

    protected bool m_active = false;

    public float m_lifeTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active)
        {
            transform.position += transform.forward * m_speed * Time.deltaTime;
            m_lifeTime -= Time.deltaTime;
        }

        if (m_lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Activate()
    {
        m_active = true;
    }

    public void SetForward(Vector3 forward)
    {
        transform.rotation = Quaternion.LookRotation(forward);
    }
}
