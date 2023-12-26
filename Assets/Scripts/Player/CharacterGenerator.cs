using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private MainMenuMeneger _MainMenuMeneger;
    [SerializeField] private CharacterMeneger _characterMeneger;

    public void GenerateCharacter()
    {
        GameObject character = Instantiate(_characterMeneger.characters[SaveSystem.Instante.Save.idChoosedCharacter].pref, transform.parent);
        if (_MainMenuMeneger != null) _MainMenuMeneger.InitCharacterGenerator(character);
        Destroy(this.gameObject);
    }
}
