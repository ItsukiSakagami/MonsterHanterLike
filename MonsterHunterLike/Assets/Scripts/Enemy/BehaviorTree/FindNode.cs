using UnityEngine;

public sealed class FindNode : Node
{
    //AIの本体
    private Transform _agent;
    //探すターゲットのタグ
    private string _targetTag;
    //探索範囲
    private float _searchRadius;
    //見つかったターゲット
    public Transform Target { get; private set; } 

    public FindNode(Transform agent, string targetTag, float searchRadius)
    {
        //初期化
        _agent = agent;
        _targetTag = targetTag;
        _searchRadius = searchRadius;
    }

    public override NodeState Evaluate()
    {
        //当たったものを取得
        //Collider2D[] hits = Physics2D.OverlapCircleAll(_agent.position, _searchRadius);
        Collider[] hits = Physics.OverlapSphere(_agent.position, _searchRadius);

        //当たったものを全て回す
        foreach (var hit in hits)
        {
            //ターゲットのタグがあれば処理
            if (hit.CompareTag(_targetTag))
            {
                //ターゲットを記憶
                Target = hit.transform;

                //見つけた
                return NodeState.SUCCESS; 
            }
        }
        Target = null;
        //見つからなかった
        return NodeState.FAILURE; 
    }

}
