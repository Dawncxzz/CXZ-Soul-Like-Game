using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CXZ
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;
        GameObject instantiatedWarmUpSpellFX;
        GameObject instantiatedSpellFX;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats);
            instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform.position + Vector3.up * 0.001f, animatorHandler.transform.rotation);
            animatorHandler.PlayerTargetAnimation(spellAnimation, true, false);
            Destroy(instantiatedWarmUpSpellFX, instantiatedWarmUpSpellFX.GetComponent<ParticleSystem>().main.duration);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HeadPlayer(healAmount);
            Destroy(instantiatedSpellFX, spellCastFX.GetComponent<ParticleSystem>().main.duration);
        }


    }
}
