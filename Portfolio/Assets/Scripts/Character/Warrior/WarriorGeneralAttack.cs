using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorGeneralAttack : MonoBehaviour
{
    public enum AttackType
    {
        Attack1, Attack2, Attack3
    }
    public AttackType CurrentType;
    public bool Hit;
    public void StartAttack1() { Hit = false; CurrentType = AttackType.Attack1; }
    public void StartAttack2() { Hit = false; CurrentType = AttackType.Attack2; }
    public void StartAttack3() { Hit = false; CurrentType = AttackType.Attack3; }
    public void GeneralAttackHit() { Hit = true; }
}
