using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class My_VirtualPad : MonoBehaviour
{
    const int MAX_DISTANCE = 100;
    RectTransform rectTransform;
    Vector2 beganPos;
    Vector2 diffPos;
    Vector2 normalized;

    [SerializeField, Header("指ID")] int fingerId = -1;
    [SerializeField, Header("入力感度")] float intensity;
    [SerializeField, Header("移動量")] float moveAmount;
    [SerializeField, Header("正規化された入力値")] Vector2 direction;
    [SerializeField, Header("入力値として使用する値")] Vector2 input;
    [SerializeField, Header("タッチされた位置")] Vector2 beginTouchPos;

    //UI
    [SerializeField, Header("現在のカーソル位置UI")] Image cursorImg;
    [SerializeField, Header("タッチBeginの位置UI")] Image beginCursorImg;

    //デバッグ用
    [SerializeField] TextMeshProUGUI tmp;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void Update()
    {
        InputUpdate();
        DrawUpdate();
    }
    /// <summary>
    /// 入力の更新
    /// </summary>
    private void InputUpdate()
    {
        MyTouchInput.FingerIdUpdate(ref fingerId);

        //tmp.text = "rectT:" + rectTransform.position + "\ntouchPos:" + MyTouchInput.GetPosition(fingerId) + "\nfingerID" + fingerId;

        if (fingerId == -1)
        {
            input = Vector2.zero;
            direction = Vector2.zero;
            moveAmount = 0;
            return;//操作中でないなら終了
        }

        MyTouchPahse currentPhase = MyTouchInput.GetPhase(fingerId);//タッチの状態
        if (currentPhase == MyTouchPahse.Begin)
        {
            beganPos = MyTouchInput.GetPosition(fingerId);
        }

        Vector2 currentTouchPos = MyTouchInput.GetPosition(fingerId);//現在のタッチ座標

        diffPos = currentTouchPos - beganPos;

        normalized = diffPos.normalized;

        float distance = Vector2.Distance(currentTouchPos, beganPos);//移動量
        moveAmount = distance / MAX_DISTANCE * intensity;
        moveAmount = moveAmount <= 1 ? moveAmount : 1;//移動量が最大以下なら移動量を、最大以上なら最大値を代入

        direction = normalized;
        input = normalized * moveAmount;
    }
    /// <summary>
    /// 描画の更新
    /// </summary>
    private void DrawUpdate()
    {
        if (fingerId == -1)
        {
            beginCursorImg.gameObject.SetActive(false);
            cursorImg.gameObject.SetActive(false);
            return;
        }
        beginCursorImg.gameObject.SetActive(true); ;
        cursorImg.gameObject.SetActive(true);

        beginCursorImg.rectTransform.position = beginTouchPos;
        cursorImg.rectTransform.position = beginTouchPos + input * MAX_DISTANCE;
    }
    /// <summary>
    /// パッドの受付範囲がタッチされた時に呼ぶイベント
    /// 最もパッドに近い位置をタッチしている指をfingerIDに設定する。
    /// </summary>
    public void OnTouch()
    {
        //最も近いタッチ
        Touch minTouch = new Touch() { position = new Vector2(Mathf.Infinity, Mathf.Infinity) };
        foreach (Touch touch in Input.touches)
        {
            float minDist = Vector2.Distance(minTouch.position, rectTransform.position);
            float distance = Vector2.Distance(touch.position, rectTransform.position);

            if (distance < minDist)
            {

                minTouch = touch;
                beginTouchPos = minTouch.position;
            }
        }
        if (Application.platform == RuntimePlatform.WindowsEditor) beginTouchPos = Input.mousePosition;

        fingerId = minTouch.fingerId;
    }
    /// <summary>
    /// 正規化された入力ベクトル
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDirection()
    {
        return direction;
    }
    /// <summary>
    /// 入力ベクトル
    /// </summary>
    /// <returns></returns>
    public Vector2 GetInput()
    {
        return input;
    }
    /// <summary>
    /// 移動量
    /// </summary>
    /// <returns></returns>
    public float GetMoveAmount()
    {
        return moveAmount;
    }
}
