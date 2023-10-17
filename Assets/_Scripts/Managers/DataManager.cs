using UnityEngine;

public class DataManager : MonoBehaviour {
    public static DataManager Instance { get; private set; }

    [SerializeField] private BallTextureSOList ballTextureSOList;
    [SerializeField] private LevelSOList levelSOList;
    private SaveData savedData;

    private void Awake() {
        Instance = this;
        ResetStaticData();
        LoadSaveData();
    }
    private void Start() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameFinished += GameManager_OnGameFinished;
        }
    }
    private void OnDestroy() {
        SaveManager.Save(savedData);
    }

    private void GameManager_OnGameFinished() {
        if (GameManager.Instance.State == GameState.LoseLevel)
            return;

        int levelIndex = Loader.indexOfCurrentLevel - 1;

        int starNumber = GetLevelStarNumberOnWin();

        if (starNumber > savedData.levelsDatas[levelIndex].star)
            savedData.levelsDatas[levelIndex].star = starNumber;

        UpdateTotalCoin(Coin.GetCollectedCoin());

        if (!Loader.IsLastLevel())
            savedData.levelsDatas[levelIndex + 1].isLevelOpen = true;
    }

    public int GetLevelStarNumberOnWin() {

        int starNumber = 1;

        //if (GameManager.Instance.State == GameState.WinLevel)
        //starNumber++;

        if (!Coin.IsThereCoin())
            starNumber++;

        if (!MoveCounterUI.IsLimitExceeded)
            starNumber++;

        return starNumber;
    }


    public int GetTotalCoin() { return savedData.totalCoin; }
    public void UpdateTotalCoin(int updateAmount) { savedData.totalCoin += updateAmount; }

    private void LoadSaveData() {
        if (SaveManager.IsThereSaveFile())
            savedData = SaveManager.Load();
        else
            savedData = SaveManager.CreateNewSaveData(Loader.countLevels, ballTextureSOList.list.Length);
    }

    public LevelSO[] GetLevelSOList() { return levelSOList.list; }

    public SaveData.LevelData[] GetLevelDatas() {
        return savedData.levelsDatas;
    }
    public SaveData.LevelData GetCurrentLevelData() { return savedData.levelsDatas[Loader.indexOfCurrentLevel - 1]; }

    public LevelSO GetCurrentLevelSO() {
        int index = Loader.indexOfCurrentLevel - 1;
        return levelSOList.list[index];
    }

    public int GetSelectedBallTextureIndex() { return savedData.selectedBallTextureindex; }

    public void SetSelectedBallTexture(int index) { savedData.selectedBallTextureindex = index; }

    public void OpenBallTexturesLock(int ballTextureIndex) {
        savedData.isBallTexturesOpen[ballTextureIndex] = true;
    }

    public bool IsBallTextureOpen(int ballTextureIndex) {
        return savedData.isBallTexturesOpen[ballTextureIndex];
    }
    public Material GetSelectedBallTexture() {
        int materialIndex = GetSelectedBallTextureIndex();
        return ballTextureSOList.list[materialIndex].material;
    }

    private void ResetStaticData() {
        Ball.ResetBallNumber();
        Coin.ResetCoinNumber();
    }
}