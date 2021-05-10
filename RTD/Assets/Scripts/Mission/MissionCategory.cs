using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;
using UnityEngine.Events;

public abstract class MissionCategory
{
    public enum STATE
    { 
        None,
        Succeed,
        Fail
    }
    public STATE State = STATE.None;
    public virtual void Init(GameObject GameManager)
    {
    }
    public abstract void Verify();
    public abstract string Message();
}
public class MissionKillMonster : MissionCategory
{
    public struct Kit
    {
        public CharacterKit.SIMBOL_ARMOR armor;
        public int num;
        public string msg;

        public Kit(CharacterKit.SIMBOL_ARMOR armor, int num, string msg)
        {
            this.armor = armor;
            this.num = num;
            this.msg = msg;
        }
    }

    public List<Kit> KitList = new List<Kit>();
    List<int> CheckCnt = new List<int>();
    public int MaxCnt = 0;
    WaveSpawner WaveSpawner = null;

    CharacterKit.SIMBOL_ARMOR TargetArmor;

    public override void Init(GameObject GameManager)
    {
        base.Init(GameManager);

        for (int i = 0; i < CheckCnt.Count; i++)
        {
            CheckCnt[i] = 0;
        }

        WaveSpawner = GameManager.GetComponent<WaveSpawner>();
        WaveSpawner.SpawnDelegate += SpawnCharacter;
    }

    public void PushMonsters(CharacterKit.SIMBOL_ARMOR armor, int num, string msg)
    {
        KitList.Add(new Kit(armor, num, msg));
        CheckCnt.Add(0);

        MaxCnt += num;
    }

    public override void Verify()
    {
        bool Succeed = true;
        for (int i = 0; i < KitList.Count; i++)
        {
            if (KitList[i].num > CheckCnt[i])
            {
                Succeed = false;
            }
        }
        State = Succeed ? STATE.Succeed : STATE.None;

    }
    public override string Message()
    {
        string str = "처치미션\n";
        for (int i = 0; i < KitList.Count; i++)
        {
            str += KitList[i].msg + CheckCnt[i] + "/" + KitList[i].num.ToString() + " ";
        }
        return str;
    }
    public void CountDead()
    {
        for (int i = 0; i < KitList.Count; i++)
        {
            Debug.Log(KitList[i].armor + ", " + TargetArmor);
            // 아무 몬스터 처치시
            if(KitList[i].armor == CharacterKit.SIMBOL_ARMOR.UNKNOWN)
            {
                CheckCnt[i]++;
            }
            // 특정 아머타입 몬스터 처치시
            else if (KitList[i].armor == TargetArmor)
            {
                CheckCnt[i]++;
            }
        }
    }
    public void SpawnCharacter(GameObject obj)
    {
        obj.GetComponent<Damageable>().onDeadDel += CountDead;
        TargetArmor = obj.GetComponent<CharacterStat>().armor;
    }
}
public class MissionNextRoundAllKillMonster : MissionCategory
{
    int ExcuteRound;
    int CheckCnt;
    public int MaxCnt = 0;

    GamePlay GamePlay = null;
    WaveSpawner WaveSpawner = null;

    CharacterKit.SIMBOL_ARMOR TargetArmor;

    public override void Init(GameObject GameManager)
    {
        base.Init(GameManager);

        GamePlay = GameManager.GetComponent<GamePlay>();
        WaveSpawner = GameManager.GetComponent<WaveSpawner>();

        CheckCnt = 0;

        ExcuteRound = GamePlay.GetRound() + 1;
        MaxCnt = GamePlay.RoundList[ExcuteRound-1].enemyCount;
        WaveSpawner.SpawnDelegate += SpawnCharacter;
    }

    public override void Verify()
    {
        if(ExcuteRound < GamePlay.GetRound())
        {
            State = STATE.Fail;
        }
        else if (CheckCnt >= MaxCnt)
        {
            State = STATE.Succeed;
        }
    }
    public override string Message()
    {
        string str = ExcuteRound + "라운드올킬\n";
        str += CheckCnt + "/" + MaxCnt + " ";
        return str;
    }
    public void CountDead()
    {
        CheckCnt++;
    }
    public void SpawnCharacter(GameObject obj)
    {
        if(ExcuteRound == GamePlay.GetRound())
        {
            obj.GetComponent<Damageable>().onDeadDel += CountDead;
            TargetArmor = obj.GetComponent<CharacterStat>().armor;
        }
    }
}

public class MissionGetCharacter : MissionCategory
{
    public struct Kit
    {
        public CharacterKit.GRADE grade;
        public CharacterKit.UNION union;
        public int num;
        public string msg;

        public Kit(GRADE grade, UNION union, int num, string msg)
        {
            this.grade = grade;
            this.union = union;
            this.num = num;
            this.msg = msg;
        }
    }

    public List<Kit> KitList = new List<Kit>();
    List<int> CheckCnt = new List<int>();

    PickController PickController = null;
    CharacterInfoManager CharacterInfoManager = null;

    public override void Init(GameObject GameManager)
    {
        base.Init(GameManager);
        PickController = GameManager.GetComponent<PickController>();
        CharacterInfoManager = GameManager.GetComponent<CharacterInfoManager>();

        for (int i = 0; i < CheckCnt.Count; i++)
        {
            CheckCnt[i] = 0;
        }
    }

    public void PushCharacters(CharacterKit.GRADE grade, CharacterKit.UNION union, int num, string msg)
    {
        KitList.Add(new Kit(grade, union, num, msg));
        CheckCnt.Add(0);
    }

    public override void Verify()
    {
        bool Succeed = true;
        for (int i = 0; i < KitList.Count; i++)
        {
            int cnt = CharacterInfoManager.GetAllCharactersCount(KitList[i].grade, KitList[i].union);
            CheckCnt[i] = cnt;
            if (cnt < KitList[i].num)
            {
                Succeed = false;
            }
        }
        State = Succeed ? STATE.Succeed : STATE.None;
    }
    public override string Message()
    {
        string str = "수집미션\n";
        for (int i = 0; i < KitList.Count; i++)
        {
            str += KitList[i].msg + CheckCnt[i] + "/" + KitList[i].num.ToString() + " ";
        }
        return str;
    }

}
