using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPanelManager : MonoBehaviour
{
    public GameObject EditPanel;
    public Button HeadingButton;
    public Button PitchButton;
    public Button RollButton;
    public Button OpenEditPanelButton;

    private int m_toggleCounter;

    private IDictionary<Button, bool> m_editStateButtonsDictionary = new Dictionary<Button, bool>();

    //private void OnGUI()
    //{
    //    GUI.Toggle(new Rect(70, -432, 105.37f, 617.9f), toggle, "Edit", "button");
    //}

    // Start is called before the first frame update
    
    void Start()
    {
        EditPanel.SetActive(false);
        m_editStateButtonsDictionary.Add(HeadingButton, true);
        m_editStateButtonsDictionary.Add(PitchButton, false);
        m_editStateButtonsDictionary.Add(RollButton, false);
        setButtonsState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenEditPanel()
    {
        if(++m_toggleCounter % 2 != 0)
        {
            EditPanel.SetActive(true);
            OpenEditPanelButton.GetComponent<Image>().color = OpenEditPanelButton.colors.pressedColor;
            broadCastEditModeChanged();
        }
        else
        {
            EditPanel.SetActive(false);
            OpenEditPanelButton.GetComponent<Image>().color = OpenEditPanelButton.colors.normalColor;
            EventManager.Broadcast(eEventEnum.EditModeChanged, null);
        }
    }

    public void SetHeadingState()
    {
        setEditState(HeadingButton);
    }

    public void SetPitchState()
    {
        setEditState(PitchButton);
    }

    public void SetRollState()
    {
        setEditState(RollButton);
    }

    private void setEditState(Button p_selectedButton)
    {
        IDictionary<Button, bool> tempDictionary = new Dictionary<Button, bool>();
   
        foreach (var button in m_editStateButtonsDictionary)
        {
            if (button.Key == p_selectedButton)
            {
                tempDictionary.Add(button.Key, true);
            }
            else
            {
                tempDictionary.Add(button.Key, false);
            }
        }

        m_editStateButtonsDictionary.Clear();
        m_editStateButtonsDictionary = tempDictionary;


        broadCastEditModeChanged();

        setButtonsState();
    }

    private void broadCastEditModeChanged()
    {
        Button currentStateButton = null;
        foreach (var button in m_editStateButtonsDictionary)
        {
            if(button.Value == true)
            {
                currentStateButton = button.Key;
            }
        }

        if(currentStateButton != null)
        {
            eEditMode selectedMode = eEditMode.Heading;
            if(currentStateButton == HeadingButton)
            {
                selectedMode = eEditMode.Heading;
            }
            if(currentStateButton == PitchButton)
            {
                selectedMode = eEditMode.Pitch;
            }
            if(currentStateButton == RollButton)
            {
                selectedMode = eEditMode.Roll;
            }
            EventManager.Broadcast(eEventEnum.EditModeChanged, selectedMode);
        }
    }

    private void setButtonsState()
    {
        foreach(var button in m_editStateButtonsDictionary)
        {
            if(button.Value)
            {
                button.Key.GetComponent<Image>().color = button.Key.colors.pressedColor;
            }
            else
            {
                button.Key.GetComponent<Image>().color = button.Key.colors.normalColor;
            }
        }
    }
}

