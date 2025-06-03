//�G�{��
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyAI : MonoBehaviour
{
    //�匳�̃m�[�h
    private Node _root;

    #region Nodes
    //��莞�ԑ҂m�[�h
    private WaitNode _waitNode;
    //�^�O�ɂ���ă^�[�Q�b�g���擾����m�[�h
    private FindNode _findNode;
    //�ړ��̃m�[�h(���g�Ŏ���)
    private BehaviorAction.Action _moveNode;
    //�U���̃m�[�h(���g�Ŏ���)
    private BehaviorAction.Action _attackNode;
    private BehaviorAction.Action _testNode;

    private Node _ifNode;
    private Node ifElseNode;
    #endregion

    [SerializeField]
    private float _moveSpeed;

    private void Start()
    {
        //�m�[�h�𐶐�
        CreateNode();

        //�V�[�P���X�̓o�^
        Sequence sequence = new Sequence(new List<Node> { _findNode, _moveNode, _waitNode, _attackNode, _ifNode, ifElseNode });

        //���[�g�m�[�h��sequence��ݒ�
        _root = sequence;
    }

    private void Update()
    {
            //�r�w�C�r�A�c���[�����s
            _root?.Evaluate();
    }

    private void CreateNode()
    {
        //��b�҂m�[�h
        _waitNode = new WaitNode(3.0f);

        //����(���g,target��tag,�����蔻��̒���)
        _findNode = new FindNode(transform, "Player", 10.0f);

        //�ړ��m�[�h�̎���
        _moveNode = new BehaviorAction.Action(() =>
        {
            //�^�[�Q�b�g������
            if (_findNode.Target != null)
            {
                Debug.Log("�ړ�");

                //�ǔ��̏���
                transform.position = Vector2.MoveTowards(transform.position, _findNode.Target.position, _moveSpeed * Time.deltaTime);

                //�^�[�Q�b�g�������̋����܂ŋ߂Â����珈��
                if (Vector2.Distance(transform.position, _findNode.Target.position) < 1.0f)
                {
                    return Node.NodeState.SUCCESS;
                }
            }
            return Node.NodeState.RUNNING;
        });

        //�U���m�[�h�̎���
        _attackNode = new BehaviorAction.Action(() =>
        {
            Debug.Log("�U��");
            return Node.NodeState.SUCCESS;
        });

        _testNode = new BehaviorAction.Action(() =>
        {
            Debug.Log("test");
            return Node.NodeState.SUCCESS;
        });

        //�����ɂ����Node������
        _ifNode = new IfNode(
            () => _moveSpeed < 5.0f,
            _testNode
        );

        var testNode = new BehaviorAction.Action(() =>
        {
            Debug.Log("false");
            return Node.NodeState.SUCCESS;
        });

        ifElseNode = new IfElseNode(
            () => false,
            _testNode,              // ������ true �̂Ƃ�
            null              // ������ false �̂Ƃ�
        );
    }
}
