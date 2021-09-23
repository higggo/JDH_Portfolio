using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public float CurrentSpeed = 0.0f;
    public float MaxSpeed = 0.0f;
    public float AttackSpeed = 3.0f;
    public float AttackRange = 3.0f;
    public float damage = 30.0f;
    public float MaxHP = 100.0f;
    public float CurrentHP = 100.0f;

    Character Player;
    public List<Action> AllAddListener;
    public List<Action> AllRemoveListener;
    public StatContainer<float> HP;
    public StatContainer<float> MP;
    public StatContainer<string> BehaviorState;
    public StatContainer<Vector3> Destination;
    public StatContainer<string> AttackTarget;
    public StatContainer<string> ChasingTarget;
    public StatContainer<float> AmountDamageRecived;
    public StatContainer<bool> IsChasing;
    private void Awake()
    {
    }
    public void Initialize()
    {
        Player = GetComponent<Character>();
        AllAddListener = new List<Action>();
        AllRemoveListener = new List<Action>();
        SetAttribute(ref HP, Player.Reference.Child(nameof(HP)));
        SetAttribute(ref MP, Player.Reference.Child(nameof(MP)));
        SetAttribute(ref BehaviorState, Player.Reference.Child(nameof(BehaviorState)));
        SetAttribute(ref Destination, Player.Reference.Child(nameof(Destination)));
        SetAttribute(ref AttackTarget, Player.Reference.Child(nameof(AttackTarget)));
        SetAttribute(ref ChasingTarget, Player.Reference.Child(nameof(ChasingTarget)));
        SetAttribute(ref AmountDamageRecived, Player.Reference.Child(nameof(AmountDamageRecived)));
        SetAttribute(ref IsChasing, Player.Reference.Child(nameof(IsChasing)));

        StartAllListener();
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
        foreach(Action listener in AllAddListener)
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
