using System;
using Photon.Pun;
using Project.Player.Scripts;
using UnityEngine;

namespace Project.Enemies.Scripts
{
    public class ComumEnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;
        // [SerializeField] private Transform headPoint;
        [SerializeField] private float speed;
        [SerializeField] private float way = -1f;

        [SerializeField] private float life = 500f;
        [SerializeField] private float damage = 10f;

        [SerializeField] private GameObject elementToNextPart;

        void Update()
        {
            Vector3 movement = new Vector3(1f, 0f, 0f);
            transform.position += movement * Time.deltaTime * speed*way;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("Hurt", RpcTarget.AllBuffered, collision.gameObject.name, damage);
                
                // float height = collision.contacts[0].point.y - headPoint.position.y;

                // if (height > 0)
                // {
                    // collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                    // speed = 0;
                    // animator.SetTrigger("Hurt");
                    // Destroy(gameObject, 0.33f);
                // }
            }
            Debug.Log("colidiu com ---> " + collision.gameObject.name);

            // if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
            // {
                way *= -1;
                transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            // }
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
                this.animator.SetBool("die", true);
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
