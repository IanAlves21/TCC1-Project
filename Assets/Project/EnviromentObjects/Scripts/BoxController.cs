using System;
using Project.Attacks.Scripts;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private float healthPoint;
        [SerializeField] private Animator animator;

        private void Start()
        {
            this.healthPoint = 100f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            animator.SetTrigger("hit");
            
            if (collision.gameObject.CompareTag("Player"))
            {
                this.healthPoint -= 15;
            }

            VerifyDestruction();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            animator.SetTrigger("hit");

            if (collision.gameObject.CompareTag("Attack"))
            {
                this.healthPoint -= collision.gameObject.GetComponent<ProjectileController>().GetDamage();
            }
            
            VerifyDestruction();
        }

        private void VerifyDestruction()
        {
            if (this.healthPoint <= 0)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("break", true);
                Destroy(this.gameObject, 0.25f);
            }
        }
    }
}
