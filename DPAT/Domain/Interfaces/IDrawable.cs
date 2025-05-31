using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT.Domain.Interfaces
{
    public interface IDrawable
    {
        public void Accept(IVisitor visitor);
    }
}
