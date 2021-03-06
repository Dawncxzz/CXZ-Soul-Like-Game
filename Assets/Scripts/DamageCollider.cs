using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(transform.root.tag);

            if (other.tag == "Player" && transform.root.tag != "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if (other.tag == "Enemy" && transform.root.tag != "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}
