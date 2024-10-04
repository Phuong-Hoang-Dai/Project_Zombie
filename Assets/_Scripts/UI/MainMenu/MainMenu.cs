using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : Singleton<MainMenu>
{
    public Transform LoadGame;
    public Transform Menu;
    public ButtonLoad[] Save;

    protected override void Awake()
    {
        base.Awake();

        LoadGame = transform.Find("LoadGame");
        Menu = transform.Find("Menu");

        Save = GetComponentsInChildren<ButtonLoad>();
    }

    private void Start()
    {
        Menu.gameObject.SetActive(true);
        LoadGame.gameObject.SetActive(false);
        DataManager.Instance.LoadAllSaveGame();


        for (int i = 0; i < DataManager.Instance.AllSaveGameData.Count; i++)
        {
            if (i > 1) break;
            GameData gameData = DataManager.Instance.AllSaveGameData[i];

            Save[i].Text.text = $"Day {gameData.day} - {gameData.timeOfDay}";

            Save[i].id = gameData.id;
        }
    }

    public void NewGame()
    {
        DataManager.Instance.NewGame();
        DataManager.Instance.LoadScene();
    }

    public void Load()
    {
        Menu.gameObject.SetActive(false);
        LoadGame.gameObject.SetActive(true);
    }

    public void Back()
    {
        Menu.gameObject.SetActive(true);
        LoadGame.gameObject.SetActive(false);
    }

    public void LoadSave(int idSave)
    {
        DataManager.Instance.LoadSaveGame(idSave);
        DataManager.Instance.LoadScene();
    }

    public void SaveGame()
    {
        DataManager.Instance.SaveData();
    }

    public void QuitGame() => Application.Quit();
}
