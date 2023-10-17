using System;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
    [SerializeField, Range(0, 2)] private float acceleration = .8f;
    [SerializeField] private float maximumSpeed;

    protected GameManager gameManager;
    protected Vector3 directionVector { get; private set; }

    private float defaultControlDistance;
    private Vector3 velocity;

    // child'ın start'ında çağrılacak.
    protected void Init(float defaultControlDistance) {
        this.defaultControlDistance = defaultControlDistance;
        gameManager = GameManager.Instance;
        gameManager.AddToMovableObjectList(this);
        SwipeDetector.Instance.OnSwipeDetected += swipe => OnSwipeSetVector(swipe);
    }

    private void OnDestroy() {
        // anladığım kadarıyla parent class'da OnDestroy yazarsak
        // child class için de geçerli olmuş oluyor
        // ama child'da tekrar OnDestroy yazarsak basedeki çalışıyor.
        gameManager.RemoveFromMovableObjectList(this);
        gameManager.UpdateState();
        SwipeDetector.Instance.OnSwipeDetected -= swipe => OnSwipeSetVector(swipe);
    }

    protected virtual void OnSwipeSetVector(SwipeDirection swipeDirection) {
        SetDirectionVector(swipeDirection);
    }

    private void SetDirectionVector(SwipeDirection swipeDirection) {
        switch (swipeDirection) {
            case SwipeDirection.Left:
                directionVector = Vector3.left;
                break;
            case SwipeDirection.Right:
                directionVector = Vector3.right;
                break;
            case SwipeDirection.Up:
                directionVector = Vector3.forward;
                break;
            case SwipeDirection.Down:
                directionVector = Vector3.back;
                break;
            default:
                directionVector = Vector3.zero;
                break;
        }
    }

    // this method will be called in FixedUpdate() 
    protected void HandleMovement() {
        // center is the transform.position
        if (gameManager.State == GameState.Moving && CanMove()) {
            // this block runs if there is no collision
            Move();
        }
        else if (IsMoving()) {
            // this block runs if object collides with anything
            HandleInteraction(out bool canMove);
            if (canMove) Move();
            else Stop();
        }
    }

    private void Move() {
        if (velocity.magnitude < maximumSpeed)
            velocity += acceleration * Time.fixedDeltaTime * directionVector;
        transform.position += velocity;
    }

    private void Stop() {
        SetFinalPosition();
        directionVector = Vector3.zero;
        velocity = Vector3.zero;
        gameManager.UpdateState();
    }

    private void SetFinalPosition() {
        Vector3 newPos = new(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
        transform.position = newPos;
    }
    protected abstract void HandleInteraction(out bool canMove);

    private bool CanMove() {
        //var controlDistance = velocity.magnitude > ballRadius ? velocity.magnitude : ballRadius;
        //float controlDistance = Mathf.Max(velocity.magnitude, defaultControlDistance);
        float controlDistance = velocity.magnitude + defaultControlDistance;
        return !Physics.Raycast(transform.position, directionVector, controlDistance);
        //return !Physics.Raycast(center, movementVector, controlDistance,Physics.DefaultRaycastLayers , QueryTriggerInteraction.Ignore);
    }

    public bool IsMoving() {
        return directionVector != Vector3.zero;
    }

    //Version 2
    //private void SetFinalPosition() {
    //    if (directionVector == Vector3.right || directionVector == Vector3.left) {
    //        Vector3 newPos = transform.position;
    //        newPos.x = Mathf.Round(newPos.x);
    //        transform.position = newPos;
    //    }

    //    else if (directionVector == Vector3.forward || directionVector == Vector3.back) {
    //        Vector3 newPos = transform.position;
    //        newPos.z = Mathf.Round(newPos.z);
    //        transform.position = newPos;
    //    }
    //}

    //Version 1
    //private void SetFinalPosition() {
    //    if (directionVector == Vector3.right ) {
    //        Vector3 newPos = transform.position;
    //        newPos.x = Mathf.Ceil(newPos.x);
    //        transform.position = newPos;
    //    }
    //    else if (directionVector == Vector3.left) {
    //        Vector3 newPos = transform.position;
    //        newPos.x = Mathf.Floor(newPos.x);
    //        transform.position = newPos;
    //    }
    //    else if (directionVector == Vector3.forward) {
    //        Vector3 newPos = transform.position;
    //        newPos.z = Mathf.Ceil(newPos.z);
    //        transform.position = newPos;
    //    }
    //    else if (directionVector == Vector3.back) {
    //        Vector3 newPos = transform.position;
    //        newPos.z = Mathf.Floor(newPos.z);
    //        transform.position = newPos;
    //    }
    //}
}
