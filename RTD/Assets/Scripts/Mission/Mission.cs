using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterKit;
using UnityEngine.Events;

public abstract class Mission
{
    public List<MissionCategory> Categories = new List<MissionCategory>();

    public enum STATE
    {
        Waiting,
        Working,
        Succeed,
        Fail,
        Finished
    }
    public STATE State = STATE.Waiting;
    protected uint reward = 0;
    
    public virtual void Init(GameObject GameManager)
    {
        State = STATE.Working;
        foreach (MissionCategory category in Categories)
        {
            category.Init(GameManager);
        }
        UpdateMessage();
        UpdateVerify();
    }
    public virtual void UpdateVerify()
    {
        if(!(State == STATE.Fail || State == STATE.Finished))
        {
            bool succeed = true;
            bool fail = true;
            foreach (MissionCategory category in Categories)
            {
                category.Verify();
                if (category.State != MissionCategory.STATE.Succeed)
                {
                    succeed = false;
                }
                if (category.State != MissionCategory.STATE.Fail)
                {
                    fail = false;
                }
            }
            if (succeed) State = STATE.Succeed;
            if (fail) State = STATE.Fail;
        }
    }
    public string UpdateMessage()
    {
        string str = "";
        for(int i = 0; i < Categories.Count; i++)
        {
            str += Categories[i].Message();
            if(i != Categories.Count-1)
                str += ", ";

        }
        if (State == STATE.Finished)
            str += " 완료";
        if (State == STATE.Fail)
            str += " 실패";
        return str;
    }
    public virtual uint Reward()
    {
        return reward;
    }
    public void MissionFinished()
    {
        State = STATE.Finished;
    }
}
public class Mission_GetChar_NM7 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_NM7()
    {
        reward = 700;
        Category1.PushCharacters(GRADE.NORMAL, UNION.MAGE, 7, "N법사");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_MW2MM2MA2 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_MW2MM2MA2()
    {
        reward = 1000;
        Category1.PushCharacters(GRADE.MAGIC, UNION.WARRIOR, 2, "M전사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.MAGE, 2, "M법사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.ARCHER, 2, "M아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_MM3RM3 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_MM3RM3()
    {
        reward = 2000;
        Category1.PushCharacters(GRADE.MAGIC, UNION.MAGE, 3, "M법사");
        Category1.PushCharacters(GRADE.RARE, UNION.MAGE, 3, "R법사");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_QM3 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_QM3()
    {
        reward = 2700;
        Category1.PushCharacters(GRADE.UNIQUE, UNION.MAGE, 3, "Q법사");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_MM1RA1 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_MM1RA1()
    {
        reward = 800;
        Category1.PushCharacters(GRADE.MAGIC, UNION.MAGE, 1, "M법사");
        Category1.PushCharacters(GRADE.RARE, UNION.ARCHER, 1, "R아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_MW2MM2MA1 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_MW2MM2MA1()
    {
        reward = 1000;
        Category1.PushCharacters(GRADE.MAGIC, UNION.WARRIOR, 2, "M전사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.MAGE, 2, "M법사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.ARCHER, 1, "M아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_RW4 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_RW4()
    {
        reward = 2000;
        Category1.PushCharacters(GRADE.RARE, UNION.WARRIOR, 4, "R전사");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_RM2RA3 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_RM2RA3()
    {
        reward = 2500;
        Category1.PushCharacters(GRADE.RARE, UNION.MAGE, 2, "R법사");
        Category1.PushCharacters(GRADE.RARE, UNION.ARCHER, 3, "R아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_MW3MA2 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_MW3MA2()
    {
        reward = 1200;
        Category1.PushCharacters(GRADE.MAGIC, UNION.WARRIOR, 3, "M전사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.ARCHER, 2, "M아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_RW2RM2 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_RW2RM2()
    {
        reward = 2000;
        Category1.PushCharacters(GRADE.MAGIC, UNION.WARRIOR, 3, "M전사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.ARCHER, 2, "M아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_QW1QM1QA1 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_QW1QM1QA1()
    {
        reward = 3000;
        Category1.PushCharacters(GRADE.UNIQUE, UNION.WARRIOR, 1, "Q전사");
        Category1.PushCharacters(GRADE.UNIQUE, UNION.MAGE, 1, "Q법사");
        Category1.PushCharacters(GRADE.UNIQUE, UNION.ARCHER, 1, "Q아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_NW2NM2NA2 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_NW2NM2NA2()
    {
        reward = 800;
        Category1.PushCharacters(GRADE.NORMAL, UNION.WARRIOR, 2, "N전사");
        Category1.PushCharacters(GRADE.NORMAL, UNION.MAGE, 2, "N법사");
        Category1.PushCharacters(GRADE.NORMAL, UNION.ARCHER, 2, "N아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_MW1MM2 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_MW1MM2()
    {
        reward = 1000;
        Category1.PushCharacters(GRADE.MAGIC, UNION.WARRIOR, 1, "M전사");
        Category1.PushCharacters(GRADE.MAGIC, UNION.MAGE, 2, "M법사");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_RW1RM1RA1 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_RW1RM1RA1()
    {
        reward = 1800;
        Category1.PushCharacters(GRADE.RARE, UNION.WARRIOR, 1, "R전사");
        Category1.PushCharacters(GRADE.RARE, UNION.MAGE, 1, "R법사");
        Category1.PushCharacters(GRADE.RARE, UNION.ARCHER, 1, "R아처");
        Categories.Add(Category1);
    }
}
public class Mission_GetChar_RW2RM2QA2 : Mission
{
    MissionGetCharacter Category1 = new MissionGetCharacter();
    public Mission_GetChar_RW2RM2QA2()
    {
        reward = 4500;
        Category1.PushCharacters(GRADE.RARE, UNION.WARRIOR, 2, "R전사");
        Category1.PushCharacters(GRADE.RARE, UNION.MAGE, 2, "R법사");
        Category1.PushCharacters(GRADE.UNIQUE, UNION.ARCHER, 2, "Q아처");
        Categories.Add(Category1);
    }
}

public class Mission_AllKillNextRound : Mission
{
    MissionNextRoundAllKillMonster Category1 = new MissionNextRoundAllKillMonster();

    public Mission_AllKillNextRound()
    {
        reward = 25;
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}

public class Mission_Kill_H10 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_H10()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.HEAVY, 10, "Heavy타입 몬스터");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}
public class Mission_Kill_Monster15 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_Monster15()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.UNKNOWN, 15, "몬스터");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}
public class Mission_Kill_Monster20 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_Monster20()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.UNKNOWN, 20, "몬스터");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}
public class Mission_Kill_Monster30 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_Monster30()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.UNKNOWN, 30, "몬스터");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}

public class Mission_Kill_L15 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_L15()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.LIGHT, 15, "Light타입");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}

public class Mission_Kill_M15 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_M15()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.MAGICAL, 10, "Magical타입");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}

public class Mission_Kill_H5L5M5 : Mission
{
    MissionKillMonster Category1 = new MissionKillMonster();

    public Mission_Kill_H5L5M5()
    {
        reward = 25;
        Category1.PushMonsters(SIMBOL_ARMOR.HEAVY, 5, "Magical타입");
        Category1.PushMonsters(SIMBOL_ARMOR.LIGHT, 5, "Light타입");
        Category1.PushMonsters(SIMBOL_ARMOR.MAGICAL, 5, "Magical타입");
        Categories.Add(Category1);
    }
    public override uint Reward()
    {
        return reward * (uint)(Category1.MaxCnt);
    }
}