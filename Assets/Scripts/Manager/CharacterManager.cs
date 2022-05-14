using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock on Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public BoxCollider backStabBoxCollider;
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;

        public int pendingCriticalDamage;


    }
}
