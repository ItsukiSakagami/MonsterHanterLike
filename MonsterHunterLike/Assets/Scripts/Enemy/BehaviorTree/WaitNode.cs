using UnityEngine;

public sealed class WaitNode : Node
{
    //待機時間
    private float _waitTime;
    //開始時間
    private float _startTime;

    //待機中かどうか
    private bool _isWaiting;            

    public WaitNode(float seconds)
    {
        //初期化
        _waitTime = seconds;
        _isWaiting = true;
        _startTime = 0.0f;
    }

    public override NodeState Evaluate()
    {
        //待機中だったら時間を加算
        if (_isWaiting)
        {
            _startTime += Time.deltaTime;
        }
        //二回目以降を考慮
        else
        {
            _startTime = 0.0f;
            _isWaiting = true;
        }

        //指定した時間経過したら処理
        if (_startTime >= _waitTime)
        {
            Debug.Log(_waitTime + "経過");
            _isWaiting = false;
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;
    }


}
