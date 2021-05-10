using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResponseMessage
{
    public class Trade : MonoBehaviour
    {
        public enum CODE
        {
            SUCCESS,
            WAITING,        // my task calculating
            NEEDMOREMONEY,
            BUSY
        };

        public static string Receive(CODE code)
        {
            string output = "";
            switch (code)
            {
                case CODE.SUCCESS:
                    output = "Success";
                    break;
                case CODE.WAITING:
                    output = "Waiting";
                    break;
                case CODE.NEEDMOREMONEY:
                    output = "NeedMoreMoney";
                    break;
                case CODE.BUSY:
                    output = "Busy";
                    break;
            }
            return output;
        }
    }
}
