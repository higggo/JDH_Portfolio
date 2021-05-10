using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamage_Lich : EffectDamageTick
{
    int id = (int)CharacterKit.ID.Q_LICH + 1000;

    [Header("Lich Skill After Effect")]
    [SerializeField] float DebuffMoveSpeed;
    [SerializeField] float DebuffDuration;

    protected override void AddToList(Collider other)
    {
        base.AddToList(other);
        if (enemyLayer == other.gameObject.layer)
        {
            CharacterKit.BuffSkill skill = new CharacterKit.BuffSkill(CharacterKit.BUFFCATEGORY.RATIO, id, 0, 0, DebuffMoveSpeed, DebuffDuration);
            other.gameObject.GetComponent<CharacterStat>().AddBuff(skill);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        AddToList(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromList(other);
    }
}
