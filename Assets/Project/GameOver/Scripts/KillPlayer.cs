using System;
using Photon.Pun;
using UnityEngine;

namespace Project.GameOver.Scripts
{
    public class KillPlayer : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("Hurt", RpcTarget.AllBuffered, collision.gameObject.name, 100000f);
            }
        }
    }
}
