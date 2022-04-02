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
            obj.transform.forward = forward;
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.tag = m_ownedByPlayer ? "Player Projectile" : "Enemy Projectile";
            projectile.gameObject.layer = m_ownedByPlayer ? 8 : 10;

            projectile.Activate();

            m_shootCooldownTimer = 0;
        }
    }

    public void Shoot(Vector3 forward, int weaponType)
    {
        if (CanShoot())
        {
            GameObject projectile;
            if (weaponType == 0)
            {
                projectile = Instantiate(m_primaryProjectilePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                projectile = Instantiate(m_secondaryProjectilePrefab, transform.position, Quaternion.identity);
            }
            projectile.tag = m_ownedByPlayer ? "Player Projectile" : "Enemy Projectile";
            projectile.transform.forward = forward;
            projectile.layer = m_ownedByPlayer ? 8 : 10;

            projectile.GetComponent<Projectile>().Activate();
        }
    }

    bool CanShoot()
    {
        return m_shootCooldownTimer > m_shootCooldown;
    }
}
