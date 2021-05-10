using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;
public class LevelUpManager : MonoBehaviour
{
    CharacterInfoManager CharInfoManager;
    MoneyManager MoneyManager;

    public BtnLevelUpMage MAGE = null;
    public BtnLevelUpWarrior WARRIOR = null;
    public BtnLevelUpArcher ARCHER = null;

    public AudioClip Audio_Upgrade;
    public AudioClip Audio_Fail;

    ResponseMessage.Trade.CODE response;
    // Start is called before the first frame update
    void Start()
    {
        // Find Reference
        if (MAGE == null) MAGE = GameObject.Find("Mage").GetComponent<BtnLevelUpMage>();
        if (WARRIOR == null) WARRIOR = GameObject.Find("Warrior").GetComponent<BtnLevelUpWarrior>();
        if (ARCHER == null) ARCHER = GameObject.Find("Archer").GetComponent<BtnLevelUpArcher>();

        CharInfoManager = GetComponent<CharacterInfoManager>();
        MoneyManager = GetComponent<MoneyManager>();

        MAGE.OnclickDelegate += LevelUpMage;
        WARRIOR.OnclickDelegate += LevelUpWarrior;
        ARCHER.OnclickDelegate += LevelUpArcher;

        GameObject.Find("Storage").GetComponent<Storage>().CreateCharacterDelegate += UpdateCharacterLevel;
    }
    void LevelUpMage()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, MAGE.Price, response, "MAGE Level Up"))
        {
            BtnLevelUpMage.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.MAGE, BtnLevelUpMage.Level);
            SoundManager.I.PlayEffectSound(Audio_Upgrade);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
            SoundManager.I.PlayEffectSound(Audio_Fail);
        }
    }

    void LevelUpWarrior()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, WARRIOR.Price, response, "WARRIOR Level Up"))
        {
            BtnLevelUpWarrior.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.WARRIOR, BtnLevelUpWarrior.Level);
            SoundManager.I.PlayEffectSound(Audio_Upgrade);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
            SoundManager.I.PlayEffectSound(Audio_Fail);
        }
    }

    void LevelUpArcher()
    {
        if (MoneyManager.CalculateMoney(MoneyManager.ACTION.Pay, ARCHER.Price, response, "ARCHER Level Up"))
        {
            BtnLevelUpArcher.Level += 1;
            CharUtils.UpdateSpecificUnion(CharacterKit.UNION.ARCHER, BtnLevelUpArcher.Level);
            SoundManager.I.PlayEffectSound(Audio_Upgrade);
        }
        else
        {
            Debug.Log(ResponseMessage.Trade.Receive(response));
            SoundManager.I.PlayEffectSound(Audio_Fail);
        }
    }

    public void UpdateCharacterLevel(GameObject character)
    {
        CharacterKit.UNION union = character.GetComponent<CharacterStat>().union;
        character.GetComponent<CharacterStat>().UpdateStat(GetLevel(union));
        //CharUtils.UpdateSpecificUnion(union, GetLevel(union));
    }

    int GetLevel(CharacterKit.UNION union)
    {
        int level = 0;
        switch(union)
        {
            case CharacterKit.UNION.MAGE:
                level = BtnLevelUpMage.Level;
                break;
            case CharacterKit.UNION.WARRIOR:
                level = BtnLevelUpWarrior.Level;
                break;
            case CharacterKit.UNION.ARCHER:
                level = BtnLevelUpArcher.Level;
                break;
        }
        return level;
    }

    public void Init()
    {
        MAGE.Init();
        WARRIOR.Init();
        ARCHER.Init();
    }
}
