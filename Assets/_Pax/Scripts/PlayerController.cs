namespace Pax
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    GameObject cube = hit.transform.gameObject;
                    if (cube != null)
                    {
                        Vector3 targetWorldPosition = cube.transform.position;
                        //Vector3 targetCanvasPosition = GameObject.FindGameObjectWithTag("Target").GetComponent<RectTransform>().position;
                        SpawnFloatingImage(targetWorldPosition);

                    }
                }
            }
        }

        private void SpawnFloatingImage(Vector3 position)
        {
            SpawnArgs args = new SpawnArgs(position);
            MainManager.Instance.EventManager.InvokeEvent(EventTypes.CurrencyEarned, args);
        }
    }

}