using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterKit
{

    /// <summary>
    /// 캐릭터 등급을 세팅합니다.
    /// </summary>
    public enum GRADE
    {
        NORMAL, MAGIC, RARE, UNIQUE
    }

    /// <summary>
    /// 추후 캐릭터 업그레이드에 쓰일 진영값
    /// </summary>
    public enum UNION
    {
        ENEMY, MAGE, WARRIOR, ARCHER
    }

    /// <summary>
    /// 캐릭터 고유 ID값입니다.
    /// 해당 값을 CharacterStat의 인스펙터창에서 넣어주면 됩니다. (UNKNOWN, NORMAL, MAGIC, RARE, UNIQUE는 넣으면 x)
    /// </summary>
    public enum ID
    {
        UNKNOWN = -1,
        NORMAL = 0 , N_HERO, N_CASTLEKNIGHT, N_GLADIATOR, N_BERSERKER, N_SWORDMASTER,
                     N_BOWKNIGHT = 21, N_ASSASSIN, N_HUNTER, N_CROSSBOWKNIGHT, N_BOWKING,
                     N_LICH = 41, N_MAGEKNIGHT, N_WIZARD, N_DARKWIZARD, N_PRIEST,
        MAGIC = 100, M_HERO, M_CASTLEKNIGHT, M_GLADIATOR, M_BERSERKER, M_SWORDMASTER,
                     M_BOWKNIGHT = 121, M_ASSASSIN, M_HUNTER, M_CROSSBOWKNIGHT, M_BOWKING,
                     M_LICH = 141, M_MAGEKNIGHT, M_WIZARD, M_DARKWIZARD, M_PRIEST,
        RARE = 200,  R_HERO, R_CASTLEKNIGHT, R_GLADIATOR, R_BERSERKER, R_SWORDMASTER,
                     R_BOWKNIGHT = 221, R_ASSASSIN, R_HUNTER, R_CROSSBOWKNIGHT, R_BOWKING,
                     R_LICH = 241, R_MAGEKNIGHT, R_WIZARD, R_DARKWIZARD, R_PRIEST,
        UNIQUE = 300, Q_HERO, Q_CASTLEKNIGHT, Q_GLADIATOR, Q_BERSERKER, Q_SWORDMASTER,
                     Q_BOWKNIGHT = 321, Q_ASSASSIN, Q_HUNTER, Q_CROSSBOWKNIGHT, Q_BOWKING,
                     Q_LICH = 341, Q_MAGEKNIGHT, Q_WIZARD, Q_DARKWIZARD, Q_PRIEST,
    }
    
    public enum SIMBOL_WEAPON
    {
        UNKNOWN, 
        SHORTED, RANGED, MAGIC
    }

    public enum SIMBOL_ARMOR
    {
        UNKNOWN,
        LIGHT, HEAVY, MAGICAL
    }

    /// <summary>
    /// For PlayerCharacter STATE MACHINE
    /// </summary>
    public enum BASICSTATE
    {
        NONE, CREATE, POSTCREATE,
        WAIT, ATTACHFIELD, DETACHFIELD,
        MOVE,
        DETECT, ATTACK, 
        READYSKILL, USESKILL,
        DEAD
    }


    /// <summary>
    /// Struct: 아군, 적의 스텟값을 저장하기 위한 구조체입니다.
    /// 인스펙터창에서 해당 값을 수정할 수 있습니다.
    /// </summary>
    [System.Serializable]
    public struct FCharacterStat
    {
        public float MaxHP;
        public float HP;
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;
        public float rotateSpeed;

        public void Init(float MaxHP, float HP, float attackDamage, float attackSpeed, float attackRange, float moveSpeed, float rotateSpeed)
        {
            this.MaxHP = MaxHP;
            this.HP = HP;
            this.attackDamage = attackDamage;
            this.attackSpeed = attackSpeed;
            this.attackRange = attackRange;
            this.moveSpeed = moveSpeed;
            this.rotateSpeed = rotateSpeed;
        }
    }


}
