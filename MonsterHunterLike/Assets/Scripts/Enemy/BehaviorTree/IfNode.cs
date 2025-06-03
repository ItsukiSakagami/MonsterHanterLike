using System;

public sealed class IfNode : Node
{
    private Func<bool> _condition;
    private Node _child;

    public IfNode(Func<bool> condition, Node child)
    {
        _condition = condition;
        _child = child;
    }

    public override NodeState Evaluate()
    {
        if (_condition())
        {
            return _child.Evaluate();
        }
        return NodeState.FAILURE;
    }
}