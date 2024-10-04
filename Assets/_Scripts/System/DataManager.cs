using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{
    protected GameData currentGameData;
    public List<GameData> AllSaveGameData { get; protected set; }
    List<IDataPersistence> dataPersistenceList = new();
    protected FileDataHandle dataHandle;

    protected override void Awake()
    {
        base.Awake();

        dataHandle = new FileDataHandle(Application.persistentDataPath, "SaveGame_");

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (currentGameData != null)
        {
            dataPersistenceList = FindAllIDataPersistences();
            LoadData();
        }
        else
        {
            LoadAllSaveGame();
        }
    }

    public void OnSceneUnload(Scene scene)
    {
        SaveData();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    public void NewGame() => currentGameData = new GameData();

    public void SaveData()
    {
        foreach (IDataPersistence dataPersistence in dataPersistenceList)
            dataPersistence.SaveData(currentGameData);

        dataHandle.Save(currentGameData);
    }

    public void LoadAllSaveGame() => AllSaveGameData = dataHandle.Load();

    public void LoadSaveGame(int idSave) 
        => currentGameData = AllSaveGameData.First(x => x.id == idSave);

    public void LoadData()
    {
        if (currentGameData == null) currentGameData = new();

        foreach (IDataPersistence dataPersistence in dataPersistenceList)
            dataPersistence.LoadData(currentGameData);
    }

    protected List<IDataPersistence> FindAllIDataPersistences()
    {
        var allDataPersistences = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(allDataPersistences);
    }

    public void LoadScene()
    {
        if (LevelManager.Instance != null)
            if (LevelManager.Instance.TimeOfDay != currentGameData.timeOfDay)
                currentGameData.timeOfDay = LevelManager.Instance.TimeOfDay;

        if (currentGameData != null)
        {
            SceneManager.LoadScene(currentGameData.timeOfDay.ToString());
        }
    }

    

    private void OnApplicationQuit() => SaveData();
}
