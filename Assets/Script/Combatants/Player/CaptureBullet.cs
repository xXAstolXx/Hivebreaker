using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class CaptureBullet : BulletBase
{
    [SerializeField]
    private float boomerangRange = -0.1f;

    private bool isMovingTowardsPlayer = false;

    protected override bool HasCollided(Collider other)
    {
        if (other.GetComponent<Enemy>() != null && other.tag != "Player") return true;
        if (other.GetComponent<Player>() != null && isMovingTowardsPlayer) return true;

        return false;
    }

    protected override void BulletHit(Collider other)
    {
        var enemyCapture = other.GetComponent<EnemyCapture>();
        var player = other.GetComponent<Player>();

        if (enemyCapture != null && other.tag == "Enemy")
        {
            enemyCapture.OnCaptureItemHit();
            DestroyBullet();
        }

        if (player != null && isMovingTowardsPlayer)
        {
            isMovingTowardsPlayer = false;
            direction = Vector3.zero;

            player.IsCaptureBulletReady = true;
            DestroyBullet();
        }
    }

    protected override void MoveBullet()
    {
        transform.position += direction * bulletSpeed * Time.deltaTime;
        
        BoomerangMovement();
    }

    private void BoomerangMovement()
    {
        bulletSpeed += boomerangRange;

        if (bulletSpeed <= 0)
        {
            boomerangRange *= -1;
            isMovingTowardsPlayer = true;
            bulletSpeed = 0.01f;
        }

        Transform player = Game.Instance.PlayerGameObjectRef.GetComponent<Player>().transform;

        if (isMovingTowardsPlayer)
        {
            transform.LookAt(player.position);
            direction.x = transform.forward.x;
            direction.z = transform.forward.z;
        }
    }
}
