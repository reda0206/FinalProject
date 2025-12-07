using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Idle, Shoot, Chase }
public class EnemyAI : MonoBehaviour
{
    public float health = 40f;
    public Transform player;
    public GameObject lazer;
    public EnemyState state = EnemyState.Idle;
    public float attackCooldown = 3f;
    private float lastAttackTime = -Mathf.Infinity;
    public float spawnOffset = 1.5f;

    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            var playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }


    void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
                if (Vector3.Distance(player.position, transform.position) < 30f)
                {
                    state = EnemyState.Shoot;
                }
                    break;

            case EnemyState.Shoot:
                Attack();
                if (Vector3.Distance(player.position, transform.position) > 60f)
                {
                    state = EnemyState.Idle;
                }
                else if (Vector3.Distance(player.position, transform.position) < 15f)
                {
                    state = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                Attack();
                agent.SetDestination(player.position);
                if (Vector3.Distance(player.position, transform.position) > 80f)
                {
                    state = EnemyState.Shoot;
                }
                    break;
            default:
                Debug.LogError(gameObject.name + " attempted to access an EnemyAI state that does not exist.");
                break;
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            Vector3 direction = (player.position - transform.position).normalized;
            Instantiate(lazer, transform.position + direction, Quaternion.LookRotation(direction));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lazer"))
        {
            health -= 5f;
            state = EnemyState.Chase;
            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
