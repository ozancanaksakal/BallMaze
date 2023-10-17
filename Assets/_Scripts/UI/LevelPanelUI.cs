using UnityEngine;
using UnityEngine.UI;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] Transform levelbuttons;
    [SerializeField] Button levelPanelBackButton;
    [SerializeField] GameObject levelSelectButton;

    private void Start() {
        levelPanelBackButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        HandleLevelButtons();
    }

    private void HandleLevelButtons() {

        foreach (Transform button in levelbuttons)
        {
            Destroy(button.gameObject);
        }

        var levelSOs = DataManager.Instance.GetLevelSOList();
        var savedLevelDatas = DataManager.Instance.GetLevelDatas();

        for (int i = 0; i < levelSOs.Length; i++) {
            LevelSO levelSO = levelSOs[i];
            SaveData.LevelData levelData = savedLevelDatas[i];

            GameObject levelButton = Instantiate(levelSelectButton, levelbuttons);
            levelButton.GetComponent<LevelSelectButtonUI>().SetLevelSelectButtonFields(levelSO, levelData);
        }
    }
}