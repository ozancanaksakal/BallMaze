using UnityEngine;

public enum SwipeDirection
{
    Left,
    Right,
    Up,
    Down,
}
public class SwipeDetector : MonoBehaviour
{
    public static SwipeDetector Instance { get; private set; }

    [SerializeField] float minimumDistance = .4f;
    [SerializeField] float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] float directionThreshold = .9f;
    [SerializeField] GameObject trial;

    public delegate void SwipeDetected(SwipeDirection swipeDirection);
    public event SwipeDetected OnSwipeDetected;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startTime;
    private float endTime;

    private void Awake() {
        Instance = this;
    }
    private void OnEnable() {
        InputManager.Instance.OnStartTouch += InputManager_OnStartTouch;
        InputManager.Instance.OnEndTouch += InputManager_OnEndTouch;
    }
    private void OnDisable() {
        InputManager.Instance.OnStartTouch -= InputManager_OnStartTouch;
        InputManager.Instance.OnEndTouch -= InputManager_OnEndTouch;
    }

    private void InputManager_OnStartTouch(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
        trial.SetActive(true);
    }

    private void InputManager_OnEndTouch(Vector2 position, float time) {
        endPosition = position;
        endTime = time;

        trial.SetActive(false);
        if (GameManager.Instance.State == GameState.WaitingInput)
            DetectSwipe();
    }

    private void Update() {
        if (trial.activeSelf) {
            trial.transform.position = InputManager.Instance.GetPrimaryPosition();
        }
    }


    private void DetectSwipe() {
        Vector2 direction = endPosition - startPosition;
        float distance = direction.magnitude;
        float deltaTime = endTime - startTime;
        if (distance > minimumDistance && deltaTime < maximumTime) {
            // that means swipe is done
            DetectDirection(direction.normalized);
        }
    }
    private void DetectDirection(Vector2 direction2D) {
        /* cosine value of the angle between vectors
        if it is 1 it means same direction */
        if (Vector2.Dot(Vector2.up, direction2D) > directionThreshold) {
            //Debug.Log("Swiped Up");
            OnSwipeDetected?.Invoke(SwipeDirection.Up);
        }
        else if (Vector2.Dot(Vector2.down, direction2D) > directionThreshold) {
           // Debug.Log("Swiped Down");
            OnSwipeDetected?.Invoke(SwipeDirection.Down);
        }
        else if (Vector2.Dot(Vector2.left, direction2D) > directionThreshold) {
            //Debug.Log("Swiped Left");
            OnSwipeDetected?.Invoke(SwipeDirection.Left);
        }
        else if (Vector2.Dot(Vector2.right, direction2D) > directionThreshold) {
           // Debug.Log("Swiped Right");
            OnSwipeDetected?.Invoke(SwipeDirection.Right);
        }
    }
}