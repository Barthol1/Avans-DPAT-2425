using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class FSM : IFSMComponent
    {
        private List<IFSMComponent> components;
        public FSM()
        {
            components = new List<IFSMComponent>();
        }

        public void Add(IFSMComponent component)
        {
            components.Add(component);
        }

        public void Remove(IFSMComponent component) {
            components.Remove(component);
        }

        public void Print()
        {
            foreach (var component in components) {
                component.Print();
            }
        }

        public void Validate() {
            foreach (var component in components) {
                component.Validate();
            }
        }

    }
}