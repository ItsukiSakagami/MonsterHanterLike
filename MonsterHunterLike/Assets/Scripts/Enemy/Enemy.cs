using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
    private BehaviorAction.Action _move2Node;
    //�U���̃m�[�h(���g�Ŏ���)
    private BehaviorAction.Action _attackNode;
    private BehaviorAction.Action _testNode;
    private BehaviorAction.Action _rotateNode;

    private Node _ifNode;
    private Node ifElseNode;
    #endregion

    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotationSpeed;
    private Transform _playerPos = null;
    [SerializeField]
    private float _runTime = 1.0f;
    private float _initrunTime;

    void Start()
    {
        //�m�[�h�𐶐�
        CreateNode();

        //�V�[�P���X�̓o�^
        Sequence sequence = new Sequence(new List<Node> { _findNode, _rotateNode, _waitNode, _move2Node, _waitNode, _attackNode, _ifNode, ifElseNode });

        //���[�g�m�[�h��sequence��ݒ�
        _root = sequence;

        _initrunTime = _runTime;
    }

    void Update()
    {
        //�r�w�C�r�A�c���[�����s
        _root?.Evaluate();
    }

    private void CreateNode()
    {
        //��b�҂m�[�h
        _waitNode = new WaitNode(1.0f);

        //����(���g,target��tag,�����蔻��̒���)
        _findNode = new FindNode(transform, "Player", 10.0f);

        //�ړ��m�[�h�̎���
        _moveNode = new BehaviorAction.Action(() =>
        {
            //�^�[�Q�b�g������
            if (_findNode.Target != null)
            {
                Debug.Log("�ړ�");

                //��ɒǔ����鏈��
                transform.position = Vector3.MoveTowards(transform.position, _findNode.Target.position, _moveSpeed * Time.deltaTime);

                //�^�[�Q�b�g�������̋����܂ŋ߂Â����珈��
                if (Vector3.Distance(transform.position, _findNode.Target.position) < 1.0f)
                {
                    return Node.NodeState.SUCCESS;
                }
            }
            return Node.NodeState.RUNNING;
        });

        //�U���m�[�h�̎���
        _move2Node = new BehaviorAction.Action(() =>
        {
            // forward�����ɐi��
            Vector3 moveDir = transform.forward * _moveSpeed * Time.deltaTime;
            transform.position += moveDir;

            _runTime -= Time.deltaTime;
            if (_runTime <= 0.0f)
            {
                _runTime = _initrunTime;
                return Node.NodeState.SUCCESS;
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
            null              // ������ fal
                              // se �̂Ƃ�
        );

        _rotateNode = new BehaviorAction.Action(() =>
        {

            if (_findNode != null && _playerPos == null)
            {
                _playerPos = _findNode.Target;
            }

            if (_playerPos == null)
            {
                return Node.NodeState.FAILURE;
            }

            // Player�̈ʒu���擾���ēG��y���W�͂��̂܂܂ɂ���i������]�̂݁j
            Vector3 direction = _playerPos.position - transform.position;
            direction.y = 0;

            if (direction == Vector3.zero)
            {
                return Node.NodeState.SUCCESS;
            }

            // �v���C���[�����̉�]Quaternion���v�Z
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ���݂̑O�����ƃ^�[�Q�b�g�����̊p�x���v�Z
            float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

            // ����臒l�ȉ��Ȃ琬���Ɣ���i��F5�x�ȉ��j
            if (angleDiff < 10f)
            {
                transform.rotation = targetRotation;  // ���S�ɍ��킹��
                return Node.NodeState.SUCCESS;
            }

            // ��������]������
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            // ��]���Ȃ�RUNNING��Ԃ�
            return Node.NodeState.RUNNING;
        });
    }
}
