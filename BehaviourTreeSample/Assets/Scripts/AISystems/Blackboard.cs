using UnityEngine;
using UnityEngine.AI;

namespace AISystems
{
    public class Blackboard
    {
        public Blackboard(GameObject owner)
        {
            transform = owner.transform;
            agent = owner.GetComponent<NavMeshAgent>();
        }

        // owner
        public Transform transform;
        public NavMeshAgent agent;

        // target
        public Transform target;
    }
}
