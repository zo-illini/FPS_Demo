using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject m_projectilePrefab;

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
            GameObject obj = Instantiate(m_projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectile = obj.GetComponent<Projectile>();
            projectile.SetSpeed(50);
            projectile.SetForward(forward);
            projectile.tag = m_ownedByPlayer ? "Player Projectile" : "Enemy Projectile";
            projectile.m_lifeTime = 3.0f;

            projectile.Activate();

            m_shootCooldownTimer = 0;
        }
        
    }

    bool CanShoot()
    {
        return m_shootCooldownTimer > m_shootCooldown;
    }
}
