using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class GroundSizeController : MonoBehaviour
{
    public Vector3 CurrentScale => currentScale;
    public int ScaleLevel
    {
        get => ES3.Load(Consts.GROUND_SCALE_LEVEL, 0);
        set
        {
            ES3.Save(Consts.GROUND_SCALE_LEVEL, value);
            ScalePlatform();
        }
    }
    [Header("Data Settings"), Space]
    public GroundScaleData[] scaleDatas;
    public MultiObjectCameraController camController;

    [Header("Scaling Settings"), Space]
    public Transform platormToScale;
    public float scaleDuration = 1.0f;
    public Ease scaleEase = Ease.OutQuint;

    private Vector3 currentScale;
    PlayerController playerController;
        HandScaleHandler scaleHandler;
    private void Start()
    {
        if(camController == null)
        {
            camController = FindObjectOfType<MultiObjectCameraController>();
        }
        if(playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>(); 
            scaleHandler = playerController.GetComponent<HandScaleHandler>();
        }
        ScalePlatform();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Upgrade();
        }
    }
    [Button("Upgrade")]
    public void Upgrade()
    {
        ScaleLevel += 1;
    }
    public void ScalePlatform()
    {
        StartCoroutine(ScaleWallsCoroutine());
    }
    int GetScaleLevel()
    {
        if (scaleDatas == null || scaleDatas.Length == 0)
        {
            return 0;
        }

        int level = ScaleLevel;
        // Clamp level value.
        level = Mathf.Min(level, scaleDatas.Length - 1);
        level = Mathf.Max(0, level);
        return level;
    }
    public float GetCamSize()
    {
        if (scaleDatas == null || scaleDatas.Length == 0)
        {
            return 22f;
        }
        return scaleDatas[GetScaleLevel()].camSize;
    }

    public Vector3 GetScaleValue()
    {
        if (scaleDatas == null || scaleDatas.Length == 0)
        {
            return Vector3.one;
        }

        currentScale = scaleDatas[GetScaleLevel()].scale;
        return currentScale;
    }
    public float playerPushSpeed;
    private IEnumerator ScaleWallsCoroutine()
    {
        playerController.CanMove = false; 
        yield return StartCoroutine(WallController.Instance.ResetWallsPerFrame());

        Camera.main.DOOrthoSize(GetCamSize(), scaleDuration)
            /*.OnUpdate(() => {
                if (playerController.isCollidingWall)
                {
                    // Calculate the amount to move the player
                    float moveAmount = Time.deltaTime * playerPushSpeed; // Adjust moveSpeed as needed

                    // Move the player backward along the camera's forward direction
                    playerController.transform.Translate(-playerController.transform.forward * moveAmount, Space.World);
                }
            })*/;
        if (playerController.isCollidingWall)
        {
            
            float handScale = scaleHandler.GetCurrentScale();
            scaleHandler.FreezeHands();
            handScale = Mathf.Min(1f, handScale);
            // Move the player backward along the camera's forward direction
            Vector3 targetPos = playerController.transform.position - playerController.transform.forward * playerPushSpeed * handScale;
            playerController.transform.DOMove(targetPos, scaleDuration / 3f * 2f);
            //playerController.transform.Translate(-playerController.transform.forward * playerPushSpeed, Space.World);
        }

        yield return platormToScale.DOScale(GetScaleValue(), scaleDuration).SetEase(scaleEase).WaitForCompletion();

        WallController.Instance.InitializeWalls();
        playerController.CanMove = true;
        scaleHandler.UnfreezeHands();
    }

    private IEnumerator ScaleWallsCoroutine(Vector3 newScale)
    {
        yield return StartCoroutine(WallController.Instance.ResetWallsPerFrame());

        yield return platormToScale.DOScale(newScale, scaleDuration).SetEase(scaleEase).WaitForCompletion();

        WallController.Instance.InitializeWalls();
    }
    public void ChangeScaleLevel(int newLevel)
    {
        ScaleLevel = newLevel;
    }
    public void IncreaseScaleLevel()
    {
        ScaleLevel += 1;
    }
}