using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CXZ
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            PlayerAnimatorManager animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

            Debug.Log("¼ñÆðÎäÆ÷");
            animatorHandler.PlayerTargetAnimation("Pick Up Item", true, false);
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
            playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            StartCoroutine(SetItemInteractableGameObject(playerManager));
            gameObject.GetComponent<ParticleSystem>().Stop();
            if (transform.parent.GetComponentInParent<Collider>() != null)
            {
                transform.parent.GetComponentInParent<Collider>().enabled = true;
            }
        }

        IEnumerator SetItemInteractableGameObject(PlayerManager playerManager)
        {
            yield return new WaitForSeconds(2f);
            playerManager.itemInteractableGameObject.SetActive(false);
            playerManager.interactableUIGameobject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
