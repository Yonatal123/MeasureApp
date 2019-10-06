using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    public void Awake()
    {
        EventManager.AddHandler(eEventEnum.ChangeColor, new Action<object>((p_val) => {
            m_material.color = Color.red;
        }));
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private Material m_material;
}
