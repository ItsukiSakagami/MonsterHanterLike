using UnityEngine;

public sealed class FindNode : Node
{
    //AI�̖{��
    private Transform _agent;
    //�T���^�[�Q�b�g�̃^�O
    private string _targetTag;
    //�T���͈�
    private float _searchRadius;
    //���������^�[�Q�b�g
    public Transform Target { get; private set; } 

    public FindNode(Transform agent, string targetTag, float searchRadius)
    {
        //������
        _agent = agent;
        _targetTag = targetTag;
        _searchRadius = searchRadius;
    }

    public override NodeState Evaluate()
    {
        //�����������̂��擾
        //Collider2D[] hits = Physics2D.OverlapCircleAll(_agent.position, _searchRadius);
        Collider[] hits = Physics.OverlapSphere(_agent.position, _searchRadius);

        //�����������̂�S�ĉ�
        foreach (var hit in hits)
        {
            //�^�[�Q�b�g�̃^�O������Ώ���
            if (hit.CompareTag(_targetTag))
            {
                //�^�[�Q�b�g���L��
                Target = hit.transform;

                //������
                return NodeState.SUCCESS; 
            }
        }
        Target = null;
        //������Ȃ�����
        return NodeState.FAILURE; 
    }

}
