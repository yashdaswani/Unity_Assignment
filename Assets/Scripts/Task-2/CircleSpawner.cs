using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public int minCircleCount = 5;
    public int maxCircleCount = 10;
    public Button restartButton;
    void Start()
    {
        SpawnCircles();
    }

    void SpawnCircles()
    {
        int circleCount = Random.Range(minCircleCount, maxCircleCount + 1);

        Vector3 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-1, Screen.height -1, 0));
        float screenWidth = screenDimensions.x * 2;
        float screenHeight = screenDimensions.y * 2;

        for (int i = 0; i < circleCount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-screenWidth / 2, screenWidth / 2), Random.Range(-screenHeight / 2, screenHeight / 2));
            GameObject circle = Instantiate(circlePrefab, spawnPosition, Quaternion.identity, transform);
            
        }
    }


    public void Restart()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        FindObjectOfType<CircleSpawner>().SpawnCircles();

        GameManager.instance.lineRenderer.positionCount = GameManager.instance.originalPositionCount; 
        GameManager.instance.lineRenderer.SetPositions(new Vector3[GameManager.instance.originalPositionCount]);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
