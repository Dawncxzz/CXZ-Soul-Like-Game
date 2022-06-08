using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CXZ
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;

        public int soulsAwardedOnDeath = 50;

        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            Debug.Log(currentHealth);
            Debug.Log(damage);
            currentHealth = currentHealth - damage;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            enemyAnimatorManager.PlayerTargetAnimation("Damage_1", true, false);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayerTargetAnimation("Dead_1", true, false);
            isDead = true;
            
        }
    }
}
