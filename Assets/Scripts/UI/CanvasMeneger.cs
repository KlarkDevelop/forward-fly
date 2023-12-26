using UnityEngine;

public class CanvasMeneger : MonoBehaviour
{
    [SerializeField] private GameObject playCanvas;
    [SerializeField] private GameObject pauseCanvas;

    private void Start()
    {
        playCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        Player.onDeath.AddListener(ShowDeathWindow);
        Player.onBombPickuped.AddListener(ShowTraining);
        Player.onDoubleTapJoystick.AddListener(EndTraining);
        RewordedAd.onContinueAdComplet.AddListener(ContinueGame);

        if (SaveSystem.Instante.Save.language == "en")
        {
            WindiwTitle.text = "PAUSE";
        }
        else if (SaveSystem.Instante.Save.language == "ru")
        {
            WindiwTitle.text = "Пауза";
        }
        else if (SaveSystem.Instante.Save.language == "ua")
        {
            WindiwTitle.text = "Пауза";
        }
    }

    public void TogglePause()
    {
        if (pauseCanvas.activeInHierarchy == false)
        {
            pauseCanvas.SetActive(true);
            playCanvas.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            pauseCanvas.SetActive(false);
            playCanvas.SetActive(true);
            Time.timeScale = 1;
        }
    }

    [Header("Death window settings")]
    [SerializeField] private TMPro.TMP_Text WindiwTitle;
    [SerializeField] private GameObject ContinueButt;
    [SerializeField] private GameObject ContinueAdButt;
    [SerializeField] private GameObject RestartButt;

    private void ShowDeathWindow()
    {
        playCanvas.SetActive(false);
        if (SaveSystem.Instante.Save.language == "en")
        {
            WindiwTitle.text = "GAME OVER";
        }
        else if (SaveSystem.Instante.Save.language == "ru")
        {
            WindiwTitle.text = "Игра окончена";
        }
        else if (SaveSystem.Instante.Save.language == "ua")
        {
            WindiwTitle.text = "Гру закінчено";
        }

        ContinueButt.SetActive(false);
        ContinueAdButt.SetActive(true);
        pauseCanvas.SetActive(true);
        RestartButt.SetActive(true);
    }

    private void ContinueGame()
    {
        playCanvas.SetActive(true);
        pauseCanvas.SetActive(false);

        if (SaveSystem.Instante.Save.language == "en")
        {
            WindiwTitle.text = "PAUSE";
        }
        else if (SaveSystem.Instante.Save.language == "ru")
        {
            WindiwTitle.text = "Пауза";
        }
        else if (SaveSystem.Instante.Save.language == "ua")
        {
            WindiwTitle.text = "Пауза";
        }

        ContinueButt.SetActive(true);
        ContinueAdButt.SetActive(false);
        RestartButt.SetActive(false);
    }

    [Header("Training")]
    [SerializeField] private trainingCursor cursor;
    
    private void ShowTraining()
    {
        if(cursor != null)
        {
            cursor.GetComponent<UnityEngine.UI.Image>().enabled = true;
            Time.timeScale = 0.5f;
        }
    }

    private void EndTraining(Player pl)
    {
        if (cursor != null)
        {
            Time.timeScale = 1f;
            SaveSystem.Instante.Save.trainingInGame = true;
            Destroy(cursor.gameObject);
        }
    }
}
