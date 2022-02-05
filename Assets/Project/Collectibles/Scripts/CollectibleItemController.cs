using System;
using Project.Player.Scripts;
using UnityEngine;

namespace Project.Collectibles.Scripts
{
    public class CollectibleItemController : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D circleCollider;
        [SerializeField] private Animator animator;

        private float lifeValue = 15.0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerController>().IncreasePlayerLife(lifeValue);
                circleCollider.enabled = false;
                animator.SetTrigger("Destroy");
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
