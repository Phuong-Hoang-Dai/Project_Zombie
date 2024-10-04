using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, IDataPersistence 
{
    public Transform SpawnPoint;
    protected List<SpawnPoint> spawnPointList = new();
    public int Day {  get; protected set; }
    public TimeOfDay TimeOfDay { get; protected set; }
    protected int QuantityEnemy;
    public int MaxQuantityEnemy = 5;
    public int QuantityKill = 0;
    public GameObject ZombiePrefab;
    public GameObject StoreOwnerPrefab;

    [SerializeField]
    protected bool isMoved = false;

    public void LoadData(GameData gameData)
    {
        Day = gameData.day;
        TimeOfDay = gameData.timeOfDay;

        MaxQuantityEnemy += Day;
    }

    public void SaveData(GameData gameData)
    {
        gameData.day = Day;
        gameData.timeOfDay = TimeOfDay;
    }

    protected override void Awake()
    {
        base.Awake();
        spawnPointList = SpawnPoint.GetComponentsInChildren<SpawnPoint>().ToList();
    }

    public void ChangeTime()
    {
        if (TimeOfDay == TimeOfDay.Morning) TimeOfDay = TimeOfDay.Afternoon;
        else
        {
            TimeOfDay = TimeOfDay.Morning;
            Day += 1;
        }
    }

    public bool LevelIsCompleted() => QuantityKill >= MaxQuantityEnemy;
    public void UpdateQuantityKill() => QuantityKill += 1;

    public void RetryLevel()
    {
        PlayerController.Instance.Stats.CurrentHp = PlayerController.Instance.Stats.MaxHp;

        DataManager.Instance.LoadScene();
    }

    private void Start()
    {
        QuantityEnemy = MaxQuantityEnemy;

        SpawnPlayer();

        SpawnShopOwner();

        InvokeRepeating(nameof(SpawnEnemy),0 , 2f);
    }

    private void Update()
    {
        if(InputManager.Instance.MoveInput != Vector2.zero)
            isMoved = true;
        if(!isMoved)
            SpawnPlayer();
    }

    protected void SpawnShopOwner()
    {
        Vector3 spawnPoint = new();
        Quaternion rot_spawnPoint = new();
        string nameSpawnPoint;

        if (Day == 0 && TimeOfDay == TimeOfDay.Morning)
            nameSpawnPoint = "Newgame_StoreOwner";
        else if (TimeOfDay == TimeOfDay.Morning)
            return;
        else
            nameSpawnPoint = "StoreOwner";


        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (spawnPointList[i].NameSpawnPoint == nameSpawnPoint)
            {
                spawnPoint = spawnPointList[i].GetPositionSpawnPoint();
                rot_spawnPoint = spawnPointList[i].GetRotationSpawnPoint();
            }
        }

        Instantiate(StoreOwnerPrefab, spawnPoint, rot_spawnPoint);
    }

    protected void SpawnPlayer()
    {
        Vector3 spawnPoint = new();
        string nameSpawnPoint;

        if (Day == 0 && TimeOfDay == TimeOfDay.Morning)
            nameSpawnPoint = "Newgame_Player";
        else if (TimeOfDay == TimeOfDay.Morning)
            nameSpawnPoint = "Player";
        else
            nameSpawnPoint = "Safearea_Player";

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (spawnPointList[i].NameSpawnPoint == nameSpawnPoint)
            {
                spawnPoint = spawnPointList[i].GetPositionSpawnPoint();
            }
        }

        PlayerController.Instance.gameObject.transform.position = spawnPoint;
    }

    protected void SpawnEnemy()
    {
        if (QuantityEnemy <= 0) CancelInvoke(); 

        Vector3 spawnPoint = new();
        string nameSpawnPoint;

        if (Day == 0 && TimeOfDay == TimeOfDay.Morning)
        {
            nameSpawnPoint = "Newgame_Zombie";
            QuantityEnemy *= 2;
        }
        else if (TimeOfDay == TimeOfDay.Morning)
            nameSpawnPoint = "Zombie";
        else
            nameSpawnPoint = "";

        for (int i = 0; i < spawnPointList.Count; i++)
        {
            if (QuantityEnemy <= 0) break;
            if (spawnPointList[i].NameSpawnPoint == nameSpawnPoint)
            {
                QuantityEnemy -= 1;
                spawnPoint = spawnPointList[i].GetPositionSpawnPoint();

                Instantiate(ZombiePrefab, spawnPoint, ZombiePrefab.transform.rotation);
            }
        }
    }
}
public enum TimeOfDay
{
    Morning,
    Afternoon,
    Night
}
