using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    public class WaterGunController : MonoBehaviour
    {
        [SerializeField] private float waterGunForce = -800.0f;
        public bool collisionCheck;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.attachedRigidbody.AddForce(new Vector2(waterGunForce, 0));                
            }
        }

        public void DestroyAfterAnimation()
        {
            Destroy(gameObject);
            collisionCheck = true;
        }
    }
}
