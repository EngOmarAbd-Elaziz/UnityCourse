using UnityEngine;
using TMPro;
using System;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpTxt;
    [SerializeField] private TextMeshProUGUI moveDownTxt;
    [SerializeField] private TextMeshProUGUI moveRightTxt;
    [SerializeField] private TextMeshProUGUI moveLeftTxt;
    [SerializeField] private TextMeshProUGUI interactTxt;
    [SerializeField] private TextMeshProUGUI interactAltTxt;
    [SerializeField] private TextMeshProUGUI pauseTxt;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        moveUpTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveRightTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        moveLeftTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        interactTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
