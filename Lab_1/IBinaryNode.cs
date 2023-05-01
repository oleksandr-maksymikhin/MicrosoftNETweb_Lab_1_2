using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public interface IBinaryNode<T, TVal> where T : IBinaryNode<T, TVal>
    {
        public T? Parent { get; set; }
        public T? Left { get; set; }
        public T? Right { get; set; }
        public TVal Value { get; set; }
    }
}
