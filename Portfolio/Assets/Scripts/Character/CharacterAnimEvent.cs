using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimEvent : MonoBehaviour
{
    public VoidDelVoid Attack = null;
    public UnityAction JumpStart = null;
    public UnityAction JumpEnd = null;

    void OnAttack()
    {
        Attack?.Invoke();
    }

    void OnJump()
    {
        JumpStart?.Invoke();
    }

    void OnJumpEnd()
    {
        JumpEnd?.Invoke();
    }
}
