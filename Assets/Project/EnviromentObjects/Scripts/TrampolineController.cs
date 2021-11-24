using System;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class TrampolineController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float impulseForce;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                animator.SetTrigger("jump");
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, impulseForce), ForceMode2D.Impulse);
            }
        }
    }
}
