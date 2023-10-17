using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInputActions playerInputActions;

    public delegate void StartTouch(Vector2 position, float time);
    public delegate void EndTouch(Vector2 position, float time);

    public event StartTouch OnStartTouch;
    public event EndTouch OnEndTouch;


    private void Awake() {
        Instance = this;
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable() {
        playerInputActions.Enable();
    }
    private void OnDisable() {
        playerInputActions.Disable();
    }

    private void Start() {
        playerInputActions.Touch.PrimaryTouch.started += PrimaryTouch_started;
        playerInputActions.Touch.PrimaryTouch.canceled += PrimaryTouch_canceled;
        playerInputActions.Touch.PrimaryTouch.canceled += OnFirstTouch;
    }

    private void OnFirstTouch(InputAction.CallbackContext obj) {
        GameUI.Instance.CloseOpeningPanel();
        GameManager.Instance.State = GameState.WaitingInput;
        playerInputActions.Touch.PrimaryTouch.canceled -= OnFirstTouch;
    }

    private void PrimaryTouch_started(InputAction.CallbackContext context) {
        if (GameManager.Instance.State == GameState.WaitingInput) {
            Vector2 touchPosition = playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
            OnStartTouch?.Invoke(touchPosition, (float)context.startTime);
        }
    }
    private void PrimaryTouch_canceled(InputAction.CallbackContext context) {
        if (GameManager.Instance.State == GameState.WaitingInput) {
        Vector2 touchPosition = playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
        OnEndTouch?.Invoke(touchPosition, (float)context.time);
         }
        else if (GameManager.Instance.State == GameState.ReadyToStart) {

        }
    }

    public Vector3 GetPrimaryPosition() {
        return Utilities.ScreenToWorld(Camera.main, playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

    //private void PrimaryTouch_started(InputAction.CallbackContext context) {
    //    if (GameManager.Instance.State == GameState.WaitingInput) {
    //        Vector2 touchPosition = playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
    //        OnStartTouch?.Invoke(touchPosition, (float)context.startTime);
    //    }
    //}
    //private void PrimaryTouch_canceled(InputAction.CallbackContext context) {
    //    if (GameManager.Instance.State == GameState.WaitingInput) {
    //        Vector2 touchPosition = playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
    //        OnEndTouch?.Invoke(touchPosition, (float)context.time);
    //        //Vector3 isteyen function'a vector2 yollarsak otomatik vector3 oluyor
    //    }
    //}
}
