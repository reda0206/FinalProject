using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelScript : MonoBehaviour
{
    public float explosionRadius;
    public int Damage;
    public GameObject explosionEffect;

    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lazer"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;

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
                otherBarrel.Explode();
            }
        }

        Destroy(gameObject);
    }
}
