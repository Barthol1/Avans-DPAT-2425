using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class FSM : IFSMComponent
    {
        private readonly List<IFSMComponent> _components;
        public IEnumerable<IFSMComponent> Components => _components.AsReadOnly();

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

        public void Print(IVisitor visitor)
        {
            foreach (var component in _components)
            {
                component.Print(visitor);
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