using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class EditorGameInfo : MonoBehaviour
{
    DatabaseReference reference;
    Button ActiveButton = null;
    bool DataSearching = false;
    DataSnapshot result;
    GameObject Contents;
    // Start is called before the first frame update
    void Start()
    {
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Contents = GameObject.Find("Contents");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitCategory()
    {
    }

    void GetDatas(string keyword)
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("GameInfo/" + keyword)
      .GetValueAsync().ContinueWith(task => {
          if (task.IsFaulted)
          {
              DataSearching = false;

          }
          else if (task.IsCompleted)
          {
              result = task.Result;
              DataSearching = false;
          }
      });
    }

    public void OnClickSystem(Button button)
    {
        SetActiveButton(button);
    }
    public void OnClickCharacter(Button button)
    {
        SetActiveButton(button);
    }
    public void OnClickSpawn(Button button)
    {
        SetActiveButton(button);
    }
    public void OnClickQuest(Button button)
    {
        SetActiveButton(button);
    }

    void SetActiveButton(Button button)
    {
        if (ActiveButton != null)
        {
            ActiveButton.interactable = true;
            string category = ActiveButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;
            Contents.transform.Find(category).gameObject.SetActive(false);
            foreach (Transform child in Contents.transform.Find(category))
            {
                Destroy(child.gameObject);
            }
        }
        ActiveButton = button;
        ActiveButton.interactable = false;

        switch(button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text)
        {
            case "System":
                StartCoroutine(SetSystemContents());
                break;
            case "Character":
                break;
            case "Spawn":
                break;
            case "Quest":
                break;
        }
    }
    GameObject GetContensItemCategory(string category)
    {
        GameObject obj = Contents.transform.Find(category).gameObject;
        return obj;
    }

    IEnumerator SetSystemContents()
    {
        GameObject parent = GetContensItemCategory("System");
        GetDatas("System");
        DataSearching = true;
        while (DataSearching)
        {
            yield return null;
        }
        foreach(DataSnapshot child in result.Children)
        {
            GameObject item = Instantiate(Resources.Load("EditorUI/SystemItem"), parent.transform) as GameObject;
            item.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = child.Key;

            List<string> options = new List<string>();
            int cnt = 0;
            foreach (DataSnapshot option in child.Children)
            {
                if(child.ChildrenCount-cnt > 5)
                {

                    FirebaseDatabase.DefaultInstance
                  .GetReference("GameInfo/System/" + child.Key + "/" + option.Key).RemoveValueAsync();
                    cnt++;
                    continue;
                }
                if (cnt == child.ChildrenCount-1)
                    item.transform.Find("Value").GetComponent<TMPro.TMP_InputField>().text = option.Value.ToString();
                options.Add(option.Value.ToString());
                cnt++;
            }
            item.transform.Find("OtherData").GetComponent<TMPro.TMP_Dropdown>().AddOptions(options);
            item.transform.Find("OtherData").GetComponent<TMPro.TMP_Dropdown>().value = cnt;
            item.transform.Find("OtherData").GetComponent<TMPro.TMP_Dropdown>().onValueChanged.AddListener(delegate{ DropboxValueChange(item.transform.Find("Value").GetComponent<TMPro.TMP_InputField>(), item.transform.Find("OtherData").GetComponent<TMPro.TMP_Dropdown>()); });
        }
        parent.SetActive(true);
    }
    Dictionary<string, object> GetSystemContentsData()
    {
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        foreach (Transform child in Contents.transform.Find("System"))
        {
            string name = child.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text;
            string value = child.Find("Value").GetComponent<TMPro.TMP_InputField>().text;
            int cnt = 0;
            foreach (TMPro.TMP_Dropdown.OptionData data in child.Find("OtherData").GetComponent<TMPro.TMP_Dropdown>().options)
            {
                if(child.Find("OtherData").GetComponent<TMPro.TMP_Dropdown>().options.Count - cnt > 5)
                {
                    cnt++;
                    continue;
                }
                cnt++;
            }
            string key = reference.Child("/GameInfo/System/" + name).Push().Key;
            childUpdates["/GameInfo/System/" + name + "/" + key] = value;
            childUpdates["/GamePlay/System/" + name] = value;
        }
        return childUpdates;
    }
    public void Save()
    {
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        string category = ActiveButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;
        switch (category)
        {
            case "System":
                childUpdates = GetSystemContentsData();
                break;
            case "Character":
                break;
            case "Spawn":
                break;
            case "Quest":
                break;
        }


        reference.UpdateChildrenAsync(childUpdates);
    }

    void DropboxValueChange(TMPro.TMP_InputField inputfield, TMPro.TMP_Dropdown dropbox)
    {
        inputfield.text = dropbox.options[dropbox.value].text;
    }
}
