using System;

namespace BehaviorAction
{
    public sealed class Action : Node
    {
        private Func<NodeState> _action;

        public Action(Func<NodeState> action)
        {
            _action = action;
        }

        //é¿ëïÇÕäeÅXÇ…îCÇπÇÈ
        public override NodeState Evaluate()
        {
            return _action();
        }
    }
}