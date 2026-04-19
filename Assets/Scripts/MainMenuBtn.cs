using UnityEngine;
using UnityEngine.UI;

public class MainMenuBtn : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
{
    Loader.Load(Loader.Scene.MainMenuScene);
});
    }
}
