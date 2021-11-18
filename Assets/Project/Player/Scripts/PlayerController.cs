using System;
using Photon.Pun;
using UnityEngine;

namespace Project.Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 15;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private Rigidbody2D  rigidBody;
        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;

        private bool isJumping = false;
        private bool canDoubleJump = true;

        void Update()
        {
            if (photonView.IsMine)
            {
                photonView.RPC("Move", RpcTarget.All, gameObject.name, Time.deltaTime, Input.GetAxis("Horizontal"));
                photonView.RPC("VerifyJump", RpcTarget.All, gameObject.name, Input.GetButtonDown("Jump"));
            }
        }

        [PunRPC]
        private void Move(string name, float deltaTime, float horizontalMove)
        {
            if (gameObject.name == name)
            {
                Vector3 movement = new Vector3(horizontalMove, 0f, 0f);
                
                transform.position += movement * deltaTime * speed;

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
        }
        
        [PunRPC]
        private void VerifyJump(string name, bool buttonDownJump)
        {
            if (gameObject.name == name)
            {
                if (buttonDownJump)
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
        
        [PunRPC]
        private void RPC_ApplyUniquePlayerName(string uniquePlayerNameKey, string playerName)
        {
            Debug.Log("aaaaaaaaaaaaaaaaaa" + gameObject.name);
            if (playerName == gameObject.name)
            {
                Debug.Log("aqui ---------------------> " + uniquePlayerNameKey);
                gameObject.name = uniquePlayerNameKey;
            }
        }
    }
}
