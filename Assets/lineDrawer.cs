using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineDrawer : MonoBehaviour
{
    private LineRenderer m_lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        //m_lineRenderer.startWidth = 0.5f;
        //m_lineRenderer.endWidth = 0.5f;
        //m_lineRenderer.startColor = Color.cyan;
        //m_lineRenderer.endColor = Color.cyan;
        //m_lineRenderer.SetPosition(0, new Vector3(Screen.width / 2, Screen.height / 2));
        //m_lineRenderer.SetPosition(1, new Vector3(Screen.width / 2 + Screen.width / 4, Screen.height / 2 + Screen.height / 4));

        EventManager.AddHandler(eEventEnum.DrawLine, new System.Action<object>((p_points) => {
            Vector3[] points = (Vector3[])p_points;
            m_lineRenderer.positionCount = 0;
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, points[0]);
            m_lineRenderer.SetPosition(1, points[1]);
            //m_lineRenderer.SetPosition(0, new Vector3(Screen.width / 2, Screen.height / 2));
            //m_lineRenderer.SetPosition(1, new Vector3(Screen.width / 2 + Screen.width / 4, Screen.height / 2 + Screen.height / 4));
        }));
    }

    public void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
