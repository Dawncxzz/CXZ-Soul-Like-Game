using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    public class AmbushState : State
    {
        public bool isSleeping;

        public float detectionRadius = 2;

        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (isSleeping && enemyManager.isInteracting == false)
            {
                enemyAnimatorManager.PlayerTargetAnimation(sleepAnimation, true, false);
            }

            #region Handle Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);
            Debug.DrawLine(enemyManager.transform.position, enemyManager.transform.position + enemyManager.transform.forward * detectionRadius, Color.yellow);
            Debug.Log(colliders.Length);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if (viewableAngle >= enemyManager.minimumDetectionAngle
                        && viewableAngle <= enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        enemyAnimatorManager.PlayerTargetAnimation(wakeAnimation, true, false);
                    }
                }
            }
            #endregion

            #region Handle State Change
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}
