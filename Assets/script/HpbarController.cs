using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class HpbarController : MonoBehaviour
{
    [SerializeField]
    int maxhp = 1000;
    int hp;
    [SerializeField]
    int damage = 0;

    //slider
    public Slider slider;

    void Start()
    {
        //sliderを最大にする
        slider.value = maxhp;
        //hpを最大hpと同じに
        hp = maxhp;


    }
    void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            hp = hp - damage;
            
            //sliderに反映
            slider.value = hp;
        }
    }

    public void Damege(int damege)
    {
        hp -= damege;
        //sliderに反映
        slider.value = hp;
    }
}
