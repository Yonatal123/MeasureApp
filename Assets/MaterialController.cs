using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public GameObject HandModel;
    // Start is called before the first frame update
    void Start()
    {
        m_material = HandModel.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetModelRed()
    {
        m_material.color = Color.red;
    }

    private Material m_material;
}
