using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomScript : MonoBehaviour
{
    public Text ZoomIndication;
    // Start is called before the first frame update

    public void Awake()
    {
        EventManager.AddHandler(eEventEnum.Hand1Selected, new Action<object>((p_val) => {
            m_currentModel = eModelEnum.HAND_1;
            m_zoomVal = 0.02f;
            updateZoomIndication();
        }));

        EventManager.AddHandler(eEventEnum.Hand2Selected, new Action<object>((p_val) => {
            m_currentModel = eModelEnum.HAND_2;
            m_zoomVal = 0.01f;
            updateZoomIndication();
        }));

        EventManager.AddHandler(eEventEnum.ZoomIn, new Action<object>((p_val) => {
            ZoomIn();
            updateZoomIndication();
        }));

        EventManager.AddHandler(eEventEnum.ZoomOut, new Action<object>((p_val) => {
            ZoomOut();
            updateZoomIndication();
        }));

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ZoomIn()
    {
        setModel();
        m_model.transform.localScale += new Vector3(m_zoomVal, m_zoomVal, m_zoomVal);
        setZoomValue(true);
    }

    public void ZoomOut()
    {
        setModel();
        m_model.transform.localScale -= new Vector3(m_zoomVal, m_zoomVal, m_zoomVal);
        setZoomValue(false);
    }

    private void setZoomValue(bool p_add)
    {
        switch (m_currentModel)
        {
            case eModelEnum.HAND_1:
                {
                    if(p_add)
                    {
                        m_modelsZoomDictionary[eModelEnum.HAND_1] += 0.02;                
                    }
                    else
                    {
                        m_modelsZoomDictionary[eModelEnum.HAND_1] -= 0.02;
                    }                   
                    break;
                }
            case eModelEnum.HAND_2:
                {
                    if(p_add)
                    {
                        m_modelsZoomDictionary[eModelEnum.HAND_2] += 0.02;
                    }
                    else
                    {
                        m_modelsZoomDictionary[eModelEnum.HAND_2] -= 0.02;
                    }
                    break;
                }
        }
        updateZoomIndication();
    }

    private void updateZoomIndication()
    {
        switch(m_currentModel)
        {
            case eModelEnum.HAND_1:
                {
                    ZoomIndication.text = "x" + m_modelsZoomDictionary[eModelEnum.HAND_1].ToString("F2");
                    EventManager.Broadcast(eEventEnum.ZoomValueChanged, m_modelsZoomDictionary[eModelEnum.HAND_1]);
                    break;
                }
            case eModelEnum.HAND_2:
                {
                    ZoomIndication.text = "x" + m_modelsZoomDictionary[eModelEnum.HAND_2].ToString("F2");
                    EventManager.Broadcast(eEventEnum.ZoomValueChanged, m_modelsZoomDictionary[eModelEnum.HAND_2]);
                    break;
                }
        }
    }

    private void setModel()
    {
        switch(m_currentModel)
        {
            case eModelEnum.HAND_1:
                {
                    m_model = GameObject.Find("AndyGreen(Clone)");
                    break;
                }
            case eModelEnum.HAND_2:
                {
                    m_model = GameObject.Find("AndyGreenDiffuse(Clone)");
                    break;
                }
        }
        //if (m_model == null)
        //{
        //    if (GameObject.Find("AndyGreen(Clone)") != null)
        //    {
        //        m_model = GameObject.Find("AndyGreen(Clone)");
        //    }
        //    else if (GameObject.Find("AndyGreenDiffuse(Clone)") != null)
        //    {
        //        m_model = GameObject.Find("AndyGreenDiffuse(Clone)");
        //    }
        //}
    }


    GameObject m_model;
    private IDictionary<eModelEnum, double> m_modelsZoomDictionary = new Dictionary<eModelEnum, double>() { {eModelEnum.HAND_1, 1.0}, { eModelEnum.HAND_2, 1.0 } };
    private eModelEnum m_currentModel = eModelEnum.HAND_1;
    private float m_zoomVal = 0.02F;
}
