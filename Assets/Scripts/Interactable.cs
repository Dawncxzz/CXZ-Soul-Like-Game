using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactbleText;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("和一个物体互动");
        }
    }
}
