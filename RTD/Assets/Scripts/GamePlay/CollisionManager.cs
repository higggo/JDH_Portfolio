using System;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Serializable]
    public struct IgnoreCollisionSet
    {
        public Collider collider1;
        public Collider collider2;
    };
    [SerializeField]
    public IgnoreCollisionSet[] IgnoreCollision;

    // Start is called before the first frame update
    void Start()
    {
        foreach(IgnoreCollisionSet set in IgnoreCollision)
        {
            Physics.IgnoreCollision(set.collider1, set.collider2);
        }
    }

// Update is called once per frame
void Update()
    {
        
    }
}
