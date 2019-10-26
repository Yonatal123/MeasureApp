using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MeasureToolDrawer : MonoBehaviour
{
    private LineRenderer m_lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        //m_lineRenderer.startWidth = 0.003f;
        //m_lineRenderer.endWidth = 0.003f;
        //m_lineRenderer.startColor = Color.cyan;
        //m_lineRenderer.endColor = Color.cyan;

        EventManager.AddHandler(eEventEnum.DrawLine, new System.Action<object>((p_points) => {
            Vector3[] points = (Vector3[])p_points;
            m_lineRenderer.positionCount += 2;
            m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 2, points[0]);
            m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, points[1]);
        }));

        EventManager.AddHandler(eEventEnum.AddMeasurePoint, new System.Action<object>((p_measurePoint) =>
        {
            MeasurePoint measurePoint = (MeasurePoint) p_measurePoint;
            GameObject prefab = Instantiate(measurePoint.PrefabToInstantiate);
            prefab.transform.position = measurePoint.InstantiatePosition;
            m_pointIconsList.Add(prefab);
        }));

        EventManager.AddHandler(eEventEnum.DrawMeasureText, new System.Action<object>((p_measureText) =>
        {
            MeasureText measureText = (MeasureText)p_measureText;
            GameObject prefab = Instantiate(measureText.PrefabToInstantiate);
            prefab.transform.position = measureText.InstantiatePosition;
            prefab.GetComponentInChildren<TextMesh>().text = measureText.DistanceValue;
        }));

        EventManager.AddHandler(eEventEnum.ExitRuller, new System.Action<object>((p_obj) =>
        {
            m_lineRenderer.positionCount = 0;
            Object[] obj = Object.FindObjectsOfType(typeof(GameObject));
            for(int i = 0; i < obj.Length; i++)
            {
                if(obj[i].name == "PointIconObject(Clone)" || obj[i].name == "MeasureTextObject(Clone)")
                {
                    Destroy(obj[i]);
                }
            }
            m_pointIconsList.ForEach(p_prefab => Destroy(p_prefab));
            m_pointIconsList.Clear();
        }));
    }

    public void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> m_pointIconsList = new List<GameObject>();

 }

public class MeasurePoint
{
    public MeasurePoint(GameObject p_gameObject, Vector3 p_instantiatePosition)
    {
        PrefabToInstantiate = p_gameObject;
        InstantiatePosition = p_instantiatePosition;
    }
    public GameObject PrefabToInstantiate { get; set; }
    public Vector3 InstantiatePosition { get; set; }
}

public class MeasureText
{
    public MeasureText(GameObject p_gameObject, Vector3 p_instantiatePoint, string p_distanceValue)
    {
        PrefabToInstantiate = p_gameObject;
        InstantiatePosition = p_instantiatePoint;
        DistanceValue = p_distanceValue;
    }

    public GameObject PrefabToInstantiate { get; set; }
    public Vector3 InstantiatePosition { get; set; }
    public string DistanceValue { get; set; }
}
