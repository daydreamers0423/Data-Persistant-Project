using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighestScore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private PlayerData player;
    

    
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/player.json";
        if(File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            player = JsonUtility.FromJson<PlayerData>(jsonText);
            StartGame.UserName = player.userName;
            HighestScore.text = "Best Score :" + StartGame.UserName +" : "+player.highestScore;
        }
        else
        {
            Debug.Log("Doesn't exist..");
            HighestScore.text = "Best Score :" + StartGame.UserName + ": 0";

        }
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        Debug.Log(player == null);
        Debug.Log(m_Points);
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path) && player != null)
        {
            player.highestScore = m_Points > player.highestScore ? m_Points : player.highestScore;
            
            string json = JsonUtility.ToJson(player);

            File.WriteAllText(Application.persistentDataPath + "/player.json", json);
        }
        else
        {

            PlayerData player = new PlayerData();
            player.highestScore = m_Points;
            player.userName = StartGame.UserName;

            string json = JsonUtility.ToJson(player);

            File.WriteAllText(Application.persistentDataPath + "/player.json", json);
        }    

        GameOverText.SetActive(true);
    }
}
