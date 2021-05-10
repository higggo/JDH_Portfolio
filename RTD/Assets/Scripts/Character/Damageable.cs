using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharacterKit 
{
    public struct FDamageMessage
    {
        public GameObject Causer;
        public float amount;
    }

    public struct FTickDamageMessage
    {
        public GameObject causer;
        public float amount;
        public float amountTime;
        public float tickTime;
    }

    /*
     * @Summary: 해당 클래스는 CharacterStat컴포넌트와 같은 하이어아키에 존재해야 합니다.
     */
    public class Damageable : MonoBehaviour
    {
        public UnityAction onDeadDel = null;
        public Transform HitPoint = null;
        float HP;
        bool isDead = false;

        public bool IsDead 
        {
            get { return isDead; } 
        }

        void Start()
        {
            HP = GetComponent<CharacterStat>().HP;
            GetComponent<CharacterStat>().OnHPChangeDel += () => { HP = GetComponent<CharacterStat>().HP; };
        }


        // msg의 amount는 양의 값을 전달해야 합니다.
        public void GetDamage(FDamageMessage msg)
        {
            GetModifiedMessage(ref msg);

            if (msg.amount < 1)
                msg.amount = 1;

            // Set Damage Direct
            HP = Mathf.Clamp(HP - msg.amount, 0.0f, GetComponent<CharacterStat>().MaxHP);
            GetComponent<CharacterStat>()?.UpdateHP(HP);
            
            // Print Damage Amount in UI
            HPBar hpbar = GetComponentInChildren<HPBar>();
            GameObject obj = Instantiate(Resources.Load("DamageMessage"), hpbar.transform.parent.position, Quaternion.identity, gameObject.transform) as GameObject;
            obj.transform.Translate(0f, 1.0f, 0f);           
            obj.GetComponent<MessageUI>()?.SetDamage(msg.amount);

            // Set Dead if HP Equal or Under Zero
            if (!isDead && HP <= Mathf.Epsilon)
            {
                Debug.Log("SetDead");
                isDead = true;
                onDeadDel?.Invoke();
            }
                
        }
        
        /// <summary>
        /// 들어오는 데미지를 상성에 맞는 데미지로 다시 계산합니다.
        /// </summary>
        /// <param name="msg">메시지가 저장될 레퍼런스 데이터</param>
        public void GetModifiedMessage(ref FDamageMessage msg)
        {
            if (msg.Causer.GetComponent<CharacterStat>() == null)
                return;

            SIMBOL_ARMOR armor = GetComponent<CharacterStat>().armor;
            SIMBOL_WEAPON weapon = msg.Causer.GetComponent<CharacterStat>().weapon;
            if (armor != SIMBOL_ARMOR.UNKNOWN)
            {
                if (weapon != SIMBOL_WEAPON.UNKNOWN)
                {
                    switch (armor)
                    {
                        case SIMBOL_ARMOR.LIGHT:
                            if (weapon == SIMBOL_WEAPON.SHORTED)
                                msg.amount *= 0.75f;
                            else if (weapon == SIMBOL_WEAPON.RANGED)
                                msg.amount *= 1.25f;
                            break;
                        case SIMBOL_ARMOR.HEAVY:
                            if (weapon == SIMBOL_WEAPON.RANGED)
                                msg.amount *= 0.75f;
                            else if (weapon == SIMBOL_WEAPON.MAGIC)
                                msg.amount *= 1.25f;
                            break;
                        case SIMBOL_ARMOR.MAGICAL:
                            if (weapon == SIMBOL_WEAPON.MAGIC)
                                msg.amount *= 0.75f;
                            else if (weapon == SIMBOL_WEAPON.SHORTED)
                                msg.amount *= 1.25f;
                            break;
                    }
                }
            }
        }

        // TickEffectDamage에서 OnTriggerExit를 이용해 조금 더 데미지를 줄때 사용, 또는 추가 피해를 TickDamage로 입힐 때 사용.
        public void GetTickDamage(FTickDamageMessage msg)
        {
            StartCoroutine(StartTickDamage(msg));
        }

        IEnumerator StartTickDamage(FTickDamageMessage msg)
        {
            float tickDamage = msg.amount;
            float ratio = msg.tickTime / msg.amountTime;
            tickDamage *= ratio;

            // 실제 GetDamage에 들어갈 함수
            FDamageMessage damageMsg;
            damageMsg.Causer = msg.causer;
            damageMsg.amount = tickDamage;

            while (msg.amountTime > Mathf.Epsilon)
            {
                GetDamage(damageMsg);
                msg.amountTime -= msg.tickTime;
                yield return new WaitForSeconds(msg.tickTime);
            }
            
        }
    }
}

