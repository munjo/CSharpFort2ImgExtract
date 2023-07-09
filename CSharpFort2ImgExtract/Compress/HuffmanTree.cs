using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFort2ImgExtract.Compress
{

    class HuffmanTree
    {
        public Node DataTree { get; set; }
        public Node BackPosTree { get; set; }
        
        /// <summary>
        /// 항목의 개수를 센 사전을 만들고 트리 만들기
        /// </summary>
        /// <param name="source"></param>
        public void Build(List<LZSSValue> source)
        {
            Dictionary<int, int> dataFrequencies = new Dictionary<int, int>();
            Dictionary<int, int> backPosFrequencies = new Dictionary<int, int>();

            for (int i = 0; i < source.Count; i++)
            {
                if (!dataFrequencies.ContainsKey(source[i].DataValue))
                {
                    dataFrequencies.Add(source[i].DataValue, 0);
                }

                dataFrequencies[source[i].DataValue]++;

                if (source[i].DataValue > 255)
                {
                    int backValue = NumberBits(source[i].BackPos);

                    if (!backPosFrequencies.ContainsKey(backValue))
                    {
                        backPosFrequencies.Add(backValue, 0);
                    }

                    backPosFrequencies[backValue]++;
                }
            }

            this.DataTree = TreeBuild(dataFrequencies);
            this.BackPosTree = TreeBuild(backPosFrequencies);
        }

        /// <summary>
        /// 개수가 입력된 사전을 토대로 트리 만들기
        /// </summary>
        /// <param name="frequencies"></param>
        private Node TreeBuild(Dictionary<int, int> frequencies)
        {
            List<Node> nodes = new List<Node>();
            Node treeNodes = null;

            // 노드 추가
            foreach (KeyValuePair<int, int> symbol in frequencies)
            {
                nodes.Add(new Node(symbol.Key, symbol.Value));
            }

            // 노드 재정렬
            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.source).OrderBy(node => node.freq).ToList();

                if (orderedNodes.Count >= 2)
                {
                    // 처음 두 항목 가져오기
                    List<Node> taken = orderedNodes.Take(2).ToList();

                    // 빈도가 높은 노드를 결합하여 상위 노드 생성
                    Node parent = new Node(0, taken[0].freq + taken[1].freq, taken[0], taken[1]);

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                treeNodes = nodes.FirstOrDefault();
            }

            return treeNodes;
        }

        public BitArray Encode(List<LZSSValue> source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Count; i++)
            {
                List<bool> encodedSymbol = Traverse(this.DataTree, source[i].DataValue, new List<bool>());
                encodedSource.AddRange(encodedSymbol);

                if(source[i].DataValue > 255)
                {
                    if(this.BackPosTree == null)
                    {
                        continue;
                    }

                    int backValue = NumberBits(source[i].BackPos);

                    encodedSymbol = Traverse(this.BackPosTree, backValue, new List<bool>());
                    encodedSource.AddRange(encodedSymbol);

                    List<bool> releaseBits = ReleaseBits(source[i].BackPos, backValue);
                    encodedSource.AddRange(releaseBits);
                }
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        private int NumberBits(int source)
        {
            int backValue = 0;
            if (source != backValue)
            {
                for (int j = 1, bit = 1; j <= 13; j++, bit *= 2)
                {
                    if (source / bit == 1)
                    {
                        backValue = j;
                    }
                }
            }

            return backValue;
        }

        private List<bool> ReleaseBits(int source, int NumberBit)
        {
            BitArray bitArray = new BitArray(new int[] { source });

            List<bool> bitList = new List<bool>();

            for(int i = NumberBit; i > 1; i--)
            {
                bitList.Add(bitArray[i - 2]);
            }

            return bitList;
        }

        private List<bool> Traverse(Node node, int symbol, List<bool> data)
        {
            // Leaf
            if (node.rightNode == null && node.leftNode == null)
            {
                if (symbol.Equals(node.source))
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

                if (node.leftNode != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);
                    left = Traverse(node.leftNode, symbol, leftPath);
                }

                if (node.rightNode != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Traverse(node.rightNode, symbol, rightPath);
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
