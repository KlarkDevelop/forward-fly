using UnityEngine;
using UnityEngine.UI;

public class trainingCursor : MonoBehaviour
{
    private Image _image;
    public bool DoDoubleClick = false;
    public bool inMenu = true;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
        if (inMenu)
        {
            if (SaveSystem.Instante.Save.trainingInMenu == false && SaveSystem.Instante.Save.coins >= 10)
            {
                _image.enabled = true;
                if (DoDoubleClick == true)
                {
                    GetComponent<Animator>().Play("doubleClick");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (SaveSystem.Instante.Save.trainingInGame == false)
            {
                if (DoDoubleClick == true)
                {
                    GetComponent<Animator>().Play("doubleClick");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
