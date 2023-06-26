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
        private List<Node> nodes = new List<Node>();
        public Node Root { get; set; }
        public Dictionary<int, int> Frequencies = new Dictionary<int, int>();

        public void Build(int[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (!Frequencies.ContainsKey(source[i]))
                {
                    Frequencies.Add(source[i], 0);
                }

                Frequencies[source[i]]++;
            }

            // 노드 추가
            foreach (KeyValuePair<int, int> symbol in Frequencies)
            {
                nodes.Add(new Node(symbol.Key, symbol.Value));
            }

            // 노드 재정렬
            while (nodes.Count > 1)
            {
                List<Node> orderedNodes = nodes.OrderBy(node => node.freq).ToList();

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

                this.Root = nodes.FirstOrDefault();

            }

        }

        public BitArray Encode(ushort[] source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

    }
}
