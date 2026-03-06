using System.Collections.Generic;
using System.Linq;

namespace ProceduralDungeon
{
    public struct InputComposite
    {
        private readonly List<EasyBinding> bindings;

        public InputComposite(params EasyBinding[] bindings)
        {
            this.bindings = bindings.ToList();
        }

        public bool IsHeld() => bindings.Where(x => x.IsHeld).Count() > 0;
        public bool WasPressed() => bindings.Where(x => x.WasPressed).Count() > 0;
        public bool WasReleased() => bindings.Where(x => x.WasReleased).Count() > 0;
    }
}
