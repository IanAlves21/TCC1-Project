using System;
using Photon.Pun;
using Project.Player.Scripts;
using UnityEngine;

namespace Project.Collectibles.Scripts
{
    public class CollectibleItemController : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D circleCollider;
        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;

        private float lifeValue = 15.0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                photonView.RPC("DestroyCollectibleItem", RpcTarget.All);
                collision.gameObject.GetComponent<PhotonView>().RPC("IncreasePlayerLife", RpcTarget.All, lifeValue, collision.gameObject.name);
            }
        }
        
        [PunRPC]
        public void DestroyCollectibleItem()
        {
            circleCollider.enabled = false;
            animator.SetTrigger("Destroy");
            Destroy(gameObject, 0.5f);
        }

    }
}
