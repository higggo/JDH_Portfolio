using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public delegate void VoidDelGameObject(GameObject obj);
public class Storage : MonoBehaviour
{
    int SpaceNum;

    public VoidDelGameObject CreateCharacterDelegate;
    private void Awake()
    {
        SpaceNum = GetAllSpaceCount();
    }

    public void Push(GameObject CharacterPrefab)
    {
        foreach (Transform child in transform.Find("Space"))
        {
            if(child.childCount <= 0)
            {
                GameObject obj = Instantiate(CharacterPrefab) as GameObject;
                obj.transform.parent = child;
                obj.transform.localPosition = Vector3.zero;
                if(CreateCharacterDelegate != null)
                    CreateCharacterDelegate?.Invoke(obj);
                break;
            }
        }
    }

    int GetAllSpaceCount()
    {
        int num = 0;
        foreach (Transform child in transform.Find("Space"))
        {
                num++;
        }
        return num;
    }

    int GetEmptySpaceCount()
    {
        int num = 0;
        foreach (Transform child in transform.Find("Space"))
        {
            if (child.childCount == 0)
            {
                num++;
            }
        }
        return Mathf.Clamp(num, 0, SpaceNum);
    }

    public bool IsFull()
    {
        if (GetEmptySpaceCount() > 0)
            return false;
        else
            return true;
    }
}
