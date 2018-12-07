using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCode.Logic
{
    public class Tree
    {
        public string Name { get; set; } = "";
        public int Weight { get; set; } = 0;
        public int ChildWeight { get; set; } = 0;

        public int TotalWeight { get { return Weight + ChildWeight; } }

        public int CheckWeight(Dictionary<string, Tree> nodes)
        {
            ChildWeight = 0;
            foreach (string child in Childs)
            {
                ChildWeight += nodes[child].CheckWeight(nodes);
            }

            return TotalWeight;
        }

        public int GetUnbalance(Dictionary<string, Tree> nodes)
        {
            int unbalance;
            foreach (string child in Childs)
            {
                unbalance = nodes[child].GetUnbalance(nodes);

                if (unbalance != -1)
                {
                    return unbalance;
                }
            }

            for (int i = 0; i < Childs.Count; i++)
            {
                int weight = nodes[Childs[i]].TotalWeight;
                int nextWeight = nodes[Childs[(i + 1) % Childs.Count]].TotalWeight;
                if (weight != nextWeight)
                {
                    int matchCount = 0;

                    foreach (string child in Childs)
                    {
                        if (weight == nodes[child].TotalWeight)
                        {
                            matchCount++;
                        }
                    }

                    if (matchCount == 1)
                    {
                        return nextWeight - nodes[Childs[i]].ChildWeight;
                    }
                }
            }

            return -1;
        }

        public List<string> Childs { get; set; } = new List<string>();
        public string Parent { get; set; } = "";
    }
}
