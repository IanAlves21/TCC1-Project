using Photon.Pun;
using Project.Attacks.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.EnviromentObjects.Scripts
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private float healthPoint;
        [SerializeField] private Animator animator;

        private void Start()
        {
            this.healthPoint = 100f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            animator.SetTrigger("hit");
            
            if (collision.gameObject.CompareTag("Player"))
            {
                this.healthPoint -= 15;
            }

            VerifyDestruction();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            animator.SetTrigger("hit");

            if (collision.gameObject.CompareTag("Attack"))
            {
                this.healthPoint -= collision.gameObject.GetComponent<ProjectileController>().GetDamage();
            }
            
            VerifyDestruction();
        }

        private void VerifyDestruction()
        {
            if (this.healthPoint <= 0)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                animator.SetBool("break", true);
                Destroy(this.gameObject, 0.25f);
                SpawnItem();
            }
        }

        private void SpawnItem()
        {
            /*
             * 30% of chance on spawn a diamond
             * 20% of chance on spawn a heart vesel
             * 50% of chance on spawn nothing
             */
            
            int randomNumber = (int) Random.Range(1.0f, 10.0f);
            string objectToSpawn = "";
            
            if (randomNumber <= 2)
            {
                objectToSpawn = "CollectibleHeart";
            }
            else
            {
                if (randomNumber <= 5)
                {
                    objectToSpawn = "CollectibleDiamond";
                }
                else
                {
                    objectToSpawn = "";
                }
            }
            
            if(PhotonNetwork.IsMasterClient)
                if(objectToSpawn!="")
                    PhotonNetwork.Instantiate(objectToSpawn, transform.position, Quaternion.identity, 0);
            
            Debug.Log("random number generated ----> " + randomNumber);
        }
    }
}
