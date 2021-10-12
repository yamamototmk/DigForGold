using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �}���`�^�b�v�Ή���Input
/// </summary>
public static class MyTouchInput
{
    // Start is called before the first frame update

    /// <summary>
    /// Android�t���O
    /// </summary>
    static readonly bool IsAndroid = Application.platform == RuntimePlatform.Android;
    /// <summary>
    /// iOS�t���O
    /// </summary>
    static readonly bool IsIOS = Application.platform == RuntimePlatform.IPhonePlayer;
    /// <summary>
    /// �G�f�B�^�t���O
    /// </summary> 
    static readonly bool IsEditor = !IsAndroid && !IsIOS;


    /// <summary>
    /// �f���^�|�W�V��������p�E�O��̃|�W�V����
    /// </summary>
    static Vector2 prevPosition;


    /// <summary>
    /// �^�b�`�����擾(�G�f�B�^�ƃX�}�z���l��)
    /// </summary>
    /// <returns>�^�b�`���</returns>
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
    /// ��ԐV����Touch��fingerID��Ԃ�
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
    /// fingerId�̍X�V�B�����fingerId��ǂ������Ƃ��Ɏg���A�w�肵��ID��������Έ�ԐV����Touch��Ԃ�
    /// </summary>
    /// <param name="fingerVariable">fingerId���L�������Ă�ϐ�</param>
    public static void FingerIdUpdateAuto(ref int fingerVariable)
    {
        if (fingerVariable != -1)//���ɒǐՒ��Ȃ�ǐՒ���finger��phase���m�F�Btouch���I�����Ă����-1
        {
            MyTouchPahse currentPhase = GetPhase(fingerVariable);
            if (currentPhase == MyTouchPahse.None || currentPhase == MyTouchPahse.Canceled || currentPhase == MyTouchPahse.Ended)
            {
                fingerVariable = -1;
            }
        }
        else//��Ȃ�fingerId��ݒ�
        {
            fingerVariable = GetFingerId();
        }
    }
    /// <summary>
    /// fingerId�̍X�V�B�����fingerId��ǂ������Ƃ��Ɏg��
    /// </summary>
    /// <param name="fingerVariable">fingerId���L�������Ă�ϐ�</param>
    public static void FingerIdUpdate(ref int fingerVariable)
    {
        if (fingerVariable != -1)//���ɒǐՒ��Ȃ�ǐՒ���finger��phase���m�F�Btouch���I�����Ă����-1
        {
            MyTouchPahse currentPhase = GetPhase(fingerVariable);
            if (currentPhase == MyTouchPahse.None || currentPhase == MyTouchPahse.Canceled || currentPhase == MyTouchPahse.Ended)
            {
                fingerVariable = -1;
            }
        }
        else//��Ȃ�fingerId��ݒ�
        {
            fingerVariable = -1;
        }
    }
}

/// <summary>
/// �^�b�`���BUnityEngine.TouchPhase �� None �̏���ǉ��g���B
/// </summary>
public enum MyTouchPahse
{
    None = -1,
    Begin = 0,//�^�b�`���ꂽ�u��
    Moved = 1,//�ړ���
    Stationary = 2,//�Î~
    Ended = 3,//�I��
    Canceled = 4//�ǐՒ��f
}
