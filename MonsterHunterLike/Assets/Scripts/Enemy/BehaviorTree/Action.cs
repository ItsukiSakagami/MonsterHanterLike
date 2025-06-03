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

        //�����͊e�X�ɔC����
        public override NodeState Evaluate()
        {
            return _action();
        }
    }
}