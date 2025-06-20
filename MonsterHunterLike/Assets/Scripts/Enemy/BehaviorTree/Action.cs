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

        //実装は各々に任せる
        public override NodeState Evaluate()
        {
            return _action();
        }
    }
}