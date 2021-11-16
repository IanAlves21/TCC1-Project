using System;
using UnityEngine;

namespace Project.Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 15;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private Rigidbody2D  rigidBody;
        [SerializeField] private Animator  animator;

        private bool isJumping = false;
        private bool canDoubleJump = true;

        void Update()
        {
            Move();
            VerifyJump();
        }

        private void Move()
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(horizontalMove, 0f, 0f);
            
            transform.position += movement * Time.deltaTime * speed;

            if (horizontalMove > 0)
            {
                animator.SetBool("run", true);
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            if (horizontalMove < 0)
            {
                animator.SetBool("run", true);
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            if (horizontalMove == 0)
            {
                animator.SetBool("run", false);
            }
        }
        
        private void VerifyJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (isJumping)
                {
                    if (canDoubleJump)
                    {
                        Jump();
                        canDoubleJump = false;
                        animator.SetBool("doubleJump", true);
                    }
                }
                else
                {
                    Jump();
                    animator.SetBool("jump", true);
                }
            }
        }

        private void Jump()
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isJumping = false;
                canDoubleJump = true;
                animator.SetBool("jump", false);
                animator.SetBool("doubleJump", false);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                isJumping = true;
            }
        }
    }
}
