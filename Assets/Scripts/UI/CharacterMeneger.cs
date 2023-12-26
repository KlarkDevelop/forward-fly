using System.Collections.Generic;
using UnityEngine;

public class CharacterMeneger : MonoBehaviour
{
    [SerializeField] private GameObject[] prefubs;
    [SerializeField] private CharacterGenerator _characterGenerator;
    [HideInInspector] public List<Character> characters = new List<Character>();

    [HideInInspector] public int choosedCharacterId;
    [HideInInspector] public bool allCharacterIsOpened;

    [HideInInspector] public List<Character> openedCharacters = new List<Character>();

    [HideInInspector] public bool charactersLoaded = false;

    public void loadCharacters()
    {
        for (int i = 0; i < prefubs.Length; i++)
        {
            Character newCharacter = new Character();
            newCharacter.id = i;
            newCharacter.pref = prefubs[i];
            newCharacter.isOpened = false;
            for (int b = 0; b < SaveSystem.Instante.Save.idOpenedCharacters.Count; b++)
            {
                if (SaveSystem.Instante.Save.idOpenedCharacters[b] == newCharacter.id)
                {
                    newCharacter.isOpened = true;
                    break;
                }
            }
            characters.Add(newCharacter);
        }
        openedCharacters = GetCharactersByState(true);
        _characterGenerator.GenerateCharacter();
        allCharacterIsOpened = ChekAllCharacterIsOpened();
        charactersLoaded = true;
    }

    public List<Character> GetCharactersByState(bool state)
    {
        List<Character> listCharacters = new List<Character>();
        for(int i = 0; i < characters.Count; i++)
        {
            if (characters[i].isOpened == state)
            {
                listCharacters.Add(characters[i]);
                if(state == true)
                {
                    if(characters[i].id == choosedCharacterId)
                    {
                        Debug.Log($"ID choosed character {characters[i].id} | In array: {i}");
                        Character rep = listCharacters[0];
                        Character choosed = characters[choosedCharacterId];
                        listCharacters[0] = choosed;
                        listCharacters[listCharacters.Count - 1] = rep;
                        Debug.Log($"New 0 char: {listCharacters[0].id}");
                    }
                }
            } 
        }
        return listCharacters;
    }

    public Character GetRandomClosedCharacterForOpen()
    {
        List<Character> closedCharacters = GetCharactersByState(false);
        Character rndCharacter = closedCharacters[Random.Range(0, closedCharacters.Count)];
        closedCharacters.Remove(rndCharacter);
        rndCharacter.isOpened = true;


        openedCharacters.Add(rndCharacter);
        SaveSystem.Instante.Save.idOpenedCharacters.Add(rndCharacter.id);

        allCharacterIsOpened = ChekAllCharacterIsOpened();
        return rndCharacter;
    }

    private bool ChekAllCharacterIsOpened()
    {
        if (openedCharacters.Count == prefubs.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
