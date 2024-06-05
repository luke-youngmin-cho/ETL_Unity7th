using AISystems;
using UnityEngine;

[RequireComponent(typeof(BehaviourTree))]
public class NonPlayerController : MonoBehaviour
{
    private BehaviourTree _behaviourTree;

    [Header("AI Behaviours")]
    [Header("Seek")]
    [SerializeField] float _seekRadius = 5.0f;
    [SerializeField] float _seekHeight = 1.0f;
    [SerializeField] float _seekAngle = 90.0f;
    [SerializeField] LayerMask _seekTargetMask;
    [SerializeField] float _seekMaxDistance = 10.0f;

    private void Awake()
    {
        // after ( chaining )
        _behaviourTree = GetComponent<BehaviourTree>();
        _behaviourTree.StartBuild()
            .Selector()
                .Parallel(1)
                    .Seek(_seekRadius, _seekHeight, _seekAngle, _seekTargetMask, _seekMaxDistance)
                    .Decorator(() => true)
                        .Execution(() => Result.Failure)
                .ExitCurrentComposite()
                .RandomSelector()
                    .Execution(() => Result.Success)
                    .Execution(() => Result.Success);

        // before
        //_behaviourTree = GetComponent<BehaviourTree>();
        //_behaviourTree.blackboard = new Blackboard(gameObject);
        //_behaviourTree.root = new Root(_behaviourTree);
        //Selector selector1 = new Selector(_behaviourTree);
        //_behaviourTree.root.child = selector1;
        //Parallel parallel1 = new Parallel(_behaviourTree, 1);
        //selector1.children.Add(parallel1);
        //Execution seek = new Execution(_behaviourTree, () => Result.Success);
        //parallel1.children.Add(seek);
        //Execution attack = new Execution(_behaviourTree, () => Result.Failure);
        //Decorator decorator = new Decorator(_behaviourTree, () => true);
        //decorator.child = attack;
        //parallel1.children.Add(decorator);
        //RandomSelector randomSelector1 = new RandomSelector(_behaviourTree);
        //selector1.children.Add(randomSelector1);
        //Execution rest = new Execution(_behaviourTree, () => Result.Success);
        //randomSelector1.children.Add(rest);
        //Execution patrol = new Execution(_behaviourTree, () => Result.Success);
        //randomSelector1.children.Add(patrol);

    }
}
