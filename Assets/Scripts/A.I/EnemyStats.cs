using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CXZ
{
    public class EnemyStats : CharacterStats
    {

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
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
            currentHealth = currentHealth - damage;

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
            animator.applyRootMotion = true;
            animator.CrossFade("Damage_1", 0.2f);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.CrossFade("Dead_1", 0.2f);
                isDead = true;
            }
        }
    }
}
