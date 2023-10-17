using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    //public static MainMenuUI Instance { get; private set; }
    [SerializeField] Button playButton;
    [SerializeField] Button selectLevelButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button quitButton;

    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject shopPanel;

    private void Awake() {

        levelPanel.SetActive(false);
        shopPanel.SetActive(false);

        playButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.Level01);
        });
        selectLevelButton.onClick.AddListener(() =>
        {
            levelPanel.SetActive(true);
        });
        shopButton.onClick.AddListener(() =>
        {
            shopPanel.SetActive(true);
        });

        quitButton.onClick.AddListener(Application.Quit);
    }
}