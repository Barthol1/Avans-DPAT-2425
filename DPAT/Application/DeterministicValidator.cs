using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class DeterministicValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            var transitions = fsm.Transitions;
            bool isValid = true;
            transitions.GroupBy(t => t.Destination).ToList().ForEach(group =>
            {
                if (group.Count() > 1)
                {
                    // check if the transitions have the same trigger
                    group.GroupBy(t => t.Trigger).ToList().ForEach(triggerGroup =>
                    {
                        //also check if the guard is the same
                        triggerGroup.GroupBy(t => t.Guard).ToList().ForEach(guardGroup =>
                        {
                            if (guardGroup.Count() > 1)
                            {
                                isValid = false;
                            }
                        });
                    });
                }
            });
            return isValid;
        }
    }
}