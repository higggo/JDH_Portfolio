using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAnimEvent : MonoBehaviour
{
    public VoidDelVoid Attack = null;
    public UnityAction JumpStart = null;
    public UnityAction JumpEnd = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
