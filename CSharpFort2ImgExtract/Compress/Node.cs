using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract.Compress
{
    class Node
    {
        public int source;
        public int freq;
        public Node leftNode = null;
        public Node rightNode = null;

        public Node(int alphabet, int freq)
        {
            this.source = alphabet;
            this.freq = freq;
        }

        public Node(int source, int freq, Node left, Node right)
        {
            this.source = source;
            this.freq = freq;
            this.leftNode = left;
            this.rightNode = right;
        }
    }
}
