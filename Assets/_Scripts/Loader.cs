using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        Level01,
        Level02,
        Level03,
        Level04,
        Level05,
        Level06,
        Level07,
        Level08,
        Level09,
        EndScene
    }
    public static int indexOfCurrentLevel =>
        SceneManager.GetActiveScene().buildIndex;

     //number of the total level scenes
    public static int countLevels => 
         SceneManager.sceneCountInBuildSettings - 2;

    public static bool IsLastLevel() { return indexOfCurrentLevel == countLevels; }

    public static void LoadScene(Scene scene) {
        SceneManager.LoadScene((int)scene);
    }

    public static void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadNextLevel() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

}