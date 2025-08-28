using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class DeterministicValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            var groupedBySource = fsm.Components.OfType<Transition>().GroupBy(t => t.SourceState.Name);
            foreach (var group in groupedBySource)
            {
                var transitions = group.ToList();

                var automaticTransitions = transitions.Where(t => string.IsNullOrEmpty(t.Trigger)).ToList();
                var hasUnguardedAutomatic = automaticTransitions.Any(t => string.IsNullOrWhiteSpace(t.Guard));
                if (hasUnguardedAutomatic && transitions.Count > 1)
                {
                    var stateName = transitions.First().SourceState.Name;
                    throw new InvalidOperationException($"Nondeterministic outgoing transitions from state '{stateName}' due to an unguarded automatic transition coexisting with other transitions.");
                }

                for (int i = 0; i < transitions.Count; i++)
                {
                    for (int j = i + 1; j < transitions.Count; j++)
                    {
                        var a = transitions[i];
                        var b = transitions[j];

                        var sameTrigger = string.Equals(a.Trigger, b.Trigger, StringComparison.Ordinal);
                        if (!sameTrigger) continue;

                        var guardA = NormalizeGuard(a.Guard);
                        var guardB = NormalizeGuard(b.Guard);

                        var overlap = (string.IsNullOrEmpty(guardA) && string.IsNullOrEmpty(guardB)) ||
                                      string.Equals(guardA, guardB, StringComparison.OrdinalIgnoreCase);
                        if (overlap)
                        {
                            throw new InvalidOperationException($"Nondeterministic outgoing transitions from state '{a.SourceState.Name}' with trigger '{a.Trigger ?? "<none>"}'. Conflicting transitions: {a.SourceState.Name} and {b.SourceState.Name}.");
                        }
                    }
                }
            }
        }

        private static string NormalizeGuard(string? guard)
        {
            if (string.IsNullOrWhiteSpace(guard)) return string.Empty;
            return guard.Trim();
        }
    }
}