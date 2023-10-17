using UnityEngine;

public class Ball : MovableObject
{
    private const string COIN_TAG = "Coin";
    private const string BREAKABLE_TAG = "Breakable";
    private const string KILLER_TAG = "Killer";

    private static int totalBallNumber;
    
    [SerializeField] Transform ballVisualTransform;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] private LayerMask wallLayer;

    private float ballRadius = 0.5f;
    private Vector3 rotationVector;

    void Start() {
        totalBallNumber++;
        var collider = GetComponent<SphereCollider>();
        ballRadius = collider.radius;
        Init(ballRadius);

        HandleVisualMaterial();
    }

    private void HandleVisualMaterial() {
        ballVisualTransform.GetComponent<Renderer>().material = DataManager.Instance.GetSelectedBallTexture();
    }

    protected override void OnSwipeSetVector(SwipeDirection swipeDirection) {
        base.OnSwipeSetVector(swipeDirection);
        SetRotationVector(swipeDirection);
    }


    void FixedUpdate() {
        HandleMovement();
        UpdateVisual();
    }

    protected override void HandleInteraction(out bool canMove) {
        canMove = false;
        if (Physics.Raycast(transform.position, directionVector, out RaycastHit hit, ballRadius + 0.5f, ~wallLayer)) {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.CompareTag(COIN_TAG)) {
                Destroy(hitObject);
                canMove = true;
            }
            else if (hitObject.CompareTag(BREAKABLE_TAG)) {
                Destroy(hitObject,0.02f);
                FXManager.Instance.ShowBreakableEffect(hitObject.transform.position);
                // canMove = false; yazmaya gerek yok
            }
            else if (hitObject.TryGetComponent(out Hole hole)) {
                hole.UpdateVisual();
                if (!IsThereBall())
                   gameManager.State = GameState.WinLevel;
                Destroy(gameObject);
            }
            else if (hitObject.CompareTag(KILLER_TAG)) {
                gameManager.State = GameState.LoseLevel;
                FXManager.Instance.ShowDestroyEffect(transform.position);
                Destroy(gameObject,0.1f);
                Debug.Log("Game is over");
            }
        }
    }

    public static bool IsThereBall() {
        totalBallNumber--;
        if (totalBallNumber==0) {
            return false;
        }
        return true ;
    }

    public static void ResetBallNumber() {
        totalBallNumber = 0;
    }

    private void UpdateVisual() {
        if (IsMoving()) {
            ballVisualTransform.Rotate(rotationSpeed * Time.deltaTime * rotationVector, Space.World);
        }
    }

    private void SetRotationVector(SwipeDirection swipeDirection) {
        switch (swipeDirection) {
            case SwipeDirection.Left:
                rotationVector = Vector3.forward;
                break;
            case SwipeDirection.Right:
                rotationVector = Vector3.back;
                break;
            case SwipeDirection.Up:
                rotationVector = Vector3.right;
                break;
            case SwipeDirection.Down:
                rotationVector = Vector3.left;
                break;
            default:
                rotationVector = Vector3.zero;
                break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * ballRadius);
    }
}