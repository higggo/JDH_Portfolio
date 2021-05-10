using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMenu : MonoBehaviour
{
    public GameObject Character = null;
    public GameObject Tile = null;

    public GameObject UICardPick = null;
    public GameObject UILevelUp = null;
    public GameObject UITop = null;

    public TileManager TileManager = null;

    public GameObject SpawnCharacter;
    // Start is called before the first frame update
    void Start()
    {
        if (UICardPick == null) UICardPick = GameObject.Find("CharacterPickerUI");
        if (UILevelUp == null) UILevelUp = GameObject.Find("LevelUp");
        if (UITop == null) UITop = GameObject.Find("TopUI");
        if (TileManager == null) TileManager = GameObject.Find("GamePlayManager").GetComponent<TileManager>();
        if (Tile == null) Tile = GameObject.Find("Ground").transform.Find("tile1").gameObject;
    }

    public void Create()
    {
        SpawnCharacter = Instantiate(Character) as GameObject;
        SpawnCharacter.transform.parent = Tile.transform;
        SpawnCharacter.transform.localPosition = Vector3.zero;
        SpawnCharacter.transform.Find("HPBar").gameObject.SetActive(false);
        SpawnCharacter.transform.Find("Skill").gameObject.SetActive(false);
        gameObject.SetActive(true);
        UICardPick.SetActive(false);
        UILevelUp.SetActive(false);
        UITop.SetActive(false);
    }
    public void Disable()
    {
        Destroy(SpawnCharacter);
        UICardPick.SetActive(true);
        UILevelUp.SetActive(true);
        UITop.SetActive(true);
    }
}
