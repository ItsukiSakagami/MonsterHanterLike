public abstract class Node
{
    public enum NodeState : uint
    {
        SUCCESS,    //����
        FAILURE,    //���s
        RUNNING     //���s
    }

    /// <summary>
    /// �m�[�h��]������
    /// </summary>
    public abstract NodeState Evaluate();
}
