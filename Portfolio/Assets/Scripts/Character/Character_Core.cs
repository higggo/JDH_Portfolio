using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public abstract class Character_Core : MonoBehaviour, BattleSystem
{
    public abstract void ApplyDamage(GameObject target, float damage);
    public abstract void StartChasing();
    public abstract void StopChasing();
    public abstract void ApplyHP();
    public abstract void SetMyCharacter(string uid);
    public string UID;
    public string CID;
    public Camera Camera;
    public TMPro.TextMeshProUGUI GUI_ID;
    public CharacterStatus Status;
    public PathFinding PathFinding;
    public CharacterAnimEvent AnimEvent;
    public Animator Animator;
    public Rigidbody Rigidbody;
    public UnityAction ReserveAction;
    public Coroutine ChasingCoroutine;

    public GameObject AttackTarget;

    public DatabaseReference Reference;

    public Dictionary<string, GameObject> ProjectileTargets;

    public List<string> ChasedObjects;

    protected void Start()
    {
        ChasedObjects = new List<string>();
    }
    protected void Update()
    {
        
    }
    public virtual void Initialize(string uid, string cid)
    {

        gameObject.name = cid;
        CID = cid;
        UID = uid;
        GUI_ID.text = cid;
        Reference = FirebaseDatabase.DefaultInstance.GetReference("Destination").Child("Town").Child("Users").Child(uid).Child(cid);

        transform.Find("CharacterCanvas").GetComponent<Billboard>().cam = Camera.main.transform;
        SetMyCharacter(uid);
        Status.Initialize();

        ApplyHP();
        Status.HP.OnUpdate += () =>{ InitHPUpdate(); };
        Status.Destination.OnUpdate += () => { InitDestinationUpdate(); };
        Status.ChasingTarget.OnUpdate += () => { InitChasingTargetUpdate(); };
        Status.AttackTarget.OnUpdate += () => { InitAttackTargetUpdate(); };
        Status.AmountDamageRecived.OnUpdate += () => { InitAmountDamageRecivedUpdate(); };
    }

    public void Logout()
    {
        Status.StopAllListener();
        Destroy(gameObject);
    }

    public virtual void ProjectileHit(GameObject target, float damage)
    {

    }
    public virtual void Hit()
    {

    }

    public virtual void Damaged(GameObject target, float damage)
    {
        Debug.Log("Damaged");
        Animator.SetTrigger("Damaged");
        ApplyDamage(target, damage);
    }
    public bool IsDead()
    {
        return false;
    }
    public virtual void InitHPUpdate()
    {
        string currentHP;
        if (Status.HP.GetValue() <= 0.0f)
        {
            Dead();
            currentHP = "0";
        }
        else
        {
            currentHP = Status.HP.GetValue().ToString();
        }
        string hpBar = currentHP + "/" + Status.MaxHP.ToString();
        Debug.Log(CID + " HP : " + hpBar);
        //Canvas.HP.text = hpBar;
    }
    public virtual void InitDestinationUpdate()
    {
    }
    public virtual void InitChasingTargetUpdate()
    {
    }
    public virtual void InitAttackTargetUpdate()
    {
    }
    public virtual void InitAmountDamageRecivedUpdate()
    {
    }
    public virtual void Dead()
    {

    }
public void DoReserveAction()
    {
        ReserveAction?.Invoke();
        ReserveAction = null;
    }

}
