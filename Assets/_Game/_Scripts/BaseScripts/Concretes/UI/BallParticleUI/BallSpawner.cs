using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAG.Utility;
public class BallSpawner : MonoSingleton<BallSpawner>
{
    [Header("Ball Settings"), Space]
    public Transform target;
    public RectTransform rectTarget;
    [SerializeField]
    private int ballPoolSize;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private RectTransform ballsHolder;

    [SerializeField]
    private Canvas particleCanvas;

    [SerializeField]
    private Camera particleCamera;

    Queue<GameObject> ballPool = new();
    private void Awake()
    {
        ballPool = new Queue<GameObject>();

        for (int i = 0; i < ballPoolSize; i++)
        {
            GameObject willCreateCoin = Instantiate(ballPrefab, ballsHolder);
            willCreateCoin.SetActive(false);
            ballPool.Enqueue(willCreateCoin);
        }
    }
    public void SpawnBallPieceToAchivement(int comboCount,Vector3 spawnWordPosition)
    {
        Vector3 targetScreenPos = rectTarget.localPosition;
        Vector2 targetCanvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ballsHolder, targetScreenPos, particleCamera, out targetCanvasPos);

        int ballCount = comboCount;

        for (int i = 0; i < ballCount; i++)
        {
            GameObject obj = ballPool.Dequeue();
            BallPiece pieceInfo = obj.GetComponent<BallPiece>();
            pieceInfo.targetPos = targetCanvasPos;
            pieceInfo.target = target;
            pieceInfo.canvas = particleCanvas;

            RectTransform uiElementRectTransform = obj.GetComponent<RectTransform>();
            // Get the position of the 3D object in world space
            Vector3 objectWorldPos = spawnWordPosition;

            // Get the screen position of the 3D object
            Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(objectWorldPos);

            // Convert the screen position to a canvas position
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ballsHolder, objectScreenPos, particleCamera, out canvasPos);

            // Set the position of the UI element to the position in the parent transform
            uiElementRectTransform.localPosition = canvasPos;

            obj.SetActive(true);
            ballPool.Enqueue(obj);
        }
    }

}
