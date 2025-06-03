using System.Collections.Generic;

public sealed class Selector : Composite
{
    public Selector(List<Node> children) : base(children) { }

    /// <summary>
    /// Ç«ÇÍÇ©Ç™ê¨å˜ÇµÇΩÇÁé¿çsÇ∑ÇÈ
    /// </summary>
    public override NodeState Evaluate()
    {
        foreach (var child in _children)
        {
            NodeState state = child.Evaluate();

            if (state == NodeState.SUCCESS)
            {
                return NodeState.SUCCESS;
            }
            else if (state == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
        }
        return NodeState.FAILURE;
    }
}
