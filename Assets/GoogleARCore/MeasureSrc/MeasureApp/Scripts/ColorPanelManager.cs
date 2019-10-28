using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPanelManager : MonoBehaviour
{
    public GameObject ColorPanel;
    // Start is called before the first frame update

    public void OpenPanel()
    {
        ColorPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        closePanel();
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
