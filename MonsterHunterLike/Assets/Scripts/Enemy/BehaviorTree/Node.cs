public abstract class Node
{
    public enum NodeState : uint
    {
        SUCCESS,    //¬Œ÷
        FAILURE,    //¸”s
        RUNNING     //Às
    }

    /// <summary>
    /// ƒm[ƒh‚ğ•]‰¿‚·‚é
    /// </summary>
    public abstract NodeState Evaluate();
}
