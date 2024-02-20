using DG.Tweening;
using PAG.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public bool FirstTime
    {
        get => ES3.Load("FirstTime", true);
        set => ES3.Save("FirstTime", value);
    }
    public float RewardSpawnSpeed { get; private set; } = 1f;
    public bool CanSpawn { get; set; } = true;
    public static SpawnController Instance { get; private set; }
    public float SpawnDuration
    {
        get => ES3.Load(Consts.SPAWN_DURATION, defaultSpawnDuration);
        set => ES3.Save(Consts.SPAWN_DURATION, value);
    }
    public List<GameObject> fruitsToSpawn = new List<GameObject>();
    public List<GameObject> fruitsInScene = new List<GameObject>();

    public int maxCount;
    public int Missing;

    public float spawnXMax;
    public float spawnXMin;
    public float spawnZMax;
    public float spawnZMin;

    [SerializeField]
    private float defaultSpawnDuration;

    InGamePanelController inGamePanelController;
    GroundSizeController groundSizeController;
    float currentTime;
    private void Awake()
    {
        inGamePanelController = FindObjectOfType<InGamePanelController>();
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        Application.targetFrameRate = 60;
    }
    public int OnboardCount = 3;
    private void Start()
    {
        Missing = LevelManager.Instance.SpawnerMaxCount - LevelManager.Instance.SpawnerCurrentCount;
        //Debug.Log(Missing);
        if (LevelManager.Instance.SpawnerCurrentCount == LevelManager.Instance.SpawnerMaxCount)
        {
            inGamePanelController.SpawnerFillingImage(1, 1);
        }
        if (FirstTime)
        {
            CanSpawn = false;
            SpawnOnboardFruit();
        }
        else
        {
            SpawnRandomFruit(fruitsToSpawn, LevelManager.Instance.SpawnerCurrentCount);
        }

        HandleTexts();
    }
    int dir = 1;
    public DirectionController dirCont;
    public void SpawnOnboardFruit()
    {
        if (OnboardCount > 0)
        {
            OnboardCount -= 1;
            Vector3 spawnPos = GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.right * 6 * dir;
            dir *= -1;
            dirCont.SwitchDirectionUI();
            if (dir % 2 != 0) dir *= 2;
            SpawnRandomFruit(fruitsToSpawn, 1, false, spawnPos);
        }
        else
        {
            FirstTime = false;
            CanSpawn = true;
            BurstManager.Instance.HandleUnonboard();
        }
            
    }
    private void Update()
    {

        if (!CanSpawn) return;

        // For each missing fruit, spawn one.
        if (Missing > 0 || LevelManager.Instance.SpawnerMaxCount - fruitsInScene.Count > 0)
        {
            // Fill the spawner bar.
            inGamePanelController.SpawnerFillingImage(Time.time - currentTime, SpawnDuration * RewardSpawnSpeed);


            // If the time is up, spawn a random fruit.
            if (Time.time - currentTime > SpawnDuration * RewardSpawnSpeed)
            {
                currentTime = Time.time;
                SpawnRandomFruit();
                HandleTexts();
            }
        }
        else
        {
            // Reset timer.
            currentTime = Time.time;
        }
    }
    public void GetSpawnSpeedReward(float ratio, float time)
    {
        StartCoroutine(RewardSpawnSpeedRoutine(ratio, time));
    }
    IEnumerator RewardSpawnSpeedRoutine(float ratio, float time)
    {
        RewardSpawnSpeed *= ratio;
        yield return new WaitForSeconds(time);
        RewardSpawnSpeed = 1f;
    }
    public void HandleTexts()
    {
        // Assign fruit count for text to use.
        LevelManager.Instance.SpawnerCurrentCount = fruitsInScene.Count;

        //Update fruitCount/Total text.
        inGamePanelController.SpawnerTexting();
    }
    public void RemoveFruit(GameObject fruit)
    {
        if (fruitsInScene.Contains(fruit))
        {
            fruitsInScene.Remove(fruit);
            Missing += 1;
            HandleTexts();
        }
    }
    public void SpawnRandomFruit()
    {
        SpawnRandomFruit(fruitsToSpawn);
    }
    public void SpawnRandomFruit(List<GameObject> spawnableFruits, int spawnCount = 1, bool isRand = true, Vector3 spawnPos = default)
    {
        // Force fill bar to be full.
        inGamePanelController.SpawnerFillingImage(1, 1);

        float xMinBorder = spawnXMin;
        float xMaxBorder = spawnXMax;
        float zMinBorder = spawnZMin;
        float zMaxBorder = spawnZMax;

        if (groundSizeController == null)
        {
            groundSizeController = FindObjectOfType<GroundSizeController>();
        }

        if (groundSizeController != null)
        {
            float xSize = groundSizeController.GetScaleValue().x;
            float zSize = groundSizeController.GetScaleValue().z;

            xMinBorder *= xSize;
            xMaxBorder *= xSize;
            zMinBorder *= zSize;
            zMaxBorder *= zSize;
        }
        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex;
            if (LevelManager.Instance.PlayerLevel <= 20)
            {
                randomIndex = Random.Range(0, LevelManager.Instance.PlayerLevel);
            }
            else
            {
                randomIndex = Random.Range(0, 20);
            }

            float randX = Random.Range(xMinBorder, xMaxBorder);
            float randZ = Random.Range(zMinBorder, zMaxBorder);
            Vector3 pos = new Vector3(randX, 2f, randZ);
            if (!isRand)
            {
                pos = spawnPos;
            }
            GameObject p = Instantiate(spawnableFruits[randomIndex], pos, spawnableFruits[randomIndex].transform.rotation, transform);
            //GameObject p = PoolManager.Instance.DequeueFromPool(spawnableFruits[randomIndex].name, pos, Quaternion.identity);
            p.transform.localScale *= p.GetComponent<BurstHandler>().fruitUpgradeObject.fruitSize;
            inGamePanelController.GenerateSpawnImage();
            //Debug.Log(p.GetComponent<BurstHandler>().fruitUpgradeObject.fruitSize);
            fruitsInScene.Add(p);
            Missing -= 1;
            //FloatingTextController.Instance.SpawnFloatingText(inGamePanelController.spawnerImage.transform.position, 0, SpriteType.Fruit, true);
        }
    }
    public Vector3 GetRandomSpawnPosition()
    {
        // We need to know our boundaries.
        return Vector3.zero;
    }
}

[CreateAssetMenu(fileName = "FruitsContainer", menuName = "Scriptable Objects/Fruits Data")]
public class FruitSpawnData : ScriptableObject
{
    public List<GameObject> Fruits = new();
}