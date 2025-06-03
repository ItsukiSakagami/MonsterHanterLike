using System.Collections.Generic;

public sealed class Sequence : Composite
{
    //自身が持つ子のノード
    private List<Node> _nodes;

    //現在のノードのインデックス
    private int currentNodeIndex = 0;

    public Sequence(List<Node> nodes) : base(nodes)
    {
        _nodes = nodes;
    }

    /// <summary>
    /// 子を順番に評価する
    /// </summary>
    public override NodeState Evaluate()
    {
        while (currentNodeIndex < _nodes?.Count)
        {
            NodeState state = _nodes[currentNodeIndex].Evaluate();

            if (state == NodeState.RUNNING)
            {
                //現在のノードが実行中なら、次に進まない
                return NodeState.RUNNING;  
            }
            else if (state == NodeState.FAILURE)
            {
                //失敗したら最初から
                currentNodeIndex = 0;  
                return NodeState.FAILURE;
            }
            //成功したら次のノードへ
            currentNodeIndex++;  
        }
        //すべて成功したらリセット
        currentNodeIndex = 0;  
        return NodeState.SUCCESS;
    }
}
