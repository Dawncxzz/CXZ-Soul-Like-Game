using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CXZ
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        HealthBar healthBar;
        StaminaBar staminaBar;

        PlayerAnimatorManager animHandler;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenerateTimer = 0;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();  
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }
        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

        }
        private int SetMaxHealthFromHealthLevel()
        { 
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        
        private float SetMaxStaminaFromStaminaLevel()
        { 
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            animHandler.PlayerTargetAnimation("Damage_1", true, false);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animHandler.PlayerTargetAnimation("Dead_1", true, false);
                isDead = true;
            }
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

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerateTimer = 0;
            }    
            else
            {
                staminaRegenerateTimer += Time.deltaTime;
                if (currentStamina < maxStamina && staminaRegenerateTimer > 0.5)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HeadPlayer(int healAmount)
        {
            currentHealth += healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }
    }
}
