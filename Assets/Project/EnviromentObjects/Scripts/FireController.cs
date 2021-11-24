using System;
using Photon.Pun;
using Project.Player.Scripts;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class FireController : MonoBehaviour
    {
        [SerializeField] private float damage = 7f;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("Hurt", RpcTarget.AllBuffered, collision.gameObject.name, damage);                
            }
        }
    }
}
