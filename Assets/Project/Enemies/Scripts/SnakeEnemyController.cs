using System;
using Photon.Pun;
using UnityEngine;

namespace Project.Enemies.Scripts
{
    public class SnakeEnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private float speed;
        [SerializeField] private float life = 500f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private GameObject elementToNextPart;

        private Vector3 movement = new Vector3(0,0,0);

        void Update()
        {
            transform.position += movement * Time.deltaTime * speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("Hurt", RpcTarget.AllBuffered, collision.gameObject.name, damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                int direction = 1;
                animator.SetBool("Attack", true);
                movement = collision.gameObject.transform.position - transform.position;
                movement = movement / movement.magnitude;
                
                if (movement.x < 0)
                    direction = -1;
                
                transform.localScale = new Vector2(Math.Abs(transform.localScale.x)*direction, transform.localScale.y);    
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                animator.SetBool("Attack", false);
                movement = new Vector3(0,0,0);
            }
        }

        [PunRPC]
        public void Hurt(string name, float damage)
        {
            if (name == gameObject.name)
            {
                animator.SetTrigger("Hurt");
                
                this.life -= damage;

                if (life <= 0)
                {
                    photonView.RPC("Die", RpcTarget.All, gameObject.name);
                }
            }
        }

        [PunRPC]
        public void Die(string name)
        {
            if (name == gameObject.name)
            {
                speed = 0.0f;
                animator.SetBool("die", true);
                if(!!elementToNextPart)
                    elementToNextPart.SetActive(true);
                Destroy(gameObject, 0.5f);
            }
        }
        
        [PunRPC]
        private void RPC_ApplyUniquePlayerName(string uniqueObjectNameKey, string objectName)
        {
            if (objectName == gameObject.name)
            {
                gameObject.name = uniqueObjectNameKey;
            }
        }
    }
}
