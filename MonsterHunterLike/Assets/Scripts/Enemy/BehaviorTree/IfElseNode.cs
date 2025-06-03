using System;

public sealed class IfElseNode : Node
{
    //����
    private Func<bool> _condition;
    //true���ɂ�点��������
    private Node _trueNode;
    //false���ɂ�点��������
    private Node _falseNode;

    public IfElseNode(Func<bool> condition, Node trueNode, Node falseNode)
    {
        _condition = condition;
        _trueNode = trueNode;
        _falseNode = falseNode;
    }

    public override NodeState Evaluate()
    {
        if (_condition())
        {
            if (_trueNode == null)
            {
                return NodeState.SUCCESS;
            }
            return _trueNode.Evaluate();
        }
        else
        {
            if (_falseNode == null)
            {
                return NodeState.SUCCESS;
            }
            return _falseNode.Evaluate();
        }
    }
}
