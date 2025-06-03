using System.Collections.Generic;

public abstract class Composite : Node
{
    //Composite�n�̃m�[�h�͕����̎q�������A�����𐧌䂷��
    //�q�m�[�h���i�[���郊�X�g
    protected List<Node> _children = new List<Node>();

    public Composite(List<Node> children)
    {
        _children = children;
    }
}
