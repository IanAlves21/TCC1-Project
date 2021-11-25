using System;
using UnityEngine;

namespace Project.Enemies.Scripts
{
    public class ComumEnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform headPoint;
        [SerializeField] private float speed;
        [SerializeField] private float way = -1f;
        [SerializeField] private LayerMask layer;

        void Update()
        {
            Vector3 movement = new Vector3(1f, 0f, 0f);
            transform.position += movement * Time.deltaTime * speed*way;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                float height = collision.contacts[0].point.y - headPoint.position.y;

                if (height > 0)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    speed = 0;
                    animator.SetTrigger("Hurt");
                    Destroy(gameObject, 0.33f);
                }
            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                way *= -1;
                transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            }
        }
    }
}
