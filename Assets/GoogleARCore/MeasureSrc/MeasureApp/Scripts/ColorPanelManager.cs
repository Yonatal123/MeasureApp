using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColorPanelManager : MonoBehaviour
{
    public GameObject ColorPanel;
    public GameObject RSlider;
    public GameObject GSlider;
    public GameObject BSlider;
    public GameObject ASlider;

    public GameObject RText;
    public GameObject GText;
    public GameObject BText;
    public GameObject AText;

    public GameObject ColorPreviewPanel;

    // Start is called before the first frame update

    public void OpenPanel()
    {
        ColorPanel.SetActive(true);
        EventManager.Broadcast(eEventEnum.ChangeColorMode, null);
    }

    public void Awake()
    {
        //EventManager.AddHandler(eEventEnum.ChangeColor, new Action<object>((p_val) => {
        //    closePanel();
        //}));

        RSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { RSliderChanged(); });
        GSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { GSliderChanged(); });
        BSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { BSliderChanged(); });
        ASlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { ASliderChanged(); });

        ASlider.GetComponent<Slider>().value = 127.5f;
    }

    public void OkColorPanelClick()
    {
        EventManager.Broadcast(eEventEnum.ChangeColor, new Color(RSlider.GetComponent<Slider>().value / 255, GSlider.GetComponent<Slider>().value / 255,
            BSlider.GetComponent<Slider>().value / 255, ASlider.GetComponent<Slider>().value / 255));
        closePanel();
    }

    public void CancelColorPanelClick()
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
    private void RSliderChanged()
    {
        RText.GetComponent<Text>().text = ((int)RSlider.GetComponent<Slider>().value).ToString();
        setColorPreview();
    }

    private void GSliderChanged()
    {
        GText.GetComponent<Text>().text = ((int)GSlider.GetComponent<Slider>().value).ToString();
        setColorPreview();
    }

    private void BSliderChanged()
    {
        BText.GetComponent<Text>().text = ((int)BSlider.GetComponent<Slider>().value).ToString();
        setColorPreview();
    }

    private void ASliderChanged()
    {
        AText.GetComponent<Text>().text = ((int)ASlider.GetComponent<Slider>().value).ToString();
        setColorPreview();
    }

    private void setColorPreview()
    {
        Color previewColor = new Color(RSlider.GetComponent<Slider>().value / 255, GSlider.GetComponent<Slider>().value / 255,
            BSlider.GetComponent<Slider>().value / 255, ASlider.GetComponent<Slider>().value / 255);
        ColorPreviewPanel.GetComponent<Image>().color = previewColor;
    }
}
