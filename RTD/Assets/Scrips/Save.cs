using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    [SerializeField]
    InputField inputField;
    [SerializeField]
    Slider slider;
    // Start is called before the first frame update

    public void SaveF()
    {
        PlayerPrefs.SetString("StringA", inputField.text);
        PlayerPrefs.SetFloat("SliderA", slider.value);
    }

    public void Load()
    {
        inputField.text = PlayerPrefs.GetString("StringA");
        slider.value = PlayerPrefs.GetFloat("SliderA");

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
