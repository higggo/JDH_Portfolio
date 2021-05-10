using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ResponseMessage;

using Firebase;
using Firebase.Database;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    uint money;
    bool IsCalculatingMoney;
    uint SerialNumber = 0;
    public TMPro.TextMeshProUGUI GoldText = null;

    Coroutine CalculateCoroutine;

    public enum ACTION
    {
        Pay,
        Receive,
    }


    private void Awake()
    {
        IsCalculatingMoney = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GoldText == null) GameObject.Find("Gold").GetComponent<TMPro.TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("GamePlay/System/Money")
      .GetValueAsync().ContinueWith(task => {
          if (task.IsFaulted)
          {
              Debug.Log("server connect fail");
          }
          else if (task.IsCompleted)
          {
              DataSnapshot result = task.Result;
              Debug.Log(result.Value.ToString());
              uint _money = uint.Parse(result.Value.ToString());
              SetMoney(_money);
          }
      });
        // LJH: 돈조절
        //SetMoney(1000); 
        foreach(Transform child in gameObject.transform.Find("Account"))
        {
            Destroy(child.gameObject);
        }
        //GoldText.text = money.ToString();
    }

    public float GetMoney()
    {
        return money;
    }

    void SetMoney(uint money)
    {
        this.money = money;
        GoldText.text = this.money.ToString();
    }

    public bool CalculateMoney(ACTION act, uint money, ResponseMessage.Trade.CODE respone, string Message)
    {
        bool output = false;
        if (IsCalculatingMoney)
        {
            respone = ResponseMessage.Trade.CODE.BUSY;
            return false;
        }

        IsCalculatingMoney = true;

        if (act == ACTION.Pay)
        {
            if (this.money >= money)
            {
                this.money -= money;
                respone = ResponseMessage.Trade.CODE.SUCCESS;
            }
            else
            {
                respone = ResponseMessage.Trade.CODE.NEEDMOREMONEY;
                output = false;
            }
        }
        else if (act == ACTION.Receive)
        {
            this.money += money;
            respone = ResponseMessage.Trade.CODE.SUCCESS;
        }

        //
        if(respone == ResponseMessage.Trade.CODE.SUCCESS)
        {
            output = true;
            GameObject obj = Instantiate(Resources.Load("UI/TradeAmount")) as GameObject;
            obj.GetComponent<TradeAmount>().Save(act, money, SerialNumber++, Message);
            obj.transform.parent = gameObject.transform.Find("Account");
        }
        GoldText.text = this.money.ToString();
        IsCalculatingMoney = false;
        return output;
    }
    //public ResponseCode.TRADE PayMoney(float money)
    //{
    //    if(IsCalculatingMoney)
    //    {
    //       return ResponseCode.TRADE.BUSY;
    //    }
    //    else
    //    {
    //        return CalculateMoney(ACTION.Pay, money);
    //    }
    //}

    //public ResponseCode.TRADE ReceiveMoney(float money)
    //{
    //    if (IsCalculatingMoney)
    //    {
    //        return ResponseCode.TRADE.BUSY;
    //    }
    //    else
    //    {
    //        return CalculateMoney(ACTION.Pay, money);
    //    }
    //}
    //IEnumerator CalculateMoney(ACTION act, float money, TradeDelegate done)
    //{
    //    ResponseCode.TRADE respone;
    //    IsCalculatingMoney = true;
    //    GameObject obj = Instantiate(Resources.Load("Resouces/UI/TradeAmount")) as GameObject;
    //    obj.GetComponent<TradeAmount>().Save(act, money, SerialNumber++);
    //    obj.transform.parent = gameObject.transform.Find("Account");

    //    //// Waiting for Server Response ...
    //    //while (IsCalculatingMoney)
    //    //{
    //    //    foreach(Transform child in gameObject.transform.Find("Account"))
    //    //    {
    //    //        child.GetComponent<TradeAmount>.
    //    //    }
    //    //    yield return null;
    //    //}
    //    if (act == ACTION.Pay)
    //    {

    //        if (this.money >= money)
    //        {
    //            this.money -= money; 
    //            respone = ResponseCode.TRADE.SUCCESS;
    //        }
    //        else
    //        {
    //            respone = ResponseCode.TRADE.NEEDMOREMONEY;
    //        }
    //    }
    //    else if(act == ACTION.Receive)
    //    {
    //        this.money += money;
    //            respone = ResponseCode.TRADE.SUCCESS;
    //    }
    //    yield return null;
    //}

}
