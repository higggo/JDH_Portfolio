using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenSkillDamage : BulletDamage
{

    // Start is called before the first frame update
    void Start()
    {
        InitComponent();
    }

    protected override void SetDamage()
    {
        
        PlayHitEffect();
    }


}
