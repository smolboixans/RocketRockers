using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using SQLite;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static Game instance;
    void Awake () {
        if (instance == null) instance = this;
    }
    [SerializeField] private Text scoreText;
    [SerializeField] private Text endText;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Transform prevScoresTab;
    [SerializeField] private GameObject textPrefab;

    private float score = 0;

    public float gameStart = 1f;
    public bool gameOver = false;
    [SerializeField] private GameObject gameoverPanel;

    private string fileName = "game_data.db";

    void Start() {
        score = 0;
    }

    void Update() {
        if (gameStart > 0f)
            gameStart -= Time.deltaTime;
        
        gameoverPanel.SetActive(gameOver);
        scoreText.gameObject.SetActive(!gameOver);

        if (!gameOver)
            score += Time.deltaTime;
        
        scoreText.text = "Score: " + Mathf.RoundToInt(score).ToString();
        endText.text = "SCORE: " + Mathf.RoundToInt(score).ToString();
    }

    public void EndGame () {
        gameOver = true;
        Debug.Log("gameover");
        //do sql database stuff
        LoadScores();
    }

    public void RestartGame () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SubmitScore() {
        Debug.Log("score submission is called!");
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) {
            Debug.Log("please enter a name");
            return;
        }


        using (SQLiteConnection db = GetConnection()) {
            GameData existingData = db.Table<GameData>()
                .Where(x => x.PlayerName == playerName)
                .FirstOrDefault();

            if (existingData != null) {
                existingData.Score = Mathf.RoundToInt(score);
                db.Update(existingData);
            } else {
                GameData newData = new GameData {
                    PlayerName = playerName,
                    Score = Mathf.RoundToInt(score)
                };
                
                db.Insert(newData);
            }
        }
        Debug.Log("Score has been saved!");
        LoadScores();
    }

    private void LoadScores() {
        foreach (Transform child in prevScoresTab)
            Destroy(child.gameObject);

        using (SQLiteConnection db = GetConnection()) {
            List<GameData> scores = db.Table<GameData>()
                .OrderByDescending(x => x.Score)
                .Take(15)
                .ToList();
            
            foreach (GameData data in scores) {
                GameObject entry = Instantiate(textPrefab, prevScoresTab);
                entry.GetComponent<Text>().text = data.Score + " - " + data.PlayerName;
            }
        }
    }

    public void AddScore(int i) {score += i;}

    private SQLiteConnection GetConnection () {
        var dbPath = Path.Combine(Application.persistentDataPath, fileName);
        var dbConnection = new SQLiteConnection(dbPath);
        Debug.Log("sqlite path: " + dbPath);

        dbConnection.CreateTable<GameData>();
        
        return dbConnection;
    }
}
