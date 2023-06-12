using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.PathSystem;
using FD.ProductSystem;
using FD.HealthSystem;

public class Soldier : MonoBehaviour
{
    [SerializeField]
    private int _initialHealthValue = 3;
    [SerializeField]
    private int _damage = 3;
    [SerializeField]
    private float _moveSpeed = 3;
    [SerializeField]
    public List<DefaultSoldier> productPattern;
    [SerializeField]
    private BoxCollider2D _boxCollider;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public enum Type { player, enemy }
    public Type type;
    [SerializeField]
    private Health health;
    private Animator _myAnimator;
    public enum AnimType { idle,run,hit,attack}
    public GameObject highLight;
    public List<PathNode> myBoundsPathNodes;
#if UNITY_EDITOR
    private void OnValidate()
    {
      //  GetPatternData(0);
    }
#endif
    private void Awake()
    {
        health = GetComponent<Health>();
        _myAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        myBoundsPathNodes = PathManager.scr.GetBoundsOfObject(transform.position, productPattern[0].CellSize);
        GetAnimClipTimes();
        if (_isDead)
            _isDead = false;
    }

    public void GetPatternData(int index)
    {
        _spriteRenderer.sprite = productPattern[index].ProductSprite;
        _spriteRenderer.size = productPattern[index].CellSize * 0.32f;
        health.InitializeHealth(productPattern[index].health);
        _damage = productPattern[index].damage;
        _myAnimator.runtimeAnimatorController = productPattern[index].animation_controller;
        _moveSpeed = productPattern[index].movementSpeed;
    }


    [SerializeField]
    private List<PathNode> _movementPath;
    public PathNode myCurrentNode,TargetNode;
    public void GetPath()
    {
        StopMovement();
        PathNode targetNode = PathManager.scr.SelectPathNode();
        TargetNode = targetNode;
        _movementPath = PathManager.scr.pathfinding.FindPath(myCurrentNode, targetNode);
        if (_movementPath != null)
        {
            StartCoroutine(Movement(2f));
            SetDirection(myCurrentNode, targetNode);

        }
    }
        PathNode selectNode;
    public void FindNearestBound(List<PathNode> nodeList)
    {
        float shortestDistance = Mathf.Infinity;
        for (int i = 0; i < nodeList.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, nodeList[i].transform.position);
            if(distance<shortestDistance && nodeList[i].walkable)
            {
                shortestDistance = distance;
                selectNode = nodeList[i];
            }
        }
        if (selectNode!=null)
        {
            StopMovement();
            TargetNode = selectNode;
            _movementPath = PathManager.scr.pathfinding.FindPath(myCurrentNode, TargetNode);
            if (_movementPath.Count <= 0)
                return;
            StartCoroutine(Movement(2f));
            SetDirection(myCurrentNode, TargetNode);
        }
    }
    public void Sethighlight(bool active)
    {
        if (!highLight)
            return;
        highLight.SetActive(active);
    }
    private int pathIndex;
    public bool _isMove;
    IEnumerator Movement(float moveSpeed)
    {
        _isMove = true;
        TargetNode.walkable = false;
        SetAnimation(AnimType.run);
        while (transform.position != _movementPath[_movementPath.Count-1].transform.position)
        {
            myCurrentNode.walkable = true;
            transform.position = Vector2.MoveTowards(transform.position, _movementPath[pathIndex].transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == _movementPath[pathIndex].transform.position && pathIndex != _movementPath.Count - 1)
            {
                myCurrentNode = _movementPath[pathIndex];
                myCurrentNode.walkable = false;
                pathIndex++;
            }
            yield return null;
            

        }
        
        myCurrentNode = _movementPath[_movementPath.Count - 1];
        myCurrentNode.walkable = false;
        _isMove = false;
        SetAnimation(AnimType.idle);
        myBoundsPathNodes = PathManager.scr.GetBoundsOfObject(transform.position, productPattern[0].CellSize);
    }
    private void StopMovement()
    {
        myCurrentNode = PathManager.scr.GetPathNode(transform.position);
        myCurrentNode.walkable = false;
        _isMove = false;
        if (myCurrentNode != TargetNode && TargetNode)
            TargetNode.walkable = true;
        SetAnimation(AnimType.idle);
        StopAllCoroutines();
        if(_movementPath!=null)
            _movementPath.Clear();
        pathIndex = 0;
        myBoundsPathNodes = PathManager.scr.GetBoundsOfObject(transform.position, productPattern[0].CellSize);
    }
    public bool _isDead;
    public void Death()
    {
        // vfx and collider;
        if (myCurrentNode)
            myCurrentNode.walkable = true;
        _isDead = true;
        gameObject.SetActive(false);
    }
    public void GetHitFeedBack()
    {
        // vfx and collider;
    }
    private void Attack()
    {
        SetAnimation(AnimType.attack);
    }
    private void SetAnimation(AnimType type)
    {
        switch (type)
        {
            case AnimType.idle:
                _myAnimator.SetBool("run", false);
                _myAnimator.SetBool("attack", false);
                break;
            case AnimType.run:
                _myAnimator.SetBool("run", true);
                break;
            case AnimType.attack:
                _myAnimator.SetBool("attack", true);
                break;
            default:
                break;
        }
    }
    
    private void SetDirection(PathNode start,PathNode end)
    {
        _spriteRenderer.flipX = end.x < start.x;
        
    }

    Health currentEnemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<Health>()&& !currentEnemy)
        {
            if (!_isDead)
            {
                StopMovement();
                StartCoroutine(AttackRoutine(collision.GetComponent<Health>()));
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentEnemy==null)
            return;
        if (collision.GetComponent<Health>() )
        {
            currentEnemy = null;
            StopAllCoroutines();
        }
    }
    private IEnumerator AttackRoutine(Health enemy)
    {
        
        currentEnemy = enemy;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        while (enemy.currentHealth >0)
        {
            Attack();
            if (enemy.currentHealth < 0)
                break;
            yield return new WaitForSeconds(_attackTime);
            enemy.GetHit(_damage, gameObject);
            SetAnimation(AnimType.idle);
            yield return new WaitForSeconds(2f);
        }
        SetAnimation(AnimType.idle);
        currentEnemy = null;
    }
    private float _attackTime;
    private void GetAnimClipTimes()
    {
        AnimationClip[] clips= _myAnimator.runtimeAnimatorController.animationClips;
        foreach (var item in clips)
        {
            if (item.name == "attack")
                _attackTime = item.length;
        }
    }
}
