using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleSystem
{
    void Damaged(GameObject target, float damage);
    bool IsDead();
}
