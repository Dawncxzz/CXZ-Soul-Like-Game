using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;
        Collider collider;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
            collider = GetComponent<Collider>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            playerManager.OpenChestIneraction(playerStandingPosition);
            animator.Play("Chest Open");
            StartCoroutine(SpawnItemInChest());

            WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

            if (weaponPickUp != null)
            {
                weaponPickUp.weapon = itemInChest;
            }
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1);
            Instantiate(itemSpawner, transform);
            collider.enabled = false;
            Destroy(openChest);
        }
    }
}
