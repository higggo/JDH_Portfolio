using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Transform GroundSpace = null;
    public Transform StorageSpace = null;
    public Transform BossSpace = null;
    public GameObject BossWarpTile = null;
    public GameObject ReturnWarpTile = null;

    // Start is called before the first frame update
    void Start()
    {
        if (GroundSpace == null) GroundSpace = GameObject.Find("Ground").transform.Find("Space").transform;
        if (StorageSpace == null) StorageSpace = GameObject.Find("Storage").transform.Find("Space").transform;
        if (BossSpace == null) BossSpace = GameObject.Find("FieldMap").transform.Find("Space").transform;
        if (BossWarpTile == null) BossWarpTile = GameObject.Find("BossWarpTile");
        if (ReturnWarpTile == null) ReturnWarpTile = GameObject.Find("ReturnWarp");
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void Init()
    {
        DestroyAllCharacter();
    }
    public Transform GetClosestTile(Vector3 pos)
    {
        Transform closestTile = null;
        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible)
            {
                if (closestTile == null)
                {
                    closestTile = GroundSpace.GetChild(i);
                    continue;
                }
                if (Vector3.Distance(GroundSpace.GetChild(i).position, pos) < Vector3.Distance(closestTile.position, pos))
                {
                    closestTile = GroundSpace.GetChild(i);
                }
            }
        }
        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible)
            {
                if (closestTile == null)
                {
                    closestTile = StorageSpace.GetChild(i);
                    continue;
                }
                if (Vector3.Distance(StorageSpace.GetChild(i).position, pos) < Vector3.Distance(closestTile.position, pos))
                {
                    closestTile = StorageSpace.GetChild(i);
                }
            }
        }
        //if (BossWarpTile.GetComponent<Tile>().State == Tile.STATE.Possible)
        //{
        //    if (closestTile == null)
        //    {
        //        closestTile = BossWarpTile.transform;
        //    }
        //    if (Vector3.Distance(BossWarpTile.transform.position, pos) < Vector3.Distance(closestTile.position, pos))
        //    {
        //        closestTile = BossWarpTile.transform;
        //    }
        //}
            
        return closestTile;
    }

    public int GetPossibleTileCount()
    {
        int cnt = 0;
        foreach (Transform child in GroundSpace)
        {
            if (child.GetComponent<Tile>().State == Tile.STATE.Possible)
                cnt++;
        }
        foreach (Transform child in StorageSpace)
        {
            if (child.GetComponent<Tile>().State == Tile.STATE.Possible)
                cnt++;
        }
        //if (BossWarpTile.GetComponent<Tile>().State == Tile.STATE.Possible)
        //    cnt++;

        return cnt;
    }

    public void AllHide()
    {
        foreach (Transform child in GroundSpace)
        {
            child.GetComponent<Tile>().State = Tile.STATE.Hide;
        }
        foreach (Transform child in StorageSpace)
        {
            child.GetComponent<Tile>().State = Tile.STATE.Hide;
        }
        BossWarpTile.GetComponent<Tile>().State = Tile.STATE.Hide;
        ReturnWarpTile.GetComponent<Tile>().State = Tile.STATE.Hide;
    }
    public void AllAppear()
    {
        foreach (Transform child in GroundSpace)
        {
            child.GetComponent<Tile>().AppearTile();
        }
        foreach (Transform child in StorageSpace)
        {
            child.GetComponent<Tile>().AppearTile();
        }
        BossWarpTile.GetComponent<Tile>().AppearTile();
        ReturnWarpTile.GetComponent<Tile>().AppearTile();
    }

    public int GetCountStorageCharacter()
    {
        int cnt = 0;
        foreach (Transform child in StorageSpace)
        {
            if (child.childCount > 0)
                cnt++;
        }
        return cnt;
    }
    public int GetCountGroundCharacter()
    {
        int cnt = 0;
        foreach (Transform child in GroundSpace)
        {
            if (child.childCount > 0)
                cnt++;
        }
        return cnt;
    }

    // Storage <--> Ground
    public Transform GetEmptyOtherFieldTile(Transform obj)
    {
        Transform emptyTile = null;
        if (obj.parent.parent.parent.name == GroundSpace.parent.name)
        {
            foreach (Transform child in StorageSpace)
            {
                if (child.childCount == 0)
                    emptyTile = child;
            }
        }
        else if (obj.parent.parent.parent.name == StorageSpace.parent.name)
        {
            foreach (Transform child in GroundSpace)
            {
                if (child.childCount == 0)
                    emptyTile = child;
            }
        }
        return emptyTile;
    }

    public void DestroyAllCharacter()
    {
        foreach (Transform child in GroundSpace)
        {
            if(child.childCount > 0) Destroy(child.GetChild(0).gameObject);
        }
        foreach (Transform child in StorageSpace)
        {
            if (child.childCount > 0) Destroy(child.GetChild(0).gameObject);
        }
        foreach (Transform child in BossSpace)
        {
            if (child.childCount > 0) Destroy(child.GetChild(0).gameObject);
        }
    }

    public Transform GetBossTile()
    {
        Transform bossTile = null;

        foreach (Transform child in BossSpace)
        {
            if (child.childCount == 0)
            {
                bossTile = child;
                break;
            }
        }
        return bossTile;
    }

    public bool IsBossTile()
    {
        bool result = false;
        if (BossWarpTile.GetComponent<Tile>().State == Tile.STATE.Possible)
        {
            result = true;
        }
        return result;
    }
    public bool IsReturnTile()
    {
        bool result = false;
        if (ReturnWarpTile.GetComponent<Tile>().State == Tile.STATE.Possible)
        {
            result = true;
        }
        return result;
    }
    public Transform GetEmptyGroundTile()
    {
        Transform result = null;

        foreach (Transform child in GroundSpace)
        {
            if (child.childCount == 0)
            {
                result = child;
                break;
            }
        }
        return result;
    }
    public Transform GetEmptyStorageTile()
    {
        Transform result = null;
        foreach (Transform child in StorageSpace)
        {
            if (child.childCount == 0)
            {
                result = child;
                break;
            }
        }
        return result;
    }
    public Transform GetEmptyBossTile()
    {
        Transform result = null;
        foreach (Transform child in BossSpace)
        {
            if (child.childCount == 0)
            {
                result = child;
                break;
            }
        }
        return result;
    }

    // LJH: 모든 캐릭터 가져오기
    public List<GameObject> GetAllCharacters()
    {
        List<GameObject> characters = new List<GameObject>();
        foreach (Transform child in StorageSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }

        foreach (Transform child in GroundSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }
        return characters;
    }

    public List<GameObject> GetStorageCharacters()
    {
        List<GameObject> characters = new List<GameObject>();

        foreach (Transform child in StorageSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }
        return characters;
    }
    public List<GameObject> GetGroundCharacters()
    {
        List<GameObject> characters = new List<GameObject>();

        foreach (Transform child in GroundSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }
        return characters;
    }
    public List<GameObject> GetBossCharacters()
    {
        List<GameObject> characters = new List<GameObject>();

        foreach (Transform child in BossSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }
        return characters;
    }
    public List<GameObject> GetBossGroundCharacters()
    {
        List<GameObject> characters = new List<GameObject>();

        foreach (Transform child in GroundSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }

        foreach (Transform child in BossSpace)
        {
            if (child.childCount > 0)
            {
                characters.Add(child.GetChild(0).gameObject);
            }
        }
        return characters;
    }
}
