using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //大元のノード
    private Node _root;

    #region Nodes
    //一定時間待つノード
    private WaitNode _waitNode;
    //タグによってターゲットを取得するノード
    private FindNode _findNode;
    //移動のノード(自身で実装)
    private BehaviorAction.Action _moveNode;
    private BehaviorAction.Action _move2Node;
    //攻撃のノード(自身で実装)
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
        //ノードを生成
        CreateNode();

        //シーケンスの登録
        Sequence sequence = new Sequence(new List<Node> { _findNode, _rotateNode, _waitNode, _move2Node, _waitNode, _attackNode, _ifNode, ifElseNode });

        //ルートノードにsequenceを設定
        _root = sequence;

        _initrunTime = _runTime;
    }

    void Update()
    {
        //ビヘイビアツリーを実行
        _root?.Evaluate();
    }

    private void CreateNode()
    {
        //一秒待つノード
        _waitNode = new WaitNode(1.0f);

        //引数(自身,targetのtag,当たり判定の長さ)
        _findNode = new FindNode(transform, "Player", 10.0f);

        //移動ノードの実装
        _moveNode = new BehaviorAction.Action(() =>
        {
            //ターゲットを検索
            if (_findNode.Target != null)
            {
                Debug.Log("移動");

                //常に追尾する処理
                transform.position = Vector3.MoveTowards(transform.position, _findNode.Target.position, _moveSpeed * Time.deltaTime);

                //ターゲットから特定の距離まで近づいたら処理
                if (Vector3.Distance(transform.position, _findNode.Target.position) < 1.0f)
                {
                    return Node.NodeState.SUCCESS;
                }
            }
            return Node.NodeState.RUNNING;
        });

        //攻撃ノードの実装
        _move2Node = new BehaviorAction.Action(() =>
        {
            // forward方向に進む
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

        //攻撃ノードの実装
        _attackNode = new BehaviorAction.Action(() =>
        {
            Debug.Log("攻撃");
            return Node.NodeState.SUCCESS;
        });

        _testNode = new BehaviorAction.Action(() =>
        {
            Debug.Log("test");
            return Node.NodeState.SUCCESS;
        });

        //条件によってNodeを処理
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
            _testNode,              // 条件が true のとき
            null              // 条件が fal
                              // se のとき
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

            // Playerの位置を取得して敵のy座標はそのままにする（水平回転のみ）
            Vector3 direction = _playerPos.position - transform.position;
            direction.y = 0;

            if (direction == Vector3.zero)
            {
                return Node.NodeState.SUCCESS;
            }

            // プレイヤー方向の回転Quaternionを計算
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 現在の前方向とターゲット方向の角度を計算
            float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

            // ある閾値以下なら成功と判定（例：5度以下）
            if (angleDiff < 10f)
            {
                transform.rotation = targetRotation;  // 完全に合わせる
                return Node.NodeState.SUCCESS;
            }

            // ゆっくり回転させる
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            // 回転中ならRUNNINGを返す
            return Node.NodeState.RUNNING;
        });
    }
}
