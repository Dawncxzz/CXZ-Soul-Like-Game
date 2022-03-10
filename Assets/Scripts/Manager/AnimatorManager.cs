using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        public void PlayerTargetAnimation(string targtAnim, bool isInteracting, bool canMove)
        {
            if (!canMove)
                GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", false);
            anim.SetBool("isInteracting", isInteracting);
            //anim.Play(targtAnim);
            anim.CrossFade(targtAnim, 0.2f);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        { 
            
        }
    }
}
