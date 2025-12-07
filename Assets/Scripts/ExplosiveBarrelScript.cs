using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelScript : MonoBehaviour
{
    public float explosionRadius;
    public int Damage;
    public GameObject explosionEffect;
    public float chainExplosionDelay = 0.1f;

    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lazer"))
        {
            TriggerExplode(0f);
        }
    }

    private void TriggerExplode(float delay = 0f)
    {
        if (hasExploded)
            return;

        hasExploded = true;
        StartCoroutine(Explode(delay));
    }

    private IEnumerator Explode(float delay)
    { 
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var collider in targets)
        {
            if (collider == null)
                continue;

            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.health -= Damage;
                if (enemy.health <= 0f)
                {
                    Destroy(enemy.gameObject);
                }
            }

            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.health -= Damage;
                if (player.health <= 0f)
                {
                    Destroy(player.gameObject);
                }
            }

            ExplosiveBarrelScript otherBarrel = collider.GetComponent<ExplosiveBarrelScript>();
            if (otherBarrel != null && otherBarrel != this)
            {
                otherBarrel.TriggerExplode(chainExplosionDelay);
            }

            DestructibleWallScript wall = collider.GetComponent<DestructibleWallScript>();
            if (wall != null)
            {
                wall.health -= Damage;
                if (wall.health <= 0f)
                {
                    Destroy(wall.gameObject);
                }
            }
        }

        Destroy(gameObject);
    }
}
