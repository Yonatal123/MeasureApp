using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorPanelManager : MonoBehaviour
{
    public GameObject ColorPanel;
    // Start is called before the first frame update

    public void OpenPanel()
    {
        ColorPanel.SetActive(true);
        EventManager.Broadcast(eEventEnum.ChangeColorMode, null);
    }

    public void Awake()
    {
        EventManager.AddHandler(eEventEnum.ChangeColor, new Action<object>((p_val) => {
            closePanel();
        }));
    }

    void Start()
    {
        closePanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void closePanel()
    {
        ColorPanel.SetActive(false);
    }
}
