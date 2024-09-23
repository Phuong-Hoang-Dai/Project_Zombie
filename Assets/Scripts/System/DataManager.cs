using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    protected GameData currentGameData;
    protected List<GameData> allSaveGameData;
    List<IDataPersistence> dataPersistenceList;
    protected FileDataHandle dataHandle;

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Only 1 dataperistence at a time");
        Instance = this;

        dataHandle = new FileDataHandle(Application.persistentDataPath, "SaveGame_");
    }

    private void Start()
    {
        dataPersistenceList = FindAllIDataPersistences();
        LoadAllSaveGame();
        if(allSaveGameData.Count < 1) NewGame();
        else currentGameData = allSaveGameData[0];

        LoadData();
    }

    public void NewGame()
    {
        currentGameData = new GameData();
    }

    public void SaveData()
    {

        foreach (IDataPersistence dataPersistence in dataPersistenceList)
            dataPersistence.SaveData(currentGameData);

        dataHandle.Save(currentGameData);
    }

    public void LoadAllSaveGame()
    {
        allSaveGameData = dataHandle.Load();
    }

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

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
