using Firebase.Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterStatus : MonoBehaviour
{
    public float CurrentSpeed = 0.0f;
    public float MaxSpeed = 0.0f;
    public float AttackSpeed = 3.0f;
    public float AttackRange = 3.0f;
    public float damage = 30.0f;
    public float MaxHP = 100.0f;
    public float CurrentHP = 100.0f;

    Monster Monster;
    public List<Action> AllAddListener;
    public List<Action> AllRemoveListener;
    public StatContainer<float> HP;
    public StatContainer<float> MP;
    public StatContainer<string> BehaviorState;
    public StatContainer<Vector3> Destination;
    public StatContainer<string> AttackTarget;
    public StatContainer<string> ChasingTarget;
    public StatContainer<float> AmountDamageRecived;
    public StatContainer<bool> Spawn;
    public StatContainer<bool> IsChasing;
    private void Awake()
    {
        Debug.Log("MonsterStatus");
    }
    public void Initialize()
    {
        Monster = GetComponent<Monster>();
        AllAddListener = new List<Action>();
        AllRemoveListener = new List<Action>();
        SetAttribute(ref HP, Monster.Reference.Child(nameof(HP)));
        SetAttribute(ref MP, Monster.Reference.Child(nameof(MP)));
        SetAttribute(ref BehaviorState, Monster.Reference.Child(nameof(BehaviorState)));
        SetAttribute(ref Destination, Monster.Reference.Child(nameof(Destination)));
        SetAttribute(ref AttackTarget, Monster.Reference.Child(nameof(AttackTarget)));
        SetAttribute(ref ChasingTarget, Monster.Reference.Child(nameof(ChasingTarget)));
        SetAttribute(ref AmountDamageRecived, Monster.Reference.Child(nameof(AmountDamageRecived)));
        SetAttribute(ref Spawn, Monster.Reference.Child(nameof(Spawn)));
        SetAttribute(ref IsChasing, Monster.Reference.Child(nameof(IsChasing)));

        StartAllListener();

        IsChasing.SetValue(false);
    }
    public void SetAttribute<T>(ref StatContainer<T> attr, DatabaseReference reference)
    {
        attr = new StatContainer<T>();
        attr.Reference(reference);
        AllAddListener.Add(attr.AddListener);
        AllRemoveListener.Add(attr.RemoveListener);
    }
    public void StartAllListener()
    {
        foreach (Action listener in AllAddListener)
        {
            listener();
        }
    }
    public void StopAllListener()
    {
        foreach (Action listener in AllRemoveListener)
        {
            listener();
        }
    }
}