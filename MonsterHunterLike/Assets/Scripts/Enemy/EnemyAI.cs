//敵本体
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyAI : MonoBehaviour
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
    //攻撃のノード(自身で実装)
    private BehaviorAction.Action _attackNode;
    private BehaviorAction.Action _testNode;

    private Node _ifNode;
    private Node ifElseNode;
    #endregion

    [SerializeField]
    private float _moveSpeed;

    private void Start()
    {
        //ノードを生成
        CreateNode();

        //シーケンスの登録
        Sequence sequence = new Sequence(new List<Node> { _findNode, _moveNode, _waitNode, _attackNode, _ifNode, ifElseNode });

        //ルートノードにsequenceを設定
        _root = sequence;
    }

    private void Update()
    {
            //ビヘイビアツリーを実行
            _root?.Evaluate();
    }

    private void CreateNode()
    {
        //一秒待つノード
        _waitNode = new WaitNode(3.0f);

        //引数(自身,targetのtag,当たり判定の長さ)
        _findNode = new FindNode(transform, "Player", 10.0f);

        //移動ノードの実装
        _moveNode = new BehaviorAction.Action(() =>
        {
            //ターゲットを検索
            if (_findNode.Target != null)
            {
                Debug.Log("移動");

                //追尾の処理
                transform.position = Vector2.MoveTowards(transform.position, _findNode.Target.position, _moveSpeed * Time.deltaTime);

                //ターゲットから特定の距離まで近づいたら処理
                if (Vector2.Distance(transform.position, _findNode.Target.position) < 1.0f)
                {
                    return Node.NodeState.SUCCESS;
                }
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
            null              // 条件が false のとき
        );
    }
}
