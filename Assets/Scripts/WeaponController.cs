using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject m_projectilePrefab;

    float m_shootCooldown;
    float m_shootCooldownTimer;

    void Start()
    {
        m_shootCooldown = 0.5f;
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
            projectile.Activate();
            m_shootCooldownTimer = 0;
        }
        
    }

    bool CanShoot()
    {
        return m_shootCooldownTimer > m_shootCooldown;
    }
}
