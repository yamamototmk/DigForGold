using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// マルチタップ対応版Input
/// </summary>
public static class MyTouchInput
{
    // Start is called before the first frame update

    /// <summary>
    /// Androidフラグ
    /// </summary>
    static readonly bool IsAndroid = Application.platform == RuntimePlatform.Android;
    /// <summary>
    /// iOSフラグ
    /// </summary>
    static readonly bool IsIOS = Application.platform == RuntimePlatform.IPhonePlayer;
    /// <summary>
    /// エディタフラグ
    /// </summary> 
    static readonly bool IsEditor = !IsAndroid && !IsIOS;


    /// <summary>
    /// デルタポジション判定用・前回のポジション
    /// </summary>
    static Vector2 prevPosition;


    /// <summary>
    /// タッチ情報を取得(エディタとスマホを考慮)
    /// </summary>
    /// <returns>タッチ情報</returns>
    public static MyTouchPahse GetPhase(int fingerId)
    {
        if (IsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                prevPosition = Input.mousePosition;
                return MyTouchPahse.Begin;
            }
            else if (Input.GetMouseButton(0))
            {
                return MyTouchPahse.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return MyTouchPahse.Ended;
            }
        }
        else
        {
            //return (MyTouchPahse)(int)Input.GetTouch(fingerId).phase;
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).fingerId == fingerId) return (MyTouchPahse)Input.GetTouch(i).phase;
            }
        }
        return MyTouchPahse.None;
    }

    public static Vector2 GetPosition(int fingerId)
    {
        if (IsEditor)
        {
            if (GetPhase(fingerId) != MyTouchPahse.None) return Input.mousePosition;
        }
        else
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (GetPhase(fingerId) == MyTouchPahse.None) break;
                if (Input.GetTouch(i).fingerId == fingerId) return Input.GetTouch(i).position;
            }

        }
        return Vector2.zero;
    }
    /// <summary>
    /// 一番新しいTouchのfingerIDを返す
    /// </summary>
    /// <returns></returns>
    public static int GetFingerId()
    {
        if (IsEditor)
        {
            if (!Input.GetMouseButton(0)) return -1;

            return 0;
        }

        if (Input.touchCount == 0) return -1;
        return Input.GetTouch(Input.touchCount - 1).fingerId;
    }
    /// <summary>
    /// fingerIdの更新。特定のfingerIdを追いたいときに使う、指定したIDが無ければ一番新しいTouchを返す
    /// </summary>
    /// <param name="fingerVariable">fingerIdを記憶させてる変数</param>
    public static void FingerIdUpdateAuto(ref int fingerVariable)
    {
        if (fingerVariable != -1)//既に追跡中なら追跡中のfingerのphaseを確認。touchが終了していれば-1
        {
            MyTouchPahse currentPhase = GetPhase(fingerVariable);
            if (currentPhase == MyTouchPahse.None || currentPhase == MyTouchPahse.Canceled || currentPhase == MyTouchPahse.Ended)
            {
                fingerVariable = -1;
            }
        }
        else//空ならfingerIdを設定
        {
            fingerVariable = GetFingerId();
        }
    }
    /// <summary>
    /// fingerIdの更新。特定のfingerIdを追いたいときに使う
    /// </summary>
    /// <param name="fingerVariable">fingerIdを記憶させてる変数</param>
    public static void FingerIdUpdate(ref int fingerVariable)
    {
        if (fingerVariable != -1)//既に追跡中なら追跡中のfingerのphaseを確認。touchが終了していれば-1
        {
            MyTouchPahse currentPhase = GetPhase(fingerVariable);
            if (currentPhase == MyTouchPahse.None || currentPhase == MyTouchPahse.Canceled || currentPhase == MyTouchPahse.Ended)
            {
                fingerVariable = -1;
            }
        }
        else//空ならfingerIdを設定
        {
            fingerVariable = -1;
        }
    }
}

/// <summary>
/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
/// </summary>
public enum MyTouchPahse
{
    None = -1,
    Begin = 0,//タッチされた瞬間
    Moved = 1,//移動中
    Stationary = 2,//静止
    Ended = 3,//終了
    Canceled = 4//追跡中断
}
