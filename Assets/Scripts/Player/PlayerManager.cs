using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class PlayerManager : CharacterManager
    {
        PlayerStats playerStats;
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerLocomotion playerLocomotion;

        InteractableUI interactableUI;
        public GameObject interactableUIGameobject; 
        public GameObject itemInteractableGameObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSpriting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isInvulnerable;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            anim = GetComponentInChildren<Animator>();
            playerStats = GetComponent<PlayerStats>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        private void StateCheck()
        {
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            anim.SetBool("isInAir", isInAir);
            anim.SetBool("isDead", playerStats.isDead);
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            StateCheck();

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = anim.GetBool("canRotate");
            //Debug.Log("4 : " + GetComponent<Rigidbody>().velocity);
            playerLocomotion.HandleRollingAndSprinting(delta);
            //Debug.Log("5 : " + GetComponent<Rigidbody>().velocity);
            playerLocomotion.HandleJumping();
            //Debug.Log("6 : " + GetComponent<Rigidbody>().velocity);
            

            playerStats.RegenerateStamina();

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            StateCheck();

            //Debug.Log("1 : " + GetComponent<Rigidbody>().velocity);
            playerLocomotion.HandleFalling(delta);
            //Debug.Log("2 : " + GetComponent<Rigidbody>().velocity);
            playerLocomotion.HandleMovement(delta);
            //Debug.Log("3 : " + GetComponent<Rigidbody>().velocity);
            playerLocomotion.HandleRotation(delta);

        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        #region Player Interactions

        public void CheckForInteractableObject()
        { 
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactbleObject = hit.collider.GetComponent<Interactable>();

                    if (interactbleObject != null)
                    {
                        string interactbleText = interactbleObject.interactbleText;
                        interactableUI.interactableText.text = interactbleText;
                        interactableUIGameobject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameobject != null)
                {
                    interactableUIGameobject.SetActive(false);
                }

                //if (itemInteractableGameObject != null && inputHandler.a_Input)
                //{ 
                //    itemInteractableGameObject.SetActive(false);
                //}
            }

        }

        public void OpenChestIneraction(Transform playerStandsHereWhenOpeningChest)
        {
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayerTargetAnimation("Open Chest", true, false);
        }

        #endregion
    }
}
