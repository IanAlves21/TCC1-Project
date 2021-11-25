using System;
using UnityEngine;

namespace Project.Enemies.Scripts
{
    public class ComumEnemyController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform rightCol;
        [SerializeField] private Transform leftCol;
        [SerializeField] private Transform headPoint;

        [SerializeField] private float speed;
        private bool colliding;

        [SerializeField] private LayerMask layer;
        
        void Start()
        {
        
        }

        void Update()
        {
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

            colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

            if (colliding)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
                speed *= -1f;
            }
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
        }
    }
}
