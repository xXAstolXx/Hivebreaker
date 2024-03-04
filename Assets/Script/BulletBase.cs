using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletBase : MonoBehaviour
{
    [SerializeField]
    protected float minBulletSpeed = 3.0f;

    [HideInInspector]
    public float bulletSpeed;

    [HideInInspector]
    public Vector3 direction = Vector3.zero;

    protected void Start()
    {
        CheckForMinBulletSpeed();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (HasCollided(other))
        {
            BulletHit(other);
            DestroyBullet();
        }
    }

    protected void Update()
    {
        MoveBullet();
    }

    protected void CheckForMinBulletSpeed()
    {
        bulletSpeed = (bulletSpeed < minBulletSpeed) ? minBulletSpeed : bulletSpeed;
    }

    protected virtual void MoveBullet()
    {
        transform.position += direction * bulletSpeed * Time.deltaTime;
    }

    protected virtual void DestroyBullet()
    {
        Destroy(gameObject, 0.1f);
    }
    
    protected virtual bool HasCollided(Collider other)
    {
        if (other.GetComponent<Fence>() != null) return false;
        if (other.GetComponent<HitBox>() != null) return false;
        if (other.GetComponent<StaticDamageArea>() != null) return false;
        if (other.GetComponent<SaveArea>() != null) return false;
        if (other.GetComponent <Teleporter>() != null) return false;
        if (other.GetComponent<BulletBase>() != null) return false;
        if (other.gameObject.layer == LayerMask.GetMask("MouseRaycast")) return false;
        if (other.tag == gameObject.tag) return false;

        return true;
    }

    protected virtual void BulletHit(Collider other)
    {

    }
}
        
