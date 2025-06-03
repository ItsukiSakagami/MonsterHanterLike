using UnityEngine;

public sealed class WaitNode : Node
{
    //�ҋ@����
    private float _waitTime;
    //�J�n����
    private float _startTime;

    //�ҋ@�����ǂ���
    private bool _isWaiting;            

    public WaitNode(float seconds)
    {
        //������
        _waitTime = seconds;
        _isWaiting = true;
        _startTime = 0.0f;
    }

    public override NodeState Evaluate()
    {
        //�ҋ@���������玞�Ԃ����Z
        if (_isWaiting)
        {
            _startTime += Time.deltaTime;
        }
        //���ڈȍ~���l��
        else
        {
            _startTime = 0.0f;
            _isWaiting = true;
        }

        //�w�肵�����Ԍo�߂����珈��
        if (_startTime >= _waitTime)
        {
            Debug.Log(_waitTime + "�o��");
            _isWaiting = false;
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }


}
