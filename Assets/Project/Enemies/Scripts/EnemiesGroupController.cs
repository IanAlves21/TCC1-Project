using System;
using UnityEngine;

namespace Project.Enemies.Scripts
{
    public class EnemiesGroupController : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemies;
        [SerializeField] private GameObject wall;

        private void Update()
        {
            bool result = false;
            
            foreach (var element in enemies)
            {
                result = result || !!element;
            }
            
            if (!result)
            {
                wall.SetActive(false);
                Destroy(this.gameObject,1f);
            }
        }
    }
}
