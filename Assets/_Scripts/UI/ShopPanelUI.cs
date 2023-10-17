using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelUI : MonoBehaviour
{
    [SerializeField] Transform buttons;

    [SerializeField] Button shopPanelBackButton;
    [SerializeField] BallTextureSOList ballTextureSOList;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject messagePanel;

    [SerializeField] GameObject buyMessageBox;
    [SerializeField] Button buyOKButton;
    [SerializeField] Button buyCancelButton;
    [SerializeField] GameObject cantBuyMessageBox;
    [SerializeField] Button cantBuyOKButton;
    
    private List<Transform> buttonTransformList;
    private int lastClickedButtonIndex;

    private void Start() {
        messagePanel.SetActive(false);
        buyMessageBox.SetActive(false);
        cantBuyMessageBox.SetActive(false);

        CreateButtonTransformList();

        shopPanelBackButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        HandlePanelButtonsOnStart();
        HandleMessageBoxButtonsOnStart();
        UpdateButtonsVisual();
    }


    private void CreateButtonTransformList() {
        buttonTransformList = new();
        foreach (Transform buttonTransform in buttons) {
            buttonTransformList.Add(buttonTransform);
        }
    }
    private void ShowMassagePanel() {
        messagePanel.SetActive(true);
        int textureCost = ballTextureSOList.list[lastClickedButtonIndex].cost;
        bool canBuy = DataManager.Instance.GetTotalCoin() >= textureCost;

        if (canBuy)
            buyMessageBox.SetActive(true);
        else
            cantBuyMessageBox.SetActive(true);
    }

    private void HandleMessageBoxButtonsOnStart() {

        buyOKButton.onClick.AddListener(() =>
        {
            //that means user bought texture
            BuyTexture();
            UpdateButtonsVisual();
            buyMessageBox.SetActive(false);
            messagePanel.SetActive(false);
        });
        buyCancelButton.onClick.AddListener(() =>
        {
            buyMessageBox.SetActive(false);
            messagePanel.SetActive(false);
        });
        cantBuyOKButton.onClick.AddListener(() =>
        {
            cantBuyMessageBox.SetActive(false);
            messagePanel.SetActive(false);
        });
    }

    private void BuyTexture() {
        DataManager.Instance.UpdateTotalCoin(-ballTextureSOList.list[lastClickedButtonIndex].cost);
        DataManager.Instance.OpenBallTexturesLock(lastClickedButtonIndex);
    }

    private void UpdateButtonsVisual() {
        BallTextureSO[] textureList = ballTextureSOList.list;
        coinText.text = DataManager.Instance.GetTotalCoin().ToString();

        for (int i = 0; i < textureList.Length; i++) {
            Transform buttonTransform = buttonTransformList[i];
            var buttonText = buttonTransform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var lockImage = buttonTransform.GetChild(1).gameObject;
            if (!DataManager.Instance.IsBallTextureOpen(i)) {
                buttonText.text = "Cost " + textureList[i].cost;
                lockImage.SetActive(true);
            }
            else if (i == DataManager.Instance.GetSelectedBallTextureIndex()) {
                buttonText.text = "Selected";
                lockImage.SetActive(false);
            }
            else {
                buttonText.text = "Owned";
                lockImage.SetActive(false);
            }
        }
    }

    private void HandlePanelButtonsOnStart() {

        for (int i = 0; i < ballTextureSOList.list.Length; i++) {

            Transform buttonTransform = buttonTransformList[i];
            int textureIndex = buttonTransformList.IndexOf(buttonTransform);
            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                lastClickedButtonIndex = textureIndex;
                // player doesnt own
                if (!DataManager.Instance.IsBallTextureOpen(textureIndex)) {
                    ShowMassagePanel();
                }
                // player owns
                else {
                    SelectTexture();
                    UpdateButtonsVisual();
                }
            });
        }
    }

    private void SelectTexture( ) {
        DataManager.Instance.SetSelectedBallTexture(lastClickedButtonIndex);
    }
}