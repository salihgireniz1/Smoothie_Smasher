using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using PAG.Utility;

public class RewardedController : MonoSingleton<RewardedController>
{
    [SerializeField] private Rewarded _dnaRewareded;
    [SerializeField] private Rewarded _offerRewareded;
    [SerializeField] private Rewarded _frenzyRewareded;
    [SerializeField] private RewardedButton _rewardedButton;
    [SerializeField] private LayerMask _rewardedLayer;
    [SerializeField] private GameObject _adBreakGO;
    [SerializeField] private List<RewardedData> _rewardedDatas = new List<RewardedData>();

    Camera _camera;
    Vector3 _adBreakScale;

    private void Awake()
    {
        _camera = Camera.main;
        _adBreakScale = _adBreakGO.transform.localScale;
    }

    private void Start()
    {
        StartCoroutine(RewardedCoroutine());
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        bool isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject(); // Mobilde (0) yapılmalı

    //        if (!isPointerOverGameObject)
    //        {
    //            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;

    //            if (Physics.Raycast(ray.origin, ray.direction, out hit, _rewardedLayer))
    //            {
    //                if (hit.transform.TryGetComponent(out IClickable clickable))
    //                {
    //                    _rewardedButton.OpenButtonAtPosition(clickable);
    //                }
    //                else
    //                {
    //                    _rewardedButton.CloseButton();
    //                }
    //            }
    //        }
    //    }

    //}

    private IEnumerator RewardedCoroutine()
    {
        Rewarded rewarded = null;
        int rewardedCounter = 0;
        float waitTime = _rewardedDatas[rewardedCounter].time;

        while (rewardedCounter < _rewardedDatas.Count)
        {
            yield return new WaitForSeconds(waitTime);
            rewarded = GetRewarded(_rewardedDatas[rewardedCounter].rewardedType);
            rewarded.RewardedData = _rewardedDatas[rewardedCounter];
            rewarded.OpenRewarded();
            rewarded.ShowRewarded();
            rewardedCounter++;

            if(rewardedCounter < _rewardedDatas.Count)
                waitTime = _rewardedDatas[rewardedCounter].time;
        }
    }

    private Rewarded GetRewarded(RewardedType rewardedType)
    {
        Rewarded rewarded = null;

        if (rewardedType == RewardedType.Dna)
        {
            rewarded = _dnaRewareded;
        }
        else if (rewardedType == RewardedType.Offer)
        {
            rewarded = _offerRewareded;
        }
        else if(rewardedType == RewardedType.Frenzy)
        {
            rewarded = _frenzyRewareded;
        }
        return rewarded;
    }

    public void OpenAdBreak()
    {
        _adBreakGO.SetActive(true);
        _adBreakGO.transform.DOScale(_adBreakScale, 0.3f).SetEase(Ease.Linear);
    }

    public void CloseAdBreak()
    {
        _adBreakGO.SetActive(false);
        _adBreakGO.transform.localScale = Vector3.zero;
    }
}

[System.Serializable]
public class RewardedData
{
    public RewardedType rewardedType;
    public float time;
    public float rewardAmount;
    public string rewardText;
    public bool isFree;

}

public enum RewardedType
{
    Dna,
    Offer,
    Frenzy,
}