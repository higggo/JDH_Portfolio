using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
public class CreateCharacterInfo
{
    public GameObject Character;
    public string path;
    public int slot;
    public string cid;
}
public class CreateCharacters : MonoBehaviour
{
    DatabaseReference reference;
    FirebaseAuth auth;
    FirebaseUser user;
    FirebaseFirestore db;

    public Transform CollectionPool;
    public Transform MyCharacterPool;
    public TMPro.TMP_InputField ID_Inputfield;

    public List<CreateCharacterInfo> CollectionCharacters;

    // Camera
    Camera mainCamera;
    Vector3 CameraCollectionPos = new Vector3(6.86f, 1.49f, 2.68f);
    Vector3 CameraCreatePos = new Vector3(-0.120f, 1.49f, 1.57f);

    int selectNum = 0;
    bool identifyActive = false;
    // Start is called before the first frame update
    void Start()
    {

        CollectionCharacters = new List<CreateCharacterInfo>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        if (CollectionPool == null) CollectionPool = GameObject.Find("CollectionPool").transform;
        if (MyCharacterPool == null) MyCharacterPool = GameObject.Find("MyCharacterPool").transform;
        if (ID_Inputfield == null) ID_Inputfield = GameObject.Find("ID_InputField").GetComponent<TMPro.TMP_InputField>();

        mainCamera = Camera.main;
        MoveCollectionPos();
        //DirectoryInfo dir = new DirectoryInfo("Assets/Resources/BasicCharacters");
        //FileInfo[] info = dir.GetFiles("*.prefab");

        List<string> BasicCharacters = new List<string>();
        BasicCharacters.Add("Archer");
        BasicCharacters.Add("Warrior");
        BasicCharacters.Add("Wizard");
        foreach (string str in BasicCharacters)
        {
            CreateCharacterInfo collection = new CreateCharacterInfo();
            collection.Character = Instantiate(Resources.Load("SlotCharacter/"+str), CollectionPool) as GameObject;
            collection.Character.SetActive(false);
            collection.path = str;
            CollectionCharacters.Add(collection);
        }
        CollectionCharacters[selectNum].Character.SetActive(true);

        GetCharacters();
    }

    public void LogOut()
    {
        Debug.Log("LogOut");
        reference.Child("Users").Child(user.UserId).RemoveValueAsync();
    }

    public void NextCharacter()
    {
        CollectionCharacters[selectNum].Character.SetActive(false);
        selectNum++;
        if (selectNum > CollectionCharacters.Count - 1)
            selectNum = 0;
        CollectionCharacters[selectNum].Character.SetActive(true);
    }

    public void PreCharacter()
    {
        CollectionCharacters[selectNum].Character.SetActive(false);
        selectNum--;
        if (selectNum < 0)
            selectNum = CollectionCharacters.Count - 1;
        CollectionCharacters[selectNum].Character.SetActive(true);
    }

    public void CreateCharacter()
    {
        Dictionary<string, object> user = new Dictionary<string, object>
        {
            { "ResourcePath", CollectionCharacters[selectNum].path },
            { "UID", auth.CurrentUser.UserId },
            { "CID", ID_Inputfield.text },
            { "State", "Idle" },
            { "MaxHP", CollectionCharacters[selectNum].Character.GetComponent<CharacterStatus>().MaxHP },
            { "CurrentHP", CollectionCharacters[selectNum].Character.GetComponent<CharacterStatus>().CurrentHP },
            { "Location", "Town"},
            { "Pos", new Dictionary<string, object>
                {
                        { "X", 0.0f },
                        { "Y", 1.0f },
                        { "Z", 0.0f }
                }
            }
        };
        db.Collection("Users").AddAsync(user).ContinueWithOnMainThread(task => {
            DocumentReference addedDocRef = task.Result;
            Debug.Log(string.Format("Added document with ID: {0}.", addedDocRef.Id));
            Debug.Log("Create User Characater");
            Debug.Log("ResourcePath : " + CollectionCharacters[selectNum].path);
            Debug.Log("uid : " + auth.CurrentUser.UserId);
            Debug.Log("cid : " + ID_Inputfield.text);
        });
    }

    public IEnumerator IdentifyIDField()
    {
        List<bool> ingLIst = new List<bool>();
        bool possibleCreate = true;
        string cid = ID_Inputfield.text;
        if (cid == "")
        {
            Debug.Log("아이디를 입력해 주세요.");
            possibleCreate = false;
        }

        ingLIst.Add(true);
        Firebase.Firestore.Query allCitiesQuery = db.Collection("Users").WhereEqualTo("CID", cid);
        allCitiesQuery.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshotTask =>
        {
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                Debug.Log("해당 아이디가 존재합니다.");
                possibleCreate = false;
                break;
            }
            ingLIst[0] = false;
        });

        ingLIst.Add(true);
        Firebase.Firestore.Query capitalQuery = db.Collection("Users").WhereEqualTo("UID", auth.CurrentUser.UserId);
        capitalQuery.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            QuerySnapshot capitalQuerySnapshot = task.Result;
            if (capitalQuerySnapshot.Count >= 10)
            {
                Debug.Log("슬롯이 가득 찼습니다.");
                possibleCreate = false;
            }
            ingLIst[1] = false;
        });

        // Waiting Server Response
        bool bEnd = false;
        while (!bEnd)
        {
            foreach (bool ing in ingLIst)
            {
                if (ing)
                {
                    bEnd = false;
                    break;
                }
                else bEnd = true;
            }
            yield return null;
        }
        if (possibleCreate)
        {
            CreateCharacter();
            GetCharacters();
            MoveCollectionPos();
        }

        ingLIst.Clear();
        identifyActive = false;
    }
    public void CompareID()
    {
        if(!identifyActive)
        {
            identifyActive = true;
            StartCoroutine(IdentifyIDField());
        }
        else
        {
            Debug.Log("응답 대기 중...");
        }
    }

    public void GetCharactersCallBack(Task<QuerySnapshot> task)
    {
        QuerySnapshot capitalQuerySnapshot = task.Result;
        foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
        {
            Dictionary<string, object> info = documentSnapshot.ToDictionary();

            CreateCharacterInfo myCharacter = new CreateCharacterInfo();
            foreach (KeyValuePair<string, object> pair in info)
            {
                if (pair.Key == "ResourcePath")
                {
                    myCharacter.path = pair.Value.ToString();
                }
                if (pair.Key == "CID")
                {
                    myCharacter.cid = pair.Value.ToString();
                }
            }
            Transform slot = GetSlot();
            GameObject slotCharacter = Instantiate(Resources.Load("SlotCharacter/" + myCharacter.path), slot) as GameObject;
            GameObject canvas = Instantiate(Resources.Load("CharacterCanvas"), slotCharacter.transform) as GameObject;
            canvas.transform.Find("ID").GetComponent<TMPro.TextMeshProUGUI>().text = myCharacter.cid;

            GetSelectButton(slot).onClick.AddListener(() => {
                RDConnection.Write.UpdateCharacterLocation(info, (res) => {
                    if (res.IsFaulted)
                    {
                        Debug.Log("UpdateCharacterLocation Faild");
                        return;
                    }
                    else if (res.IsCompleted)
                    {
                        FAuth.CID = myCharacter.cid;
                        FieldScene();
                    }
                });
            });
        };
    }
    public void GetCharacters()
    {
        // 초기화
        foreach(Transform t in MyCharacterPool)
        {
            foreach (Transform c in t.Find("Slot"))
            {
                Destroy(c.gameObject);
            }
        }
        FBConnection.Read.GetMyCharacterList(GetCharactersCallBack);
    }

    Transform GetSlot()
    {
        Transform t = transform;
        for (int i=0; i < MyCharacterPool.childCount; i++)
        {
            if(MyCharacterPool.GetChild(i).Find("Slot").childCount == 0)
            {
                MyCharacterPool.GetChild(i).Find("Canvas").gameObject.SetActive(true);
                t = MyCharacterPool.GetChild(i).Find("Slot");
                break;
            }
        }
        return t;
    }

    Button GetSelectButton(Transform slot)
    {
        return slot.parent.Find("Canvas").Find("SelectButton").GetComponent<Button>();
    }

    public void FieldScene()
    {
        SceneManager.LoadScene("Scenes/Field");
    }

    public void MoveCollectionPos()
    {
        mainCamera.transform.position = CameraCollectionPos;
    }

    public void MoveCreatePos()
    {
        mainCamera.transform.position = CameraCreatePos;
    }
}
