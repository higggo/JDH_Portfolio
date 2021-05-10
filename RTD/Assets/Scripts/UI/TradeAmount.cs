using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeAmount : MonoBehaviour
{
    [SerializeField]
    float Amount;
    [SerializeField]
    MoneyManager.ACTION Action;
    [SerializeField]
    uint SerialNumber;
    [SerializeField]
    bool IsSuccess;
    [SerializeField]
    string Message;

    public void Save(MoneyManager.ACTION Action, float Money, uint SerialNumber, string Message)
    {
        this.Amount = Money;
        this.Action = Action;
        this.SerialNumber = SerialNumber;
        this.Message = Message;
        IsSuccess = true;
        if (Action == MoneyManager.ACTION.Pay)
        {
            gameObject.name = "Pay";
        }
        else if (Action == MoneyManager.ACTION.Receive)
        {
            gameObject.name = "Receive";
        }
    }
}
