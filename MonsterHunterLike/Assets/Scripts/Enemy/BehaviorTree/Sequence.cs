using System.Collections.Generic;

public sealed class Sequence : Composite
{
    //���g�����q�̃m�[�h
    private List<Node> _nodes;

    //���݂̃m�[�h�̃C���f�b�N�X
    private int currentNodeIndex = 0;

    public Sequence(List<Node> nodes) : base(nodes)
    {
        _nodes = nodes;
    }

    /// <summary>
    /// �q�����Ԃɕ]������
    /// </summary>
    public override NodeState Evaluate()
    {
        while (currentNodeIndex < _nodes?.Count)
        {
            NodeState state = _nodes[currentNodeIndex].Evaluate();

            if (state == NodeState.RUNNING)
            {
                //���݂̃m�[�h�����s���Ȃ�A���ɐi�܂Ȃ�
                return NodeState.RUNNING;  
            }
            else if (state == NodeState.FAILURE)
            {
                //���s������ŏ�����
                currentNodeIndex = 0;  
                return NodeState.FAILURE;
            }
            //���������玟�̃m�[�h��
            currentNodeIndex++;  
        }
        //���ׂĐ��������烊�Z�b�g
        currentNodeIndex = 0;  
        return NodeState.SUCCESS;
    }
}
