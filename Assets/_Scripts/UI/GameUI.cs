using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    // Panels
    [SerializeField] private GameObject winResultPanel;
    [SerializeField] private GameObject loseResultPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject openingPanel;

    //Buttons
    [SerializeField] private Button winButton;
    [SerializeField] private Button loseButton;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button optionsButton;

    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button continueButton;

    //Texts
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelNameText;

    [SerializeField] private GameObject[] starImages;

    private GameState lastState;
    
    private void Start() {
        Instance = this;

        Coin.OnAnyCoinCollected += (sender, collectetCoin) => UpdateScore(collectetCoin);
        GameManager.Instance.OnGameFinished += () => ShowResultPanel();

        winResultPanel.SetActive(false);
        loseResultPanel.SetActive(false);
        openingPanel.SetActive(true);
        optionsPanel.SetActive(false);

        winButton.onClick.AddListener(() =>
        {
            Loader.LoadNextLevel();
        });
        loseButton.onClick.AddListener(() =>
        {
            Loader.LoadCurrentLevel();
        });
        restartButton.onClick.AddListener(() =>
        {
            Loader.LoadCurrentLevel();
        });
        optionsButton.onClick.AddListener(() =>
        {
            optionsPanel.SetActive(true);
            lastState = GameManager.Instance.State;
            GameManager.Instance.State = GameState.Pause;
        });
        returnToMenuButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.MainMenu);
        });
        continueButton.onClick.AddListener(() =>
        {
            optionsPanel.SetActive(false);
            GameManager.Instance.State = lastState;
        });

        levelNameText.text = DataManager.Instance.GetCurrentLevelSO().levelName;
    }

    private void OnDestroy() {
        Coin.OnAnyCoinCollected -= (sender, collectetCoin) => UpdateScore(collectetCoin);
    }

    public void CloseOpeningPanel() {
        openingPanel.SetActive(false);
    }

    private void InputManager_OnEndTouch(Vector2 position, float time) {
        InputManager.Instance.OnEndTouch -= InputManager_OnEndTouch;
    }

    
    private void ShowResultPanel() {
        GameObject panel; float delayTime; bool isWin;

        if (GameManager.Instance.State == GameState.WinLevel) {
            panel = winResultPanel;
            delayTime = 0.5f;
            isWin = true;
        }
        else {
            panel = loseResultPanel;
            delayTime = 1.5f;
            isWin = false;
        }
        StartCoroutine(ShowPanel(panel, delayTime,isWin));
    }

    private IEnumerator ShowPanel(GameObject panel, float delayTime, bool isWin) {
        yield return new WaitForSeconds(delayTime);
        panel.SetActive(true);
        if (isWin) {
            int starNumber = DataManager.Instance.GetLevelStarNumberOnWin();
            starImages[starNumber-1].SetActive(true);
        }
    }

    public void UpdateScore(int score) {
        scoreText.text = "Points: " + score;
    }
}