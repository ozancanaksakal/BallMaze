using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event EventHandler<int> OnAnyCoinCollected;

    private static int totalCoinInLevel;
    private static int collectedCoin;

    [SerializeField] float rotSpeed = 5f;

    private void Start() {
        totalCoinInLevel++;
    }

    private void OnDestroy() {
        collectedCoin++;
        if (GameManager.Instance.State == GameState.Moving)
            OnAnyCoinCollected(this, collectedCoin);
    }

    public static int GetCollectedCoin () { return collectedCoin; }

    public static void ResetCoinNumber() { totalCoinInLevel = 0; collectedCoin = 0; }
    public static bool IsThereCoin() => totalCoinInLevel != collectedCoin;
 
    private void Update() {
        transform.Rotate(rotSpeed * Time.deltaTime * Vector3.up, Space.World);
    }
}