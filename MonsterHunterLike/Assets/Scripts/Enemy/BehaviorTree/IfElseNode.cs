using System;

public sealed class IfElseNode : Node
{
    //ğŒ
    private Func<bool> _condition;
    //true‚É‚â‚ç‚¹‚½‚¢ˆ—
    private Node _trueNode;
    //false‚É‚â‚ç‚¹‚½‚¢ˆ—
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
