using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Project.Online.Scripts;
using UnityEngine;

namespace Project.Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 15;
        [SerializeField] private float jumpForce = 10;
        [SerializeField] private float attackForce = 10;
        [SerializeField] private Rigidbody2D  rigidBody;
        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject mediumAttack;
        // [SerializeField] private BoxCollider2D attackCollider;
        // [SerializeField] private GameObject attackGameObject;

        private bool isJumping = false;
        private bool canDoubleJump = true;

        private void Awake()
        {
            RoomController.Room.SetPlayers(gameObject);
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                Move();
                photonView.RPC("VerifyJump", RpcTarget.All, gameObject.name, Input.GetButtonDown("Jump"));
                photonView.RPC("VerifyAttack", RpcTarget.All, gameObject.name, Input.GetKeyDown(KeyCode.C));
                // photonView.RPC("VerifySimpleAttack", RpcTarget.All, gameObject.name, Input.GetKeyDown(KeyCode.Z));
            }
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

        [PunRPC]
        private void RPC_ApplyUniquePlayerName(string uniquePlayerNameKey, string playerName)
        {
            if (playerName == gameObject.name)
            {
                gameObject.name = uniquePlayerNameKey;
            }
        }
        
        private void Jump()
        {
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        [PunRPC]
        private void VerifyAttack(string name, bool keyCode)
        {
            if (name == gameObject.name)
            {
                if(keyCode)
                {
                    animator.SetTrigger("specialAttack");

                    Vector3 rotation = transform.eulerAngles.y != 0 ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 180f, 0f);

                    GameObject attack = Instantiate(mediumAttack, transform.position, name.Contains("Biker")? Quaternion.Euler(rotation): transform.rotation);
                    attack.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.forward.z * attackForce, 0);
                }
            }
        }
        
        // [PunRPC]
        // private void VerifySimpleAttack(string name, bool keyCode)
        // {
        //     if (name == gameObject.name)
        //     {
        //         if(keyCode)
        //         {
        //             animator.SetTrigger("simpleAttack");
        //             
        //             attackGameObject.SetActive(true);
        //             StartCoroutine(DeActivateAttackCollider());
        //         }
        //     }
        // }
        //
        // private IEnumerator DeActivateAttackCollider()
        // {
        //     yield return new WaitForSeconds(0.65f);
        //     
        //     attackGameObject.SetActive(false);
        // }
        
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

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     Debug.Log("dadakljdakjdakwjh");
        // }
    }
}
