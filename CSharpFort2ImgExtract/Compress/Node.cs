using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract.Compress
{
    class Node
    {
        public int alphabet;
        public int freq;
        public Node leftNode = null;
        public Node rightNode = null;

        public Node(int alphabet, int freq)
        {
            this.alphabet = alphabet;
            this.freq = freq;
        }

        public Node(int alphabet, int freq, Node left, Node right)
        {
            this.alphabet = alphabet;
            this.freq = freq;
            this.leftNode = left;
            this.rightNode = right;
        }

        public List<bool> Traverse(int symbol, List<bool> data)
        {
            // Leaf
            if (rightNode == null && leftNode == null)
            {
                if (symbol.Equals(this.alphabet))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (leftNode != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = leftNode.Traverse(symbol, leftPath);
                }

                if (rightNode != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = rightNode.Traverse(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
