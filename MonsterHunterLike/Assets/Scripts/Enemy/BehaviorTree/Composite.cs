using System.Collections.Generic;

public abstract class Composite : Node
{
    //Composite系のノードは複数の子を持ち、それらを制御する
    //子ノードを格納するリスト
    protected List<Node> _children = new List<Node>();

    public Composite(List<Node> children)
    {
        _children = children;
    }
}
