using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject m_primaryProjectilePrefab;

    public GameObject m_secondaryProjectilePrefab;


    public float m_shootCooldown;
    float m_shootCooldownTimer;

    public bool m_ownedByPlayer;

    void Start()
    {
        m_shootCooldownTimer = 0;
    }

    void Update()
    {
        if (m_shootCooldownTimer < m_shootCooldown)
            m_shootCooldownTimer += Time.deltaTime;
    }

    public void Shoot(Vector3 forward)
    {
        if (CanShoot())
        {
            GameObject obj = Instantiate(m_primaryProjectilePrefab, transform.position, Quaternion.identity);
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.SetForward(forward);
            projectile.tag = m_ownedByPlayer ? "Player Projectile" : "Enemy Projectile";
            projectile.Activate();

            m_shootCooldownTimer = 0;
        }
    }

    public void Shoot(Vector3 forward, int weaponType)
    {
        if (weaponType == 0)
        {
            Shoot(forward);
        }
        else
        {
            if (CanShoot())
            {
                GameObject obj = Instantiate(m_secondaryProjectilePrefab, transform.position, Quaternion.identity);
                Projectile projectile = obj.GetComponent<Projectile>();
                projectile.SetForward(forward);
                projectile.tag = m_ownedByPlayer ? "Player Projectile" : "Enemy Projectile";
                projectile.Activate();

                m_shootCooldownTimer = 0;
            }
        }
    }

    bool CanShoot()
    {
        return m_shootCooldownTimer > m_shootCooldown;
    }
}
