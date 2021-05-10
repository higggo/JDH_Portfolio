using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class EffectDamageTick : EffectDamage
{
    [SerializeField] float TickTime = 0.0f;
    List<GameObject> inRange = new List<GameObject>();
    float checkTickTime;
    public override void Init(LayerMask mask, float damage, float effectTime = 1)
    {
        base.Init(mask, damage, effectTime);
        if (TickTime == 0.0f)
            TickTime = 0.1f;
        
        StartCoroutine(StartTimer());
    }

    protected IEnumerator StartTimer()
    {
        float dmg = (TickTime / EffectTime) * damage;
        FDamageMessage msg;
        msg.Causer = this.gameObject;
        msg.amount = (int)dmg;
        List<GameObject> RemovedEnemy = new List<GameObject>();
        yield return new WaitForEndOfFrame();

        while (EffectTime > Mathf.Epsilon)
        {
            CheckingForRemoveList(ref RemovedEnemy);

            if (checkTickTime <= Mathf.Epsilon)
            {
                foreach (GameObject enemy in inRange)
                {
                    if (enemy.GetComponent<Damageable>() == null
                        || enemy.GetComponent<Damageable>().IsDead)
                        continue;
                    
                    enemy.GetComponent<Damageable>().GetDamage(msg);
                }
                checkTickTime = TickTime;
            }
            EffectTime -= Time.deltaTime;
            checkTickTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }

    void CheckingForRemoveList(ref List<GameObject> trashContainer)
    {
        trashContainer.Clear();
        foreach (GameObject enemy in inRange)
        {
            if (enemy.GetComponent<Damageable>() == null
                || enemy.GetComponent<Damageable>().IsDead)
                trashContainer.Add(enemy);
        }

        if (trashContainer.Count != 0)
        {
            foreach (GameObject enemy in trashContainer)
                inRange.Remove(enemy);
        }
    }

    protected virtual void AddToList(Collider other)
    {
        if (enemyLayer == other.gameObject.layer)
            inRange.Add(other.gameObject);
    }

    protected virtual void RemoveFromList(Collider other)
    {
        if (enemyLayer == other.gameObject.layer)
            inRange.Remove(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        AddToList(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromList(other);
    }

}
