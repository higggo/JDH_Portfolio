using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class BasicAttack_Warrior : BasicAttack
{
    Animator animator;
    Coroutine ResetCombo;
    int currentCombo = 1;
    int maxCombo = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("Combo", currentCombo);
    }

    public override void OnAttack(GameObject Target)
    {
        base.OnAttack(Target);
        if (attackClip != null)
            SoundManager.I.PlayEffectSound(gameObject, attackClip);

        if (ResetCombo != null)
            StopCoroutine(ResetCombo);
        
        switch (statInfo.grade)
        {
            case GRADE.NORMAL:
                ComboReset();
                break;
            case GRADE.MAGIC:
                if (currentCombo < 2)
                    currentCombo++;
                else
                    ComboReset();
                break;
            case GRADE.RARE:
                if (currentCombo < 3)
                    currentCombo++;
                else
                    ComboReset();
                break;
            case GRADE.UNIQUE:
                if (currentCombo < 3)
                    currentCombo++;
                else
                    ComboReset();
                break;
        }
        
        animator.SetInteger("Combo", currentCombo);
        ResetCombo = StartCoroutine(ComboResetTimer(2.0f));
    }

    void ComboReset()
    {
        currentCombo = 1;
        animator.SetInteger("Combo", currentCombo);
    }

    IEnumerator ComboResetTimer(float time)
    {
        while (currentCombo < maxCombo + 1 && time > Mathf.Epsilon)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        ComboReset();
        Debug.Log("end Timer");
    }
}
