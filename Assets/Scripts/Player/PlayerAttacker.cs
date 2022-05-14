using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorManager animatorHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        PlayerWeaponSlotManager weaponSlotManager;
        public string lastAttack;

        LayerMask backStabLayer;
        LayerMask riposteLayer;


        private void Awake()
        {
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            backStabLayer = 1 << LayerMask.NameToLayer("BackStab");
            riposteLayer = 1 << LayerMask.NameToLayer("Riposte");
        }

        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weaponItem.on_light_attack_1)
                {
                    animatorHandler.PlayerTargetAnimation(weaponItem.on_light_attack_2, true, false);
                }
                else if (lastAttack == weaponItem.th_light_attack_1)
                {
                    animatorHandler.PlayerTargetAnimation(weaponItem.th_light_attack_2, true, false);
                }
                else if (lastAttack == weaponItem.on_heavy_attack_1)
                {
                    animatorHandler.PlayerTargetAnimation(weaponItem.oh_heavy_attack_2, true, false);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weaponItem)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weaponItem;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayerTargetAnimation(weaponItem.th_light_attack_1, true, false);
                lastAttack = weaponItem.th_light_attack_1;
            }
            else
            {
                animatorHandler.PlayerTargetAnimation(weaponItem.on_light_attack_1, true, false);
                lastAttack = weaponItem.on_light_attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weaponItem)
        {
            if (playerStats.currentStamina <= 0)
                return;

            animatorHandler.PlayerTargetAnimation(weaponItem.on_heavy_attack_1, true, false);

            if (inputHandler.twoHandFlag)
            {
                weaponSlotManager.attackingWeapon = weaponItem;
                lastAttack = weaponItem.on_heavy_attack_1;
            }
            else
            {
                weaponSlotManager.attackingWeapon = weaponItem;
                lastAttack = weaponItem.on_heavy_attack_1;
            }
        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }
        #endregion

        public void HandleRTAction()
        {
            if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                PerformRTMeleeAction();
            }
            else if (playerInventory.leftWeapon.isSpellCaster || playerInventory.leftWeapon.isFaithCaster || playerInventory.leftWeapon.isPyroCaster)
            {
                PerformRTMagicAction(playerInventory.leftWeapon);
            }
        }

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }
        
        private void PerformRTMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.leftWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;
                animatorHandler.anim.SetBool("isUsingLeftHand", true);
                HandleLightAttack(playerInventory.leftWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weaponItem)
        {
            if (playerManager.isInteracting)
                return;

            if (weaponItem.isFaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        animatorHandler.anim.SetBool("isUsingRightHand", true);
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                    }
                    else
                    {
                        animatorHandler.PlayerTargetAnimation("Shrug", true, false);
                    }
                }
            }
        }
        
        private void PerformRTMagicAction(WeaponItem weaponItem)
        {
            if (playerManager.isInteracting)
                return;

            if (weaponItem.isFaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        animatorHandler.anim.SetBool("isUsingLeftHand", true);
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                    }
                    else
                    {
                        animatorHandler.PlayerTargetAnimation("Shrug", true, false);
                    }
                }
            }
        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }
        #endregion

        public void AttemptBackStabOrRiposte()
        {
            if (playerManager.isInteracting)
                return;
            if (playerStats.currentStamina <= 0)
                return;
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                RaycastHit hit;

                if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                    transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
                {
                    CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                    DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                    if (enemyCharacterManager != null)
                    {
                        playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;

                        Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                        rotationDirection = hit.transform.position - playerManager.transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                        playerManager.transform.rotation = targetRotation;

                        int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                        enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                        animatorHandler.PlayerTargetAnimation("Back Stab", true, false);
                        enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayerTargetAnimation("Back Stabbed", true, false);
                    }
                }
                else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                    transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
                {
                    CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                    DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                    if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                    {
                        playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

                        Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                        rotationDirection = hit.transform.position - playerManager.transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();
                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                        playerManager.transform.rotation = targetRotation;

                        int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                        enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                        animatorHandler.PlayerTargetAnimation("Riposte", true, false);
                        enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayerTargetAnimation("Risposted0", true, false);
                    }
                   
                }
            }
        }
    }
}
