using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveCounterUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI remainMoveText;
    [SerializeField] private Image remainMoveImage;

    private int remainMoveNumber;
    private int maxMoveNumber;

    public static bool IsLimitExceeded { get; private set; }

    private void Awake() {
        IsLimitExceeded = false;
    }

    private void Start()
    {
        GameManager.Instance.OnMovementCompleted += GameManager_OnMovementCompleted;
        maxMoveNumber = DataManager.Instance.GetCurrentLevelSO().maxMoveNumber;
        remainMoveNumber = maxMoveNumber;
        UpdateVisual();
    }
    private void OnDestroy() {
        GameManager.Instance.OnMovementCompleted -= GameManager_OnMovementCompleted;
    }

    private void GameManager_OnMovementCompleted()
    {
        remainMoveNumber--;
        if (remainMoveNumber > 0) {
            UpdateVisual();
        }
        else if (remainMoveNumber == 0) {
            remainMoveText.color = Color.red;
            UpdateVisual();
        }
        else { 
            IsLimitExceeded = true;
        }
    }

    private void UpdateVisual()
    {
        remainMoveText.text = remainMoveNumber.ToString();
        remainMoveImage.fillAmount = (float)remainMoveNumber / maxMoveNumber;
    }
}