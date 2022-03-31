using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleProjectile : Projectile
{

    GameObject m_player;

    public float m_flyOutTime;
    public float m_flyBackSpeed;

    float m_flyOutTimer;
    // Start is called before the first frame update
    void Start()
    {
        m_player = FindObjectOfType<GameWorld>().m_player;
        m_flyOutTimer = m_flyOutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active)
        {
            if (m_flyOutTimer > 0)
            {
                transform.position += transform.forward * m_speed * Time.deltaTime;
                m_flyOutTimer -= Time.deltaTime;
            }
            else
            {
                Vector3 playerDir = (m_player.transform.position - transform.position);
                if (playerDir.magnitude < 1.0f)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                transform.position += m_flyBackSpeed * playerDir.normalized * Time.deltaTime;
                }

            }
            m_lifeTime -= Time.deltaTime;
        }

        if (m_lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

    }


}
