using UnityEngine;
using TMPro;

public class Translator : MonoBehaviour
{
    private TMP_Text value;

    private string textEn;
    [TextArea] public string textUa;
    [TextArea] public string textRu;

    private void Start()
    {
        value = GetComponent<TMP_Text>();
        MainMenuMeneger.onLanguageChange.AddListener(TranslateText);
        textEn = value.text;
        TranslateText();
    }

    private void TranslateText()
    {
        string language = SaveSystem.Instante.Save.language;

        if (language == "ua")
        {
            value.text = textUa;
        }
        else if (language == "ru")
        {
            value.text = textRu;
        }
        else if (language == "en")
        {
            value.text = textEn;
        }
    }
}
