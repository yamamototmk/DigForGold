using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] RectTransform rectT;
    [SerializeField] Vector3 offset;
    void Awake()
    {
        bar.TryGetComponent(out rectT);
    }
    public void SetCurrentHp(float max, float current)
    {
        bar.fillAmount = current / max;
    }

    public void HpBarUpdate(float maxHp, float currentHp, Vector3 worldPos)
    {
        SetCurrentHp(maxHp, currentHp);
        rectT.position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos+offset);
    }
}
