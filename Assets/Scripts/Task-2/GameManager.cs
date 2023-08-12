using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    
    public LayerMask circleLayer;
    private bool isDrawing = false;
    public int originalPositionCount;
    public GameObject ui;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        originalPositionCount = lineRenderer.positionCount;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }

        if (isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, mousePos);
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        lineRenderer.enabled = true;
    }

    void StopDrawing()
    {
        isDrawing = false;
        lineRenderer.enabled = false;

        Vector2[] linePositions = new Vector2[lineRenderer.positionCount];
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            linePositions[i] = lineRenderer.GetPosition(i);
        }

        RaycastHit2D[] hits = Physics2D.LinecastAll(linePositions[0], linePositions[linePositions.Length - 1], circleLayer);

        foreach (RaycastHit2D hit in hits)
        {
            Destroy(hit.collider.gameObject);
        }

        
        for(int  i = 0; i < ui.transform.childCount; i++)
        {
            Destroy(ui.transform.GetChild(i).gameObject);
        }
    }


    
}
