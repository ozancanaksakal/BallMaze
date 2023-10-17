using UnityEngine;
using System.IO;
using System;

[Serializable]
public class SaveData
{
    [Serializable]
    public struct LevelData
    {
        public bool isLevelOpen;
        public int star;
    }

    public int totalCoin;
    public int selectedBallTextureindex;
    public bool[] isBallTexturesOpen;
    public LevelData[] levelsDatas;

    public SaveData(int levelCount, int numberOfBallTextures) {
        isBallTexturesOpen = new bool[numberOfBallTextures];
        isBallTexturesOpen[0] = true;
        selectedBallTextureindex = 0;
        totalCoin = 0;
        levelsDatas = new LevelData[levelCount];
        levelsDatas[0].isLevelOpen = true;
    }
}

public static class SaveManager
{
    public static readonly string directoryPath = Application.persistentDataPath + "/" + "saveData.json";


    public static SaveData CreateNewSaveData(int levelCount, int numberOfBallTextures) {
        return new SaveData(levelCount, numberOfBallTextures);
    }

    public static void Save(SaveData saveData) {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(directoryPath, json);
    }

    public static SaveData Load() {
        if (IsThereSaveFile()) {
            string json = File.ReadAllText(directoryPath);
            SaveData loadedSaveFile = JsonUtility.FromJson<SaveData>(json);
            return loadedSaveFile;
        }
        else {
            Debug.Log("Save File not found");
            return null;
        }
    }

    public static bool IsThereSaveFile() { return File.Exists(directoryPath); }
}