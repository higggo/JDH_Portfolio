using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

namespace CharacterKit
{
    public enum BUFFCATEGORY
    {
        RATIO, RAW
    }

    public class BuffSkill
    {
        public BUFFCATEGORY category;
        public int id;
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;
        public float durationTime;

        public BuffSkill(BUFFCATEGORY category, int id, float attackDamage, float attackSpeed, float moveSpeed, float durationTime = 1f)
        {
            this.category = category;
            this.id = id;
            this.attackDamage = attackDamage;
            this.attackSpeed = attackSpeed;
            this.moveSpeed = moveSpeed;
            this.durationTime = durationTime;
        }

    }
}


public class CharacterStat : MonoBehaviour
{
    GRADE _grade;
    [SerializeField] UNION _union;
    [SerializeField] ID _id = ID.UNKNOWN;
    [SerializeField] SIMBOL_WEAPON _Weapon = SIMBOL_WEAPON.UNKNOWN;
    [SerializeField] SIMBOL_ARMOR _Armor = SIMBOL_ARMOR.UNKNOWN;
    [SerializeField] FCharacterStat basicStat;
    [SerializeField] FCharacterStat bonusStat;
    [SerializeField] FCharacterStat currentStat;
    
    // Delegate
    public UnityAction BuffDel = null;
    public UnityAction OnHPChangeDel = null;

    // Buff
    List<BuffSkill> buffList = new List<BuffSkill>();

    IEnumerator PrintBuff(BuffSkill skill, float lifeTime)
    {
        List<string> printList = new List<string>();
        Color color = new Color();
        
        if (skill.attackDamage != 0)
        {
            if (skill.attackDamage > 0)
                printList.Add("Damage Up!");
            else if (skill.attackDamage < 0)
                printList.Add("Damage Down");
        }
        if (skill.attackSpeed != 0)
        {
            if (skill.attackSpeed > 0)
                printList.Add("AtkSpeed Up!");
            else if (skill.attackSpeed < 0)
                printList.Add("AtkSpeed Down");
        }
        if (skill.moveSpeed != 0)
        {
            if (skill.moveSpeed > 0)
                printList.Add("Speed Up!");
            else if (skill.moveSpeed < 0)
                printList.Add("Speed Down");
        }


        if (printList.Count > 0)
        {
            if (printList[0].Contains("!"))
                color = Color.magenta;
            else
                color = Color.black;

            float time = lifeTime / printList.Count;
            float firstTime = time;
            HPBar hpbar = GetComponentInChildren<HPBar>();
            GameObject obj = Instantiate(Resources.Load("StatusMessage"), hpbar.transform.parent.position, Quaternion.identity, gameObject.transform) as GameObject;
            obj.transform.Translate(0f, 1.0f, 0f);
            obj.GetComponent<MessageUI>()?.SetText(printList[0], color);
            while (printList.Count > 0)
            {
                obj.GetComponent<MessageUI>().txt.alpha = time / firstTime;
                time -= Time.deltaTime;

                if (time < 0)
                {
                    printList.RemoveAt(0);
                    if (printList.Count > 0)
                    {
                        if (printList[0].Contains("!"))
                            color = Color.magenta;
                        else
                            color = Color.black;
                        obj.GetComponent<MessageUI>()?.SetText(printList[0], color);
                        time = firstTime;
                    }
                }
                yield return null;
            }
            Destroy(obj);
        }
    }


    public void AddBuff(BuffSkill skill)
    {
        // Checking has same buff
        foreach (BuffSkill inskill in buffList)
        {
            if (inskill.id == skill.id)
            {
                inskill.durationTime = skill.durationTime;
                return;
            }
        }

        buffList.Add(skill);
        StartCoroutine(PrintBuff(skill, 2.5f));
        StartCoroutine(StartBuffTimer(skill));
        UpdateStat(CharUtils.GetLevel(_union));
    }

    void DeleteBuff(BuffSkill skill)
    {
        int level = CharUtils.GetLevel(_union);
        buffList.Remove(skill);
        UpdateStat(level);
    }

    IEnumerator StartBuffTimer(BuffSkill skill)
    {
        while (skill.durationTime > Mathf.Epsilon)
        {
            skill.durationTime -= Time.deltaTime;
            yield return null;
        }
        DeleteBuff(skill);
    }

    FCharacterStat GetCurrentRatioBuff()
    {
        FCharacterStat ratio = new FCharacterStat();
        ratio.Init(1, 1, 1, 1, 1, 1, 1);
        if (buffList.Count == 0)
            return ratio;

        foreach (BuffSkill inskill in buffList)
        {
            if (inskill.category == BUFFCATEGORY.RATIO)
            {
                ratio.attackDamage = Mathf.Clamp(ratio.attackDamage + inskill.attackDamage, 0.1f, float.MaxValue);
                ratio.attackSpeed = Mathf.Clamp(ratio.attackSpeed + inskill.attackSpeed, 0.1f, float.MaxValue);
                ratio.moveSpeed = Mathf.Clamp(ratio.moveSpeed + inskill.moveSpeed, 0.1f, float.MaxValue);
            }
        }

        return ratio;
    }

    FCharacterStat GetCurrentRawBuff()
    {
        FCharacterStat raw = new FCharacterStat();
        raw.Init(0, 0, 0, 0, 0, 0, 0);
        if (buffList.Count == 0)
            return raw;

        foreach (BuffSkill inskill in buffList)
        {
            if (inskill.category == BUFFCATEGORY.RAW)
            {
                raw.attackDamage += inskill.attackDamage;
                raw.attackSpeed += inskill.attackSpeed;
                raw.moveSpeed += inskill.moveSpeed;
            }
        }

        return raw;
    }

    void Awake()
    {
        currentStat = basicStat;
        // To Do: CharController의 LevelUpDel에 UpdateStat을 연결?
    }


    /// <summary>
    /// 해당 인스턴스 캐릭터의 스텟을 업데이트 합니다.
    /// </summary>
    /// <param name="unionLevel">각 캐릭터의 union에 맞는 레벨값을 넘겨주세요.</param>
    public void UpdateStat(int unionLevel)
    {
        if (unionLevel <= 0)
        {
            currentStat = basicStat;
            return;
        }
        // level이 1인 경우 bonusStat이 더해지면 안되므로 -1 계산
        unionLevel -= 1;
        FCharacterStat buffRatio = GetCurrentRatioBuff();
        FCharacterStat buffRaw = GetCurrentRawBuff();

        FCharacterStat tempStat = basicStat;
        tempStat.attackDamage += bonusStat.attackDamage * unionLevel;
        tempStat.attackRange += bonusStat.attackRange * unionLevel;
        tempStat.attackSpeed += bonusStat.attackSpeed * unionLevel;
        tempStat.MaxHP += bonusStat.MaxHP * unionLevel;
        tempStat.HP = currentStat.HP + bonusStat.HP * unionLevel;

        // Calculate Ratio Buff
        tempStat.attackDamage *= buffRatio.attackDamage;
        tempStat.attackSpeed *= buffRatio.attackSpeed;
        tempStat.moveSpeed *= buffRatio.moveSpeed;

        // Calculate Raw Buff
        tempStat.attackDamage += buffRaw.attackDamage;
        tempStat.attackSpeed += buffRaw.attackSpeed;
        tempStat.moveSpeed += buffRaw.moveSpeed;

        // Clamp
        tempStat.attackDamage = Mathf.Clamp(tempStat.attackDamage, 1, float.MaxValue);
        tempStat.attackSpeed = Mathf.Clamp(tempStat.attackSpeed, 0.1f, float.MaxValue);
        tempStat.moveSpeed = Mathf.Clamp(tempStat.moveSpeed, 0.1f, float.MaxValue);

        currentStat = tempStat;
        OnHPChangeDel?.Invoke();
    }

    public void ResetHP()
    {
        currentStat.HP = currentStat.MaxHP;
        OnHPChangeDel?.Invoke();
    }

    public void UpdateHP(float value)
    {
        if (value < Mathf.Epsilon)
            value = 0.0f;
        else if (value > currentStat.MaxHP)
            value = MaxHP;

        currentStat.HP = value;
        OnHPChangeDel?.Invoke();
    }


    // Property
    public GRADE grade
    {
        get { return _grade; }
        set { _grade = value; }
    }
    
    public UNION union
    {
        get { return _union; }
    }

    public SIMBOL_WEAPON weapon
    {
        get { return _Weapon; }
    }

    public SIMBOL_ARMOR armor
    {
        get { return _Armor; }
    }

    public ID id
    {
        get { return _id; }
    }

    public float MaxHP 
    { 
        get { return currentStat.MaxHP; } 
        set 
        {
            currentStat.MaxHP = value;
        } 
    }
    public float HP
    {
        get { return currentStat.HP; }
        set
        {
            currentStat.HP = value;
        }
    }
    public float attackRange
    {
        get { return currentStat.attackRange; }
        set
        {
            currentStat.attackRange = value;
        }
    }
    public float attackSpeed
    {
        get { return currentStat.attackSpeed; }
        set
        {
            currentStat.attackSpeed = value;
        }
    }
    public float attackDamage
    {
        get { return currentStat.attackDamage; }
        set
        {
            currentStat.attackDamage = value;
        }
    }

    public float moveSpeed
    {
        get { return currentStat.moveSpeed; }
        set
        {
            currentStat.moveSpeed = value;
        }
    }


    
    /// <summary>
    /// 캐릭터 고유 id를 Get합니다.
    /// </summary>
    /// <returns>ID정보를 Get</returns>
    public ID GetID()
    {
        return _id;
    }

    void OnDrawGizmosSelected()
    {
        Color color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, attackRange);
        DebugExtension.DrawCircle(transform.position, attackRange);
    }
}