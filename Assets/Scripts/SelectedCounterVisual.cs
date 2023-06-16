using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private _BaseCounter counter;
    [SerializeField] private GameObject[] selectedCounterVisualArray;

    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += PlayerController_OnSelectedCounterChanged;
    }

    private void PlayerController_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == counter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject selectedCounterVisual in selectedCounterVisualArray)
        {
            selectedCounterVisual.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject selectedCounterVisual in selectedCounterVisualArray)
        {
            selectedCounterVisual.SetActive(false);
        }
    }
}
