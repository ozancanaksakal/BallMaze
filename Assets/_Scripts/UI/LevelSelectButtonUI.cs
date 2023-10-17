using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButtonUI : MonoBehaviour {
    [SerializeField] Button button;
    [SerializeField] Image buttonImage;
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] Transform starImageContainer;
    [SerializeField] GameObject starImage;
    [SerializeField] Color lockColor;
    [SerializeField] GameObject lockImage;

    private void Awake() {
        starImage.SetActive(false);
        lockImage.SetActive(false);
    }

    public void SetLevelSelectButtonFields(LevelSO levelSO, SaveData.LevelData levelData) {
        buttonImage.sprite = levelSO.sprite;
        levelNameText.text = levelSO.levelName;

        if (levelData.isLevelOpen) {
            button.onClick.AddListener(() =>
            {
                Loader.LoadScene(levelSO.scene);
            });

            for (int i = 0; i < levelData.star; i++) {

                Instantiate(starImage, starImageContainer).SetActive(true);
            }

        }
        else {
            buttonImage.color = lockColor;
            Instantiate(lockImage, transform).SetActive(true);
        }
    }
}