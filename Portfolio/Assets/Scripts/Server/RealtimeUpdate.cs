using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeUpdate : RealtimeUpdate_Core
{
    float clickTimer = 0.0f;
    float clickInterval = 0.25f;
    bool clickPossible = true;
    private new void Start()
    {
        //FirebaseDatabase.DefaultInstance.GetReference("Destination").Child("Town").Child("Users").ValueChanged += (sender, task)=>{ Debug.Log("ccccccccc"); };
        base.Start();
    }
    private new void Update()
    {
        base.Update();
    }
    public override void SetCharacterCamera(GameObject target, string uid) { }
    public override void CleanDatabase()
    {
        RDConnection.Write.RemoveCharacter();
        RDConnection.Write.RemoveDestination();
    }
    public override void SetDestination()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Units.MyCharacter.GetComponent<Character>().Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999.0f, ClickMask | (1 << LayerMask.NameToLayer("Monster"))))
            {
                if(clickPossible)
                {
                    Debug.Log("Mouse Click");
                    if (hit.transform.tag == "Monster")
                    {
                        // Attack Target
                        Units.MyCharacter.GetComponent<CharacterStatus>().ChasingTarget.SetValue(hit.transform.gameObject.GetComponent<Monster>().ID);
                    }
                    else
                    {
                        Units.MyCharacter.GetComponent<CharacterStatus>().Destination.SetValue(hit.point);
                        if (Units.MyCharacter.GetComponent<CharacterStatus>().ChasingTarget.GetValue() != "")
                            Units.MyCharacter.GetComponent<CharacterStatus>().ChasingTarget.SetValue("");
                    }
                    StartCoroutine(ClickRestrictor());
                }
            }
        }
    }
    public IEnumerator ClickRestrictor()
    {
        clickPossible = false;
        while(clickTimer < clickInterval)
        {
            clickTimer += Time.deltaTime;
            yield return null;
        }
        clickTimer = 0.0f;
        clickPossible = true;
    }
}
