using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BossAttacks.Modules;

namespace BossAttacks.Utils
{
    public class ModException : Exception
    {
        public ModException(string message) : base(message)
        {
        }
    }
}
