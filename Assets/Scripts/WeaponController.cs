using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour
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

/*
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
*/

    [Command]

    public void CmdShoot(Vector3 forward, int weaponType, GameObject player)
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
                projectile.GetComponent<TriangleProjectile>().m_player = player;
            }
            projectile.tag = m_ownedByPlayer ? "Player Projectile" : "Enemy Projectile";
            projectile.transform.forward = forward;
            projectile.layer = m_ownedByPlayer ? 8 : 10;

            if (m_ownedByPlayer)
            {
                Event_Player_Fire_Projectile evt = new Event_Player_Fire_Projectile();
                evt.m_transform = projectile.transform;
                evt.m_radius = 0.25f;
                evt.m_isSphere = weaponType == 0 ? true : false;
                EventManager.Broadcast(evt);
            }

            NetworkServer.Spawn(projectile);

            projectile.GetComponent<Projectile>().Activate();

            m_shootCooldownTimer = 0;

        }
    }

    bool CanShoot()
    {
        return m_shootCooldownTimer > m_shootCooldown;
    }
}
