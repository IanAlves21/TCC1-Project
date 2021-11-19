using System;
using UnityEngine;

namespace Project.Attacks.Scripts
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private BoxCollider2D boxCollider;

        [SerializeField] private float damage;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                rigidbody.velocity = Vector2.zero;
                boxCollider.enabled = false;
                animator.SetBool("destroy", true);
                Destroy(this.gameObject, 0.5f);
            }
        }
    }
}
