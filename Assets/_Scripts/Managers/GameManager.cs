using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    ReadyToStart,
    WaitingInput,
    Moving,
    WinLevel,
    LoseLevel,
    Pause
}

//[DefaultExecutionOrder(-2)]
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public event Action OnGameFinished;
    public event Action OnMovementCompleted;

    private List<MovableObject> movableObjectList;

    private GameState _state;
    public GameState State
    {

        get { return _state; }

        set {
            _state = value;
            if (_state == GameState.WinLevel ||
            _state == GameState.LoseLevel)
                OnGameFinished?.Invoke();
        }
    }

    private void Awake() {
        Instance = this;
        movableObjectList = new();
        State = GameState.ReadyToStart;
    }

    private void Start() {
        SwipeDetector.Instance.OnSwipeDetected += (swipeDirection) => State = GameState.Moving;
    }

    private bool IsThereMovement() {
        foreach (MovableObject movableObject in movableObjectList) {
            if (movableObject.IsMoving())
                return true;
        }
        return false;
    }

    public void UpdateState() {
        // Every time when a ball destoryed this func will be called
        if (State == GameState.Moving && !IsThereMovement()) {
            State = GameState.WaitingInput;
            OnMovementCompleted?.Invoke();
        }

    }

    public void AddToMovableObjectList(MovableObject movableObject) {
        movableObjectList.Add(movableObject);
    }
    public void RemoveFromMovableObjectList(MovableObject movableObject) {
        movableObjectList.Remove(movableObject);
    }
}