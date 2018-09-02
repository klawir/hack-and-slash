using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MobHealth : MonoBehaviour
{
    public float healthPoint;
    public Image healthPointPref;
    public Transform healthPointPosition;

    private Image hpBar;
    private float currentHp;
    private MobAttributes mobAttributes;
    private Mob Mob;

    public float CurrentHP
    {
        get { return currentHp; }
        set { currentHp = value; }
    }
    public Image HpBar
    {
        get { return hpBar; }
    }

    void Start()
    {
        mobAttributes = GetComponent<MobAttributes>();
        Mob = GetComponent<Mob>();
        hpBar = Instantiate(healthPointPref, healthPointPosition.position, healthPointPref.transform.rotation) as Image;
        hpBar.transform.SetParent(GameObject.FindGameObjectWithTag("canvasHp").transform, false);
        if (Mob.name == "Ghost")
            hpBar.gameObject.SetActive(false);
        currentHp = healthPoint;
        if(Mob.Melee)
            hpBar.transform.GetComponent<TextMesh>().text = Mob.name;
        else
            hpBar.transform.Find("HpBar").GetComponent<TextMesh>().text = Mob.name;
    }
    void Update()
    {
        hpBar.transform.position = healthPointPosition.position;
        if (Mob.Melee)
            hpBar.fillAmount = currentHp / healthPoint;
        else
            hpBar.transform.Find("HpBar").GetComponent<Image>().fillAmount = currentHp / healthPoint;
    }
}