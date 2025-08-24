using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class UnreachableStateValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            var initial = fsm.States.OfType<InitialState>().FirstOrDefault();
            if (initial == null)
            {
                throw new Exception("FSM must have exactly one initial state.");
            }


            var childToParent = BuildChildToParentMap(fsm);

            var visited = new HashSet<string>();
            var queue = new Queue<IState>();

            void VisitAndPropagateParents(IState state)
            {
                if (!visited.Add(state.Identifier)) return;

                var currentId = state.Identifier;
                while (childToParent.TryGetValue(currentId, out var parent))
                {
                    if (!visited.Add(parent.Identifier)) break;

                    queue.Enqueue(parent);
                    currentId = parent.Identifier;
                }
                queue.Enqueue(state);
            }

            VisitAndPropagateParents(initial);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var outgoing = fsm.Transitions.Where(t => t.Connection.Item1.Identifier == current.Identifier)
                                              .Select(t => t.Connection.Item2)
                                              .ToList();
                foreach (var next in outgoing)
                {
                    VisitAndPropagateParents(next);
                }
            }

            var unreachable = fsm.States
                .Where(s => s is not InitialState && !visited.Contains(s.Identifier))

                .Where(s => !childToParent.ContainsKey(s.Identifier))
                .ToList();
            if (unreachable.Any())
            {
                var names = string.Join(", ", unreachable.Select(s => s.Name));
                throw new InvalidOperationException($"Unreachable state(s): {names}");
            }
        }

        private static Dictionary<string, CompoundState> BuildChildToParentMap(FSM fsm)
        {
            var map = new Dictionary<string, CompoundState>();

            void Recurse(CompoundState compound)
            {
                foreach (var child in compound.SubStates)
                {
                    map[child.Identifier] = compound;
                    if (child is CompoundState nested)
                    {
                        Recurse(nested);
                    }
                }
            }

            foreach (var rootCompound in fsm.States.OfType<CompoundState>())
            {
                Recurse(rootCompound);
            }

            return map;
        }
    }
}