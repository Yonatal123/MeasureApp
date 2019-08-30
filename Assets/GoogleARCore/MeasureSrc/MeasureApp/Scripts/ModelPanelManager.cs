using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelPanelManager : MonoBehaviour
{
    public GameObject ModelPanel;

    public Image Hand1SelectionPanel;
    public Image Hand2SelectionPanel;
    public Text Hand1SelectionText;
    public Text Hand2SelectionText;

    public void OpenPanel()
    {
        ModelPanel.SetActive(true);
        //Animator animator = ModelPanel.GetComponent<Animator>();
        //if(animator != null)
        //{
        //    bool isOpen = animator.GetBool("open");
        //    animator.SetBool("open", !isOpen);
        //}
    }

    public void Hand1Selected()
    {
        selectModel(eModelEnum.HAND_1);
    }

    public void Hand2Selected()
    {
        selectModel(eModelEnum.HAND_2);
    }

    private void selectModel(eModelEnum p_modelType)
    {
        ModelPanel.SetActive(false);

        switch (p_modelType)
        {
            case eModelEnum.HAND_1:
                {
                    Hand1SelectionPanel.color = new Color(18 / 255.0f, 255 / 255.0f, 255 / 255.0f, 255 / 255.0f);
                    Hand2SelectionPanel.color = new Color(255 / 255.0f, 255 / 255.0f, 255 / 255.0f, 255 / 255.0f);
                    Hand1SelectionText.color = Color.white;
                    Hand2SelectionText.color = Color.black;
                    EventManager.Broadcast(eEventEnum.Hand1Selected, 0);
                    break;
                }
            case eModelEnum.HAND_2:
                {
                    Hand1SelectionPanel.color = new Color(255 / 255.0f, 255 / 255.0f, 255 / 255.0f, 255 / 255.0f);
                    Hand2SelectionPanel.color = new Color(18 / 255.0f, 255 / 255.0f, 255 / 255.0f, 255 / 255.0f);
                    Hand1SelectionText.color = Color.black;
                    Hand2SelectionText.color = Color.white;
                    EventManager.Broadcast(eEventEnum.Hand2Selected, 0);
                    break;
                }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ModelPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
