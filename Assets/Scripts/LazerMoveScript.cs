using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerMoveScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public Vector3 direction = Vector3.zero;

    void Update()
    {
        Vector3 moveDirection = (direction.sqrMagnitude > 0.0001f) ? direction.normalized : transform.forward;
        transform.position += moveDirection * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }

