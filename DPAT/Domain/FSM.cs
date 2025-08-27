using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class FSM : IFSMComponent
    {
        private List<IFSMComponent> _components;

        public FSM()
        {
            _components = [];
        }

        public void Add(IFSMComponent component)
        {
            _components.Add(component);
        }

        public void Remove(IFSMComponent component)
        {
            _components.Remove(component);
        }

        public void Print()
        {
            foreach (var component in _components)
            {
                component.Print();
            }
        }

        public void Validate()
        {
            foreach (var component in _components)
            {
                component.Validate();
            }
        }

    }
}