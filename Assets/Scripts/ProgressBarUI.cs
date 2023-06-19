using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter counter;
    [SerializeField] private GameObject backgroundBar;
    [SerializeField] private Image progressBar;

    private void Start()
    {
        counter.ChopEvent += CuttingCounter_OnChopEvent;
    }

    private void CuttingCounter_OnChopEvent(object sender, CuttingCounter.OnChopEventArgs e)
    {
        int maxProgress = e.kitchenObject.GetMaxChopProgress();
        int progress = e.kitchenObject.GetChopProgress();
        float progressNormalized = (float)progress / maxProgress;

        if (e.kitchenObject)
        {
            if (progress < maxProgress)
            {
                progressBar.fillAmount = progressNormalized;
                Show();
            }
            else
            {
                Hide();
            }
        }
        else
        {
            Hide();
        }

    }

    private void Show()
    {
        backgroundBar.SetActive(true);
    }
    private void Hide()
    {
        backgroundBar.SetActive(false);
    }
}
