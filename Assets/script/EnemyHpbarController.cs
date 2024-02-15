using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpbarController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�G�����ʂƂ��̃G�t�F�N�g")]
    private GameObject effectDeathPrefab;

    [SerializeField]
    [Tooltip("hit�����Ƃ��̃G�t�F�N�g")]
    private GameObject effectHitPrefab;

    bool isDeath = false;

    [SerializeField]
    Slider slider;

    //�G��maxHP
    [SerializeField]
    private int maxHp = 100;
    //�G��HP
    [SerializeField]
    private int hp = 0;
    //�G�̍U����
    [SerializeField]
    int atttackPower = 5;
    //HP�\���p�X���C�_�[UI
    [SerializeField]
    int hpUI;

    EnemySpowner spowner;

    public void SetUpEnemy(EnemySpowner enemySpowner)
    {
        spowner = enemySpowner;
    }
    
    void Start()
    {
        //hp�������l�ɐݒ�
        hp = maxHp;
        //�X���C�_�[�������l�ɐݒ�
        slider.value = maxHp;
    }
    void Update()
    {

    }

    public void Damage(int damage)
    {
        slider.value -= damage;
        hp -= damage;
        if (hp <= 0 && isDeath == false) 
        {
            isDeath = true;
            spowner.enmeyKillCount++;
            Destroy(gameObject);
            //�G�����ʂƂ��̃G�t�F�N�g
            GameObject effect = Instantiate(effectDeathPrefab, this.transform.position, effectDeathPrefab.transform.rotation);
            Destroy(effect, 2.0f);
            return;
        }
        if (isDeath == false)
        {
            GameObject hiteffect = Instantiate(effectHitPrefab, this.transform.position, effectHitPrefab.transform.rotation);
            Destroy(hiteffect, 1.0f);
        }
    }
}
