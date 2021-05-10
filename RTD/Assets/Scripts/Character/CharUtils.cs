using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterKit
{
    public class CharUtils : MonoBehaviour
    {
        // @Summary: Owner의 rotation을 한 번에 Target을 향하도록 변경합니다. (높낮이의 차이는 무시됩니다.)
        public static void RotateToTarget(Transform Owner, Transform Target)
        {
            if (Owner == null || Target == null)
                return;

            RotateToTarget(Owner, Target.position);
        }

        public static void RotateToTarget(Transform Owner, Vector3 Target)
        {
            if (Owner == null || Target == null)
                return;

            Vector3 modifiedTargetPos = Target;
            modifiedTargetPos.y = Owner.position.y;
            Owner.LookAt(modifiedTargetPos);
        }

        public static Quaternion GetRotationToTarget(Transform Owner, Transform Target)
        {
            return GetRotationToTarget(Owner, Target.position);
        }

        public static Quaternion GetRotationToTarget(Transform Owner, Vector3 Target)
        {
            Vector3 dir = Target;
            dir.y = Owner.position.y;
            dir = dir - Owner.position;
            dir.Normalize();

            return Quaternion.LookRotation(dir);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Calculate
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool AnyObjInRange(Transform Instance, float Range, LayerMask Mask)
        {
            List<GameObject> list = new List<GameObject>();
            if (FindTargetAll(Instance.transform, Mask, ref list, Range))
            {
                return true;
            }
            return false;
        }

        public static bool IsInRange(Transform Instance, Transform Target, float Range, LayerMask Mask)
        {
            List<GameObject> list = new List<GameObject>();
            if (FindTargetAll(Instance.transform, Mask, ref list, Range))
            {
                foreach (GameObject obj in list)
                {
                    if (obj == Target.gameObject)
                        return true;
                }
            }

            return false;
        }

        // @Summary: Target이 Instance로부터 Range안에 있는지 검사합니다. (높낮이의 차이는 무시됩니다.)
        public static bool IsInRange(Transform Instance, Transform Target, float Range)
        {
            Vector3 Targetpos = Target.position;
            Targetpos.y = Instance.position.y;

            if (Range < Vector3.Distance(Targetpos, Instance.position))
                return false;

            return true;
        }

        public static bool FindTarget(CharController Instance, LayerMask Mask, float Range, out GameObject obj)
        {
            List<GameObject> list = new List<GameObject>();
            if (FindTargetAll(Instance.transform, Mask, ref list, Range))
                obj = GetCloseTarget(Instance, list);
            else
                obj = null;


            return (obj == null) ? false : true;
        }



        public static bool FindTarget(Transform Instance, LayerMask Mask, float Range, out GameObject obj)
        {
            List<GameObject> list = new List<GameObject>();
            if (FindTargetAll(Instance, Mask, ref list, Range))
                obj = GetCloseTarget(Instance, list);
            else
                obj = null;

            return (obj == null) ? false : true;
        }

        public static bool FindRandomTarget(Transform Instance, LayerMask Mask, float Range, out GameObject obj)
        {
            List<GameObject> list = new List<GameObject>();
            if (FindTargetAll(Instance, Mask, ref list, Range))
            {
                if (list.Count != 0)
                    obj = list[Random.Range(0, list.Count - 1)];
                else
                    obj = null;
            }
                
            else
                obj = null;

            return (obj == null) ? false : true;
            
        }

        // @Summary: 해당 Instance의 위치에서 LayerMask에 따른 SphereCast를 하고, 목표가 있으면 TargetContainer에 담고 true를 리턴합니다.
        public static bool FindTargetAll(CharController Instance, LayerMask Mask, ref List<GameObject> TargetContainer)
        {
            float range = Instance.statInfo.attackRange;
            RaycastHit[] hitInfo = Physics.SphereCastAll(Instance.transform.position, range, Instance.transform.forward, 0.0f, Mask);
            if (hitInfo.Length == 0)
                return false;
            
            TargetContainer.Clear();
            foreach (RaycastHit hit in hitInfo)
            {
                Debug.Log("FindTargetAll");
                GameObject hitObj = hit.collider.gameObject;
                if (!hitObj.GetComponent<Damageable>().IsDead)
                    TargetContainer.Add(hitObj);
            }

            return true;
        }

        public static bool FindTargetAll(Transform Instance, LayerMask Mask, ref List<GameObject> TargetContainer, float range)
        {
            RaycastHit[] hitInfo = Physics.SphereCastAll(Instance.transform.position, range, Instance.transform.forward, 0.0f, Mask);
            if (hitInfo.Length == 0)
                return false;

            TargetContainer.Clear();
            foreach (RaycastHit hit in hitInfo)
            {
                Debug.Log("FindTargetAll");
                GameObject hitObj = hit.collider.gameObject;
                if (!hitObj.GetComponent<Damageable>().IsDead)
                    TargetContainer.Add(hitObj);
            }

            return true;
        }

        // @Summary: Targets중 Instance와 가장 가까운 적을 리턴합니다.
        public static GameObject GetCloseTarget(CharController Instance, List<GameObject> Targets)
        {
            if (Targets.Count == 0)
            {
                Debug.Log("Targets is Empty!");
                return null;
            }

            GameObject TargetObj = Targets[0];
            float MinDist = Vector3.Distance(Instance.transform.position, Targets[0].transform.position);

            foreach (GameObject target in Targets)
            {
                float dist = Vector3.Distance(Instance.transform.position, target.transform.position);
                if (MinDist > dist)
                {
                    MinDist = dist;
                    TargetObj = target;
                }
            }

            Debug.DrawLine(Instance.transform.position, TargetObj.transform.position, Color.red, 0.5f);
            return TargetObj;
        }

        public static GameObject GetCloseTarget(Transform Instance, List<GameObject> Targets)
        {
            if (Targets.Count == 0)
            {
                Debug.Log("Targets is Empty!");
                return null;
            }

            GameObject TargetObj = Targets[0];
            float MinDist = Vector3.Distance(Instance.transform.position, Targets[0].transform.position);

            foreach (GameObject target in Targets)
            {
                float dist = Vector3.Distance(Instance.transform.position, target.transform.position);
                if (MinDist > dist)
                {
                    MinDist = dist;
                    TargetObj = target;
                }
            }

            Debug.DrawLine(Instance.transform.position, TargetObj.transform.position, Color.red, 0.5f);
            return TargetObj;
        }

        public static List<GameObject> GetInFieldAllCharacters(GameObject obj)
        {
            List<GameObject> inField = new List<GameObject>();
            int mask = 1 << obj.layer;
            RaycastHit[] hitInfo = Physics.SphereCastAll(obj.transform.position, 1000, obj.transform.forward, 0.0f, mask);

            foreach (RaycastHit hit in hitInfo)
            {
                if (hit.collider.gameObject.GetComponent<CharController>() == null)
                    continue;

                if (hit.collider.gameObject.GetComponent<CharController>().isInField)
                    inField.Add(hit.collider.gameObject);

            }
            return inField;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Grade
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 캐릭터 Grade에 맞는 식별 가능한 Ring Object를 달아줍니다.
        /// </summary>
        /// <param name="grade">해당 캐릭터의 등급</param>
        /// <param name="parent">붙여줄 트랜스폼</param>
        public static void SettingGradeRing(GRADE grade, Transform parent)
        {
            string path = "Tile/Grade Rings/Unit Ring_";
            int i = (int)grade;
            switch (i)
            {
                case 0:
                    path += "Normal";
                    break;
                case 1:
                    path += "Magic";
                    break;
                case 2:
                    path += "Rare";
                    break;
                case 3:
                    path += "Unique";
                    break;
            }
            GameObject obj = Instantiate(Resources.Load(path), parent) as GameObject;
            float ModifiedUpDir = 0.25f;
            obj.transform.Translate(parent.up * ModifiedUpDir, Space.World);
        }


        /// <summary>
        /// CharacterID를 넘기면 해당 캐릭터의 Grade정보를 반환합니다.
        /// </summary>
        /// <param name="characterID">캐릭터의 고유 코드</param>
        /// <returns></returns>
        public static GRADE SetCharacterGrade(ID characterID)
        {
            if (characterID == ID.UNKNOWN
                || characterID == ID.NORMAL || characterID == ID.MAGIC
                || characterID == ID.RARE || characterID == ID.UNIQUE)
                return GRADE.NORMAL;

            int i = (int)characterID / 100;
            GRADE grade;
            switch (i)
            {
                case 0:
                    grade = GRADE.NORMAL;
                    break;
                case 1:
                    grade = GRADE.MAGIC;
                    break;
                case 2:
                    grade = GRADE.RARE;
                    break;
                case 3:
                    grade = GRADE.UNIQUE;
                    break;
                default:
                    grade = GRADE.NORMAL;
                    break;
            }
            return grade;
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Stat
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 특정 유니온의 캐릭터들의 스텟을 전부 업데이트합니다.
        /// </summary>
        /// <param name="union">업데이트할 유니온</param>
        /// <param name="unionLevel">해당 유니온 레벨</param>
        public static void UpdateSpecificUnion(UNION union, int unionLevel)
        {
            GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject character in characters)
            {
                if (character.GetComponent<CharacterStat>() == null
                    || character.GetComponent<CharacterStat>().union != union)
                    continue;

                character.GetComponent<CharacterStat>().UpdateStat(unionLevel);
            }
        }



        /// <summary>
        /// 필드의 모든 플레이어 캐릭터의 스킬 쿨다운 스위치를 라운드 시작, 끝에 따라 변경합니다.
        /// </summary>
        /// <param name="RoundStart">라운드가 시작하면 true를, 끝났을 때는 false를 대입해주세요.</param>
        public static void SetSkillCoolDownTrigger(bool RoundStart)
        {
            GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject character in characters)
            {
                CharController controller = character.GetComponent<CharController>();
                if (controller == null)
                    continue;

                if (RoundStart)
                {
                    if (controller.isInField)
                    {
                        if (controller.skillController != null)
                            controller.skillController.startCoolDown = true;
                    }
                    else
                    {
                        if (controller.skillController != null)
                            controller.skillController.startCoolDown = false;
                    }
                }
                else
                {
                    if (controller.skillController != null)
                        controller.skillController.ResetAll();
                }
            }
        }

        public static int GetLevel(UNION characterUnion)
        {
            int level = 1;
            switch (characterUnion)
            {
                case UNION.ENEMY:
                    level = 1;
                    break;
                case UNION.WARRIOR:
                    level = BtnLevelUpWarrior.Level;
                    break;
                case UNION.ARCHER:
                    level = BtnLevelUpArcher.Level;
                    break;
                case UNION.MAGE:
                    level = BtnLevelUpMage.Level;
                    break;
            }

            return (level <= 0) ? 1 : level;
        }

        /// <summary>
        /// 두 대상이 같은 캐릭터인지 검사합니다.
        /// </summary>
        /// <param name="owner">검사할 오브젝트 1번</param>
        /// <param name="target">검사할 오브젝트 2번</param>
        /// <returns></returns>
        public static bool CompareID(GameObject owner, GameObject target)
        {
            if (owner.GetComponent<CharacterStat>() != null
                && target.GetComponent<CharacterStat>() != null)
            {
                if (owner.GetComponent<CharacterStat>().id == target.GetComponent<CharacterStat>().id)
                    return true;
            }

            return false;
        }

    }
}

