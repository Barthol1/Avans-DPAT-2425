using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class DeterministicValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            var groupedBySource = fsm.Transitions.GroupBy(t => t.Connection.Item1.Identifier);
            foreach (var group in groupedBySource)
            {
                var transitions = group.ToList();
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
                            throw new InvalidOperationException($"Nondeterministic outgoing transitions from state '{a.Connection.Item1.Name}' with trigger '{a.Trigger ?? "<none>"}'. Conflicting transitions: {a.Identifier} and {b.Identifier}.");
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