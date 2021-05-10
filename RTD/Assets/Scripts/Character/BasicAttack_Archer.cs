using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack_Archer : BasicAttack
{
    [SerializeField] Transform bulletStartPos;
    [SerializeField] GameObject ArrowInMesh;
    
    private void Start()
    {
        Init();

        if (ArrowInMesh != null)
            GetComponentInChildren<AnimEvent>().AttackEndDel += () => { ArrowInMesh.GetComponent<MeshRenderer>().enabled = true; };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnAttack(GameObject target)
    {
        if (target == null)
            return;

        if (ArrowInMesh != null)
            ArrowInMesh.GetComponent<MeshRenderer>().enabled = false;

        if (attackClip != null)
            SoundManager.I.PlayEffectSound(gameObject, attackClip);

        ProjectileManager manager = GetComponent<ProjectileManager>();
        manager.FireProjectile(bulletStartPos.position, gameObject, target, statInfo.attackDamage);
    }

    
}
