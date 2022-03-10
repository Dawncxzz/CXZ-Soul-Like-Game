using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Idle Animations")]
        public string right_Hand_Idle;
        public string left_Hand_Idle;
        public string th_idle;

        [Header("Attack Animations")]
        public string on_light_attack_1;
        public string on_light_attack_2;
        public string on_heavy_attack_1;
        public string oh_heavy_attack_2;
        public string th_light_attack_1;
        public string th_light_attack_2;


        [Header("Stamina Drain")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")]
        public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;

    }
}
