using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDB : MonoBehaviour
{

    public string[] Card = new string[] {
        "UI/CardList/EmptyCard",
        "UI/CardList/N_Assassin",
        "UI/CardList/N_Berserker",
        "UI/CardList/N_BowKing",
        "UI/CardList/N_BowKnight",
        "UI/CardList/N_CastleKnight",
        "UI/CardList/N_CrossBow Knight",
        "UI/CardList/N_Dark Wizard",
        "UI/CardList/N_Gladiator",
        "UI/CardList/N_Hero",
        "UI/CardList/N_Hunter",
        "UI/CardList/N_Lich",
        "UI/CardList/N_Mage Knight",
        "UI/CardList/N_Priest",
        "UI/CardList/N_SwordMaster",
        "UI/CardList/N_Wizard"
    };

    public string[] CharacterAllClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/N_Marine",
        "Character/Player Characters/Long Ranged/M_Marine",
        "Character/Player Characters/Long Ranged/R_Marine",
        "Character/Player Characters/Long Ranged/Q_Marine",
        "Character/Player Characters/Magic/N_Lich",
        "Character/Player Characters/Magic/M_Lich",
        "Character/Player Characters/Magic/R_Lich",
        "Character/Player Characters/Magic/Q_Lich",
        "Character/Player Characters/Short Ranged/N_Warrior",
        "Character/Player Characters/Short Ranged/M_Warrior",
        "Character/Player Characters/Short Ranged/R_Warrior",
        "Character/Player Characters/Short Ranged/Q_Warrior"
    };

    public string[] CharacterNoramlClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/N_Assassin",
        "Character/Player Characters/Long Ranged/Grade/N_BowKing",
        "Character/Player Characters/Long Ranged/Grade/N_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/N_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/N_Hunter",
        "Character/Player Characters/Magic/Grade/N_Wizard",
        "Character/Player Characters/Magic/Grade/N_Dark Wizard",
        "Character/Player Characters/Magic/Grade/N_Mage Knight",
        "Character/Player Characters/Magic/Grade/N_Priest",
        "Character/Player Characters/Magic/Grade/N_Lich",
        "Character/Player Characters/Short Ranged/Grade/N_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/N_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/N_Hero",
        "Character/Player Characters/Short Ranged/Grade/N_Berserker",
        "Character/Player Characters/Short Ranged/Grade/N_SwordMaster"
    };
    public string[] CharacterMagicClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/M_Assassin",
        "Character/Player Characters/Long Ranged/Grade/M_BowKing",
        "Character/Player Characters/Long Ranged/Grade/M_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/M_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/M_Hunter",
        "Character/Player Characters/Magic/Grade/M_Wizard",
        "Character/Player Characters/Magic/Grade/M_Dark Wizard",
        "Character/Player Characters/Magic/Grade/M_Mage Knight",
        "Character/Player Characters/Magic/Grade/M_Priest",
        "Character/Player Characters/Magic/Grade/M_Lich",
        "Character/Player Characters/Short Ranged/Grade/M_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/M_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/M_Hero",
        "Character/Player Characters/Short Ranged/Grade/M_Berserker",
        "Character/Player Characters/Short Ranged/Grade/M_SwordMaster"
    };
    public string[] CharacterRareClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/R_Assassin",
        "Character/Player Characters/Long Ranged/Grade/R_BowKing",
        "Character/Player Characters/Long Ranged/Grade/R_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/R_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/R_Hunter",
        "Character/Player Characters/Magic/Grade/R_Wizard",
        "Character/Player Characters/Magic/Grade/R_Dark Wizard",
        "Character/Player Characters/Magic/Grade/R_Mage Knight",
        "Character/Player Characters/Magic/Grade/R_Priest",
        "Character/Player Characters/Magic/Grade/R_Lich",
        "Character/Player Characters/Short Ranged/Grade/R_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/R_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/R_Hero",
        "Character/Player Characters/Short Ranged/Grade/R_Berserker",
        "Character/Player Characters/Short Ranged/Grade/R_SwordMaster"
    };
    public string[] CharacterUniqueClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/Q_Assassin",
        "Character/Player Characters/Long Ranged/Grade/Q_BowKing",
        "Character/Player Characters/Long Ranged/Grade/Q_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/Q_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/Q_Hunter",
        "Character/Player Characters/Magic/Grade/Q_Wizard",
        "Character/Player Characters/Magic/Grade/Q_Dark Wizard",
        "Character/Player Characters/Magic/Grade/Q_Mage Knight",
        "Character/Player Characters/Magic/Grade/Q_Priest",
        "Character/Player Characters/Magic/Grade/Q_Lich",
        "Character/Player Characters/Short Ranged/Grade/Q_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/Q_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/Q_Hero",
        "Character/Player Characters/Short Ranged/Grade/Q_Berserker",
        "Character/Player Characters/Short Ranged/Grade/Q_SwordMaster"
    };

    public string[] CharacterNoramlMageClassAddr = new string[] {
        "Character/Player Characters/Magic/Grade/N_Wizard",
        "Character/Player Characters/Magic/Grade/N_Dark Wizard",
        "Character/Player Characters/Magic/Grade/N_Mage Knight",
        "Character/Player Characters/Magic/Grade/N_Priest",
        "Character/Player Characters/Magic/Grade/N_Lich",
    };
    public string[] CharacterNoramlWarriorClassAddr = new string[] {
        "Character/Player Characters/Short Ranged/Grade/N_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/N_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/N_Hero",
        "Character/Player Characters/Short Ranged/Grade/N_Berserker",
        "Character/Player Characters/Short Ranged/Grade/N_SwordMaster"
    };
    public string[] CharacterNoramlArcherClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/N_Assassin",
        "Character/Player Characters/Long Ranged/Grade/N_BowKing",
        "Character/Player Characters/Long Ranged/Grade/N_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/N_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/N_Hunter"
    };
    public string[] CharacterMagicMageClassAddr = new string[] {
        "Character/Player Characters/Magic/Grade/M_Wizard",
        "Character/Player Characters/Magic/Grade/M_Dark Wizard",
        "Character/Player Characters/Magic/Grade/M_Mage Knight",
        "Character/Player Characters/Magic/Grade/M_Priest",
        "Character/Player Characters/Magic/Grade/M_Lich",
    };
    public string[] CharacterMagicWarriorClassAddr = new string[] {
        "Character/Player Characters/Short Ranged/Grade/M_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/M_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/M_Hero",
        "Character/Player Characters/Short Ranged/Grade/M_Berserker",
        "Character/Player Characters/Short Ranged/Grade/M_SwordMaster"
    };
    public string[] CharacterMagicArcherClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/M_Assassin",
        "Character/Player Characters/Long Ranged/Grade/M_BowKing",
        "Character/Player Characters/Long Ranged/Grade/M_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/M_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/M_Hunter"
    };
    public string[] CharacterRareMageClassAddr = new string[] {
        "Character/Player Characters/Magic/Grade/R_Wizard",
        "Character/Player Characters/Magic/Grade/R_Dark Wizard",
        "Character/Player Characters/Magic/Grade/R_Mage Knight",
        "Character/Player Characters/Magic/Grade/R_Priest",
        "Character/Player Characters/Magic/Grade/R_Lich",
    };
    public string[] CharacterRareWarriorClassAddr = new string[] {
        "Character/Player Characters/Short Ranged/Grade/R_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/R_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/R_Hero",
        "Character/Player Characters/Short Ranged/Grade/R_Berserker",
        "Character/Player Characters/Short Ranged/Grade/R_SwordMaster"
    };
    public string[] CharacterRareArcherClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/R_Assassin",
        "Character/Player Characters/Long Ranged/Grade/R_BowKing",
        "Character/Player Characters/Long Ranged/Grade/R_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/R_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/R_Hunter"
    };
    public string[] CharacterUniqueMageClassAddr = new string[] {
        "Character/Player Characters/Magic/Grade/Q_Wizard",
        "Character/Player Characters/Magic/Grade/Q_Dark Wizard",
        "Character/Player Characters/Magic/Grade/Q_Mage Knight",
        "Character/Player Characters/Magic/Grade/Q_Priest",
        "Character/Player Characters/Magic/Grade/Q_Lich",
    };
    public string[] CharacterUniqueWarriorClassAddr = new string[] {
        "Character/Player Characters/Short Ranged/Grade/Q_CastleKnight",
        "Character/Player Characters/Short Ranged/Grade/Q_Gladiator",
        "Character/Player Characters/Short Ranged/Grade/Q_Hero",
        "Character/Player Characters/Short Ranged/Grade/Q_Berserker",
        "Character/Player Characters/Short Ranged/Grade/Q_SwordMaster"
    };
    public string[] CharacterUniqueArcherClassAddr = new string[] {
        "Character/Player Characters/Long Ranged/Grade/Q_Assassin",
        "Character/Player Characters/Long Ranged/Grade/Q_BowKing",
        "Character/Player Characters/Long Ranged/Grade/Q_BowKnight",
        "Character/Player Characters/Long Ranged/Grade/Q_CrossBow Knight",
        "Character/Player Characters/Long Ranged/Grade/Q_Hunter"
    };
    // LJH: TO DO - Prefab list Add
    public string[] PreLoadingPrefabAddr = new string[]{
        //"Visual Effect/Skill Effect/Marine Skill",
        //"Visual Effect/Skill Effect/Marine SkillReady",
        //"Character/Boss/Dragon",
        //"Character/Enemy/Golem",
        //"Character/Enemy/Slime",
        //"Character/Enemy/TurtleShell",
        //"Character/Player Characters/Long Ranged/Marine",
        //"Character/Player Characters/Long Ranged/N_Marine",
        //"Character/Player Characters/Long Ranged/M_Marine",
        //"Character/Player Characters/Long Ranged/R_Marine",
        //"Character/Player Characters/Long Ranged/Q_Marine",
        //"Character/Player Characters/Magic/Lich",
        //"Character/Player Characters/Magic/N_Lich",
        //"Character/Player Characters/Magic/M_Lich",
        //"Character/Player Characters/Magic/R_Lich",
        //"Character/Player Characters/Magic/Q_Lich",
        //"Character/Player Characters/Short Ranged/Warrior",
        //"Character/Player Characters/Short Ranged/N_Warrior",
        //"Character/Player Characters/Short Ranged/M_Warrior",
        //"Character/Player Characters/Short Ranged/R_Warrior",
        //"Character/Player Characters/Short Ranged/Q_Warrior",
        //"Projectile/FireBall Hit Effect",
        //"Projectile/FireBall01",
        //"Projectile/Icebolt Hit Effect",
        //"Projectile/Icebolt",
    };

    public string[] Boss = new string[]{
        "Character/Boss/Dragon"
    };
}

