using Photon.Pun;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class SpikesController : MonoBehaviour
    {
        [SerializeField] private float damage = 200f;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("Hurt", RpcTarget.AllBuffered, collision.gameObject.name, damage);                
            }
        }
    }
}
