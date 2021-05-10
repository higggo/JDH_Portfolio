using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Health;

    private void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            Health.value += Time.deltaTime;
        }
    }
}
