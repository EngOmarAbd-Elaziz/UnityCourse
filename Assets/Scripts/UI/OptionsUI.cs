using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    // [SerializeField] private Button moveUpBtn;
    // [SerializeField] private Button moveDownBtn;
    // [SerializeField] private Button moveRightBtn;
    // [SerializeField] private Button moveLeftBtn;
    // [SerializeField] private Button interactBtn;
    // [SerializeField] private Button interactAltBtn;
    // [SerializeField] private Button pauseBtn;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    // [SerializeField] private TextMeshProUGUI moveUpTxt;
    // [SerializeField] private TextMeshProUGUI moveDownTxt;
    // [SerializeField] private TextMeshProUGUI moveRightTxt;
    // [SerializeField] private TextMeshProUGUI moveLeftTxt;
    // [SerializeField] private TextMeshProUGUI interactTxt;
    // [SerializeField] private TextMeshProUGUI interactAltTxt;
    // [SerializeField] private TextMeshProUGUI pauseTxt;

    // [SerializeField] private Transform pressToRebindKeyTransform;

    [SerializeField] private Image bgImage;

    private Action onCloseButtonAction;
    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            bgImage.gameObject.SetActive(false);
            onCloseButtonAction();
        });

        // moveUpBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.Move_Up);
        // });

        // moveDownBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.Move_Down);
        // });

        // moveRightBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.Move_Right);
        // });

        // moveLeftBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.Move_Left);
        // });

        // interactBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.Interact);
        // });

        // interactAltBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.InteractAlternate);
        // });

        // pauseBtn.onClick.AddListener(() =>
        // {
        //     RebindBinding(GameInput.Binding.Pause);
        // });

    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        bgImage.gameObject.SetActive(false);
        // HidePressToRebindKey();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
        bgImage.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10.0f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10.0f);

        //     moveUpTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        //     moveDownTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        //     moveRightTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        //     moveLeftTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        //     interactTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        //     interactAltTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        //     pauseTxt.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        bgImage.gameObject.SetActive(true);
        // soundEffectsButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // private void ShowPressToRebindKey()
    // {
    //     pressToRebindKeyTransform.gameObject.SetActive(true);
    // }

    // private void HidePressToRebindKey()
    // {
    //     pressToRebindKeyTransform.gameObject.SetActive(false);
    // }

    // private void RebindBinding(GameInput.Binding binding)
    // {
    //     ShowPressToRebindKey();
    //     GameInput.Instance.RebindBinding(binding, () =>
    //     {
    //         HidePressToRebindKey();
    //         UpdateVisual();
    //     });
    // }
}
