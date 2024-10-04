using System.IO;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FileDataHandle
{
    protected string filePath;
    protected string fileName;
    protected int maxQuantityFileSaves = 10;
    protected string encryptionCodeWord = "FC Nuoc";
    public FileDataHandle(string filePath, string fileName)
    {
        this.filePath = filePath;
        this.fileName = fileName;
    }
    public void Save(GameData gameData)
    {
        if (gameData == null) return;

        int id = gameData.id;

        string[] allSaves = Directory.GetFiles(filePath, $"{fileName}*");
        if (allSaves.Length < 1) id = 0;
        if (id < 0) FindEmptyIdSave(ref id, allSaves);

        gameData.id = id;

        string idSave = id < 10 ? "0" + id : id.ToString();
        string fullPath = Path.Combine(filePath, fileName + idSave + ".fcn");

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(gameData, true);
            dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream steam = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(steam))
                {
                    sw.Write(dataToStore);
                }
            }
        }

        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void FindEmptyIdSave(ref int id, string[] allSaves)
    {
        for (int i = 0; i < maxQuantityFileSaves; i++)
        {
            if (i >= allSaves.Length)
            {
                id = i;
                return;
            }
            if (allSaves[i].EndsWith(i.ToString()))
            {
                id = i;
                return;
            }
        }
    }

    public List<GameData> Load()
    {
        List<GameData> loadData = new();

        if (Directory.Exists(filePath))
        {
            try
            {
                string[] allSaves = Directory.GetFiles(filePath, $"{fileName}*");
                if (allSaves.Length < 1 ) return loadData;

                foreach( string savePath in allSaves)
                {
                    string dataToLoad = "";
                    using (FileStream steam = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(steam))
                        {
                            dataToLoad = sr.ReadToEnd();
                        }
                    }

                    dataToLoad = EncryptDecrypt(dataToLoad);
                    loadData.Add(JsonUtility.FromJson<GameData>(dataToLoad));
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        return loadData;
    }
    public GameData LoadById(int id)
    {
        string idSave = id < 10 ? "0" + id : id.ToString();
        string fullPath = Path.Combine(filePath, fileName + idSave + ".fcn");


        GameData loadData = new();
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream steam = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(steam))
                    {
                        dataToLoad = sr.ReadToEnd();
                    }
                }

                dataToLoad = EncryptDecrypt(dataToLoad);
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        return loadData;
    }
    public void UpdateCurrentSave(int id)
    {
        if (id < 0) return;
    }

    protected string EncryptDecrypt(string data)
    {
        //string modifiedData = "";
        //for(int i = 0; i < data.Length; i++)
        //{
        //    modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        //}
        //return modifiedData;
        return data;
    }
}
