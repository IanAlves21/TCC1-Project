using System;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private GameObject endScreen;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                endScreen.SetActive(true);
            }
        }
    }
}
