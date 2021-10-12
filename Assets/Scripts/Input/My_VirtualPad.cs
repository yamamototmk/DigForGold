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

    [SerializeField, Header("�wID")] int fingerId = -1;
    [SerializeField, Header("���͊��x")] float intensity;
    [SerializeField, Header("�ړ���")] float moveAmount;
    [SerializeField, Header("���K�����ꂽ���͒l")] Vector2 direction;
    [SerializeField, Header("���͒l�Ƃ��Ďg�p����l")] Vector2 input;
    [SerializeField, Header("�^�b�`���ꂽ�ʒu")] Vector2 beginTouchPos;

    //UI
    [SerializeField, Header("���݂̃J�[�\���ʒuUI")] Image cursorImg;
    [SerializeField, Header("�^�b�`Begin�̈ʒuUI")] Image beginCursorImg;

    //�f�o�b�O�p
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
    /// ���͂̍X�V
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
            return;//���쒆�łȂ��Ȃ�I��
        }

        MyTouchPahse currentPhase = MyTouchInput.GetPhase(fingerId);//�^�b�`�̏��
        if (currentPhase == MyTouchPahse.Begin)
        {
            beganPos = MyTouchInput.GetPosition(fingerId);
        }

        Vector2 currentTouchPos = MyTouchInput.GetPosition(fingerId);//���݂̃^�b�`���W

        diffPos = currentTouchPos - beganPos;

        normalized = diffPos.normalized;

        float distance = Vector2.Distance(currentTouchPos, beganPos);//�ړ���
        moveAmount = distance / MAX_DISTANCE * intensity;
        moveAmount = moveAmount <= 1 ? moveAmount : 1;//�ړ��ʂ��ő�ȉ��Ȃ�ړ��ʂ��A�ő�ȏ�Ȃ�ő�l����

        direction = normalized;
        input = normalized * moveAmount;
    }
    /// <summary>
    /// �`��̍X�V
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
    /// �p�b�h�̎�t�͈͂��^�b�`���ꂽ���ɌĂԃC�x���g
    /// �ł��p�b�h�ɋ߂��ʒu���^�b�`���Ă���w��fingerID�ɐݒ肷��B
    /// </summary>
    public void OnTouch()
    {
        //�ł��߂��^�b�`
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
    /// ���K�����ꂽ���̓x�N�g��
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDirection()
    {
        return direction;
    }
    /// <summary>
    /// ���̓x�N�g��
    /// </summary>
    /// <returns></returns>
    public Vector2 GetInput()
    {
        return input;
    }
    /// <summary>
    /// �ړ���
    /// </summary>
    /// <returns></returns>
    public float GetMoveAmount()
    {
        return moveAmount;
    }
}
