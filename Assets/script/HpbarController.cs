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
        //slider���ő�ɂ���
        slider.value = maxhp;
        //hp���ő�hp�Ɠ�����
        hp = maxhp;


    }
    void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            hp = hp - damage;
            
            //slider�ɔ��f
            slider.value = hp;
        }
    }

    public void Damege(int damege)
    {
        hp -= damege;
        //slider�ɔ��f
        slider.value = hp;
    }
}
