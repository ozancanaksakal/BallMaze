using UnityEngine;
using UnityEngine.UI;

public class EndSceneUI : MonoBehaviour
{
    [SerializeField] Button menuButton;

    private void Awake() {
        menuButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.MainMenu);
        }
            );

    }
}
