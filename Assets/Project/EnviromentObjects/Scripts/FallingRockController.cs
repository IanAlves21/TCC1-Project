using System;
using System.Collections;
using Photon.Pun;
using Project.Player.Scripts;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class FallingRockController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        
        private Vector3 startingPosition;
        private bool fall;
        
        void Start()
        {
            startingPosition = transform.position;
            fall = true;
        }

        private void Update()
        {
            if (fall)
            {
                Fall();
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }

        private void ReturnToOriginalPosition()
        {
            if (transform.position.y < startingPosition.y)
            {
                transform.position =
                    new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            }
            else
            {
                StartCoroutine(ChangeFallValue(true));
            }
        }
        
        private void Fall()
        {
            // if (transform.position.y > -3.7f)
            // {
            //     transform.position =
            //         new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
            // }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                StartCoroutine(ChangeFallValue(false));
            }
            
            if (collision.gameObject.CompareTag("Player"))
            {
                this.fall = false;
                collision.gameObject.GetComponent<PhotonView>().RPC("Die", RpcTarget.AllBuffered, collision.gameObject.name);
            }
        }

        private IEnumerator ChangeFallValue(bool value)
        {
            yield return new WaitForSeconds(3);
            rigidbody2D.simulated = value;
            this.fall = value;
        }
    }
}
