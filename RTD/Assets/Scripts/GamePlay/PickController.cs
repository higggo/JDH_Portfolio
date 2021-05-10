using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class PickController : MonoBehaviour
{
    enum STATE
    {
        Normal,
        MouseButtonDown,
        MouseButtonDragging,
        MouseButtonUp,
        Disable
    }
    STATE state = STATE.Normal;

    public GameObject PickUpObject = null;
    Vector3 OriginPos = Vector3.zero;
    TileManager TileManager;
    CharacterInfoManager CharacterInfoManager;
    public VoidDelGameObject UpgradeDelegate = null;

    //
    public AudioClip Audio_Pickup;
    public AudioClip Audio_Pickdown;
    public AudioClip Audio_UpGold;
    public AudioClip Audio_UpGrade;

    //BossZoneWarp BossZoneWarp;

    // LJH : 드래그 시 파는 영역 계산을 위한 화면 높이 비율값
    float ratio;
    // LJH : 합칠 수 있는 캐릭터 List
    List<GameObject> possibleSumList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        TileManager = GetComponent<TileManager>();
        CharacterInfoManager = GetComponent<CharacterInfoManager>();
        //BossZoneWarp = GameObject.Find("BossWarp").GetComponent<BossZoneWarp>();
        // LJH : 팔기 로직을 위한 rect 비율 계산
        Rect bound;
        GameObject UICharacterPicker = GameObject.Find("Canvas").transform.Find("CharacterPickerUI").gameObject;
        bound = UICharacterPicker.GetComponent<RectTransform>().rect;
        bound.width *= GameObject.Find("Canvas").transform.lossyScale.x * UICharacterPicker.transform.localScale.x;
        bound.height *= GameObject.Find("Canvas").transform.lossyScale.y * UICharacterPicker.transform.localScale.y;
        ratio = bound.height / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    public void Init()
    {
        PickUpObject = null;
        OriginPos = Vector3.zero;
        ChangeState(STATE.Normal);
    }
    void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch (state)
        {
            case STATE.Normal:
                break;
            case STATE.MouseButtonDown:
                if (PickUpObject != null)
                {
                    OriginPos = PickUpObject.transform.position;

                    // LJH: sum effect 파티클 disable -> enable
                    possibleSumList = TileManager.GetAllCharacters();
                    if (possibleSumList.Contains(PickUpObject))
                        possibleSumList.Remove(PickUpObject);

                    if (possibleSumList.Count != 0)
                    {
                        foreach (GameObject obj in possibleSumList)
                        {
                            if (CharUtils.CompareID(PickUpObject, obj))
                            {
                                GameObject effect = obj.transform.Find("SumEffect").gameObject;
                                effect?.SetActive(true);
                            }
                        }
                    }
                }
                break;
            case STATE.MouseButtonDragging:
                if (PickUpObject != null)
                {
                    TileManager.AllAppear();
                }
                break;
            case STATE.MouseButtonUp:
                if (PickUpObject != null)
                {
                    // BossZone Warp
                    if (TileManager.IsBossTile())
                    {
                        Transform parent = TileManager.GetBossTile();
                        if (parent != null)
                        {
                            PickUpObject.transform.parent = parent;
                            PickUpObject.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            Debug.Log("BossZone Full");
                            PickUpObject.transform.position = OriginPos;
                        }
                    }
                    // Return Warp to Storage or Ground
                    else if (TileManager.IsReturnTile())
                    {
                        Transform parent = TileManager.GetEmptyStorageTile();
                        if (parent == null)
                            parent = TileManager.GetEmptyGroundTile();
                        if (parent != null)
                        {
                            PickUpObject.transform.parent = parent;
                            PickUpObject.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            Debug.Log("BossZone Full");
                            PickUpObject.transform.position = OriginPos;
                        }
                    }
                    // 합치기 or 이동
                    else if (TileManager.GetPossibleTileCount() > 0)
                    {
                        Transform targetTile = TileManager.GetClosestTile(PickUpObject.transform.position);
                        GameObject target = targetTile.childCount > 0 ? targetTile.GetChild(0).gameObject : null;

                        // 합치기
                        if (CharacterInfoManager.IsUpgradeTaret(PickUpObject, target))
                        {
                            // LJH : possibleSumList에서 target 제거해주기
                            if (possibleSumList.Contains(target))
                                possibleSumList.Remove(target);

                            // Success
                            UpgradeCharacter(PickUpObject.transform.gameObject, target);
                            SoundManager.I.PlayEffectSound(Audio_UpGrade);
                        }
                        //이동
                        else if (target == null)
                        {
                            PickUpObject.transform.parent = targetTile;
                            PickUpObject.transform.localPosition = Vector3.zero;
                            SoundManager.I.PlayEffectSound(Audio_Pickdown);
                        }
                        // 서로 자리 바꾸기
                        else
                        {
                            Transform myTile = PickUpObject.transform.parent;
                            PickUpObject.transform.parent = targetTile;
                            PickUpObject.transform.localPosition = Vector3.zero;

                            target.transform.parent = myTile;
                            target.transform.localPosition = Vector3.zero;
                            SoundManager.I.PlayEffectSound(Audio_Pickdown);
                        }
                    }
                    // LJH : 팔기
                    else if (Input.mousePosition.y <= Screen.height * ratio)
                    {
                        int grade = (int)PickUpObject.GetComponent<CharController>().statInfo.grade;
                        float refund = (float)Mathf.Pow(2.0f, grade) * 100.0f;
                        refund *= 0.75f;
                        ResponseMessage.Trade.CODE response = new ResponseMessage.Trade.CODE();
                        GetComponent<MoneyManager>()?.CalculateMoney(MoneyManager.ACTION.Receive, (uint)refund, response, "Character Refund");
                        Destroy(PickUpObject.gameObject);
                        SoundManager.I.PlayEffectSound(Audio_UpGold);
                    }
                    else
                    {
                        PickUpObject.transform.position = OriginPos;
                    }
                    TileManager.AllHide();
                    PickUpObject = null;

                    // LJH: sum effect 파티클 enable->disable
                    if (possibleSumList.Count != 0)
                    {
                        foreach (GameObject obj in possibleSumList)
                        {
                            GameObject effect = obj.transform.Find("SumEffect").gameObject;
                            effect?.SetActive(false);
                        }
                    }
                    possibleSumList.Clear();
                }
                break;
            case STATE.Disable:
                break;
        }
    }
    void StateProcess()
    {
        switch (state)
        {
            case STATE.Normal:
                // Control + 클릭 : 합치기
                if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
                {
                    Debug.Log("Control + 클릭 : 합치기");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.tag == "Player")
                        {
                            if (UpgradeCharacter(hits[i].transform.gameObject, null))
                            {
                                SoundManager.I.PlayEffectSound(Audio_UpGrade);
                                // Success
                            }
                            else
                            {
                                // Fail
                            }
                        }
                    }
                }

                // Alt + 클릭 : 자동 옮기기
                else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
                {
                    Debug.Log("Alt + 클릭 : 자동 옮기기");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.tag == "Player")
                        {
                            Transform parent = TileManager.GetEmptyOtherFieldTile(hits[i].transform);
                            if (parent == null)
                            {
                                Debug.Log("이동할 곳이 가득찼습니다.");
                                break;
                            }
                            hits[i].transform.parent = parent;
                            hits[i].transform.localPosition = Vector3.zero;
                            //GetComponent<CharacterInfoManager>().UpdateCharacterField(hits[i].transform.gameObject);
                            SoundManager.I.PlayEffectSound(Audio_Pickdown);
                        }
                    }
                }

                // 좌클릭
                else if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 500.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.tag == "Player")
                        {
                            Debug.Log("캐릭터 좌클릭");
                            PickUpObject = hits[i].transform.gameObject;
                            SoundManager.I.PlayEffectSound(Audio_Pickup);
                        }
                    }
                    ChangeState(STATE.MouseButtonDown);
                }

                // D버튼 클릭 (간편 팔기)
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.transform.tag == "Player")
                        {
                            Debug.Log("캐릭터 팔기");
                            int grade = (int)hit.transform.gameObject.GetComponent<CharController>().statInfo.grade;
                            float refund = (float)Mathf.Pow(2.0f, grade) * 100.0f;
                            refund *= 0.75f;
                            ResponseMessage.Trade.CODE response = new ResponseMessage.Trade.CODE();
                            GetComponent<MoneyManager>()?.CalculateMoney(MoneyManager.ACTION.Receive, (uint)refund, response, "Character Refund");
                            SoundManager.I.PlayEffectSound(Audio_UpGold);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
                break;
            case STATE.MouseButtonDown:
                ChangeState(STATE.MouseButtonDragging);
                break;
            case STATE.MouseButtonDragging:
                if (PickUpObject != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50.0f, (~(1 << LayerMask.NameToLayer("UI")) & (1 << LayerMask.NameToLayer("Map")))))
                    {
                        PickUpObject.transform.position = new Vector3(
                            hit.point.x,
                            PickUpObject.transform.position.y,
                            hit.point.z
                        );
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    ChangeState(STATE.MouseButtonUp);
                }
                
                break;
            case STATE.MouseButtonUp:
                ChangeState(STATE.Normal);
                break;

            case STATE.Disable:
                break;
        }
    }

    public void SetDisable()
    {
        if (PickUpObject != null)
        {
            PickUpObject.transform.position = OriginPos;
            TileManager.AllHide();
            PickUpObject = null;
        }
        ChangeState(STATE.Disable);
    }

    public void SetNormal()
    {
        ChangeState(STATE.Normal);
    }

    public bool UpgradeCharacter(GameObject obj, GameObject target)
    {
        bool rt = false;
        Transform parent;
        if (target == null)
        {
            target = CharacterInfoManager.GetUpgradeTarget(obj);
            parent = obj.transform.parent;
        }
        else
        {
            parent = target.transform.parent;
        }

        if (target != null)
        {
            string[] charList = CharacterInfoManager.GetGradeUnionCharacters(CharacterInfoManager.GetNextGrade(CharUtils.SetCharacterGrade(obj.GetComponent<CharController>().statInfo.id)), obj.GetComponent<CharController>().statInfo.union);
            GameObject upgradeCharacter = Instantiate(Resources.Load(charList[Random.Range(0, charList.Length)])) as GameObject;
            upgradeCharacter.transform.parent = parent;
            upgradeCharacter.transform.localPosition = Vector3.zero;
            UpgradeDelegate?.Invoke(upgradeCharacter);
            GetComponent<LevelUpManager>().UpdateCharacterLevel(upgradeCharacter);
            Destroy(obj);
            Destroy(target);
            rt = true;
        }
        else
        {
            rt = false;
        }
        return rt;
    }
}
