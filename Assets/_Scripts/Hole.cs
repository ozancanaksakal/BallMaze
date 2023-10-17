using UnityEngine;

public class Hole : MovableObject
{
    private const string COIN_TAG = "Coin";
    private const string BALL_TAG = "Ball";

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject[] ballVisualList;

    private float colliderWidth;
    private int ballVisualIndex = 0;

    void Start() {
        var myCollider = GetComponent<BoxCollider>();
        colliderWidth = myCollider.size.x / 2;
        Init(colliderWidth);
        HandleBallVisualsMaterial();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleBallVisualsMaterial() {
        foreach (GameObject ballVisual in ballVisualList) {
            ballVisual.GetComponent<Renderer>().material = DataManager.Instance.GetSelectedBallTexture();
        }
    }

    public void UpdateVisual() {
        ballVisualList[ballVisualIndex].SetActive(true);
        ballVisualIndex = ballVisualIndex == ballVisualList.Length ? ballVisualIndex : ballVisualIndex + 1;
        FXManager.Instance.ShowHoleEffect(transform.position);
    }

    protected override void HandleInteraction(out bool canMove) {
        canMove = false;
        if (Physics.Raycast(transform.position, directionVector, out RaycastHit hit, colliderWidth + 0.5f, ~wallLayer)) {
            // this code runs if hole collide something except walls

            var hitObjectTransform = hit.transform;

            if (hitObjectTransform.CompareTag(COIN_TAG)) {
                canMove = true;
            }
            else if (hitObjectTransform.CompareTag(BALL_TAG)) {
                Destroy(hitObjectTransform.gameObject);
                UpdateVisual();
                canMove = true;
                if (!Ball.IsThereBall()) {
                    transform.position = hitObjectTransform.position;
                    gameManager.State = GameState.WinLevel;
                }
            }
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * colliderWidth);
    }
}