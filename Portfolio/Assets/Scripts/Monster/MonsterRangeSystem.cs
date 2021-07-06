using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidDelVoid();

public class MonsterRangeSystem : MonoBehaviour
{
    public string Enemy = "Player";
    public VoidDelVoid battle;
    public Transform Target = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        // Layer로 찾기
        //if (LayerMask.LayerToName(other.gameObject.layer) == Enemy);

        // Tag로 찾기
        if (other.gameObject.tag == "Player")
        {
            Target = other.transform;
            //if(battle != null)
            battle?.Invoke();
            //this.GetComponent<Monster>().ChangeState(Monster.STATE.BATTLE);
        }
    }

}
