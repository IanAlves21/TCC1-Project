using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.EnviromentObjects.Scripts
{
    public class FallingPlatformController : MonoBehaviour
    {
        [SerializeField] private TargetJoint2D targetJoint2D;
        [SerializeField] private BoxCollider2D boxCollider2D;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(FallPlatform());
            }
        }

        private IEnumerator FallPlatform()
        {
            yield return new WaitForSeconds(2);

            targetJoint2D.enabled = false;
            boxCollider2D.enabled = false;
            
            Destroy(this.gameObject, 5f);
        }
    }
}
