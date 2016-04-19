using System;
using System.IO;
// Petar Kostic
// 4075897

namespace MinMaxHeap
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.SetIn(File.OpenText("t.txt"));

            char [] sep = {' '};
            MinMaxHeap minmax = new MinMaxHeap(100000);
            while (true)
            {
                string line = Console.ReadLine();
                if (line == "S") break;
                string[] parts = line.Split(sep);
                if (parts[0] == "P")
                {
                    int number = minmax.getMax();
                    Console.WriteLine("P "+number.ToString());
                }
                else if (parts[0] == "D")
                {
                    int number = minmax.getMin();
                    Console.WriteLine("D " + number.ToString());
                }
                else if (parts[0] == "N")
                {
                    int number = Convert.ToInt32(parts[1]);
                    int score = Convert.ToInt32(parts[2]);
                    minmax.add(number, score);
                }
                else // V
                {
                    int number = Convert.ToInt32(parts[1]);
                    int score = Convert.ToInt32(parts[2]);
                    minmax.changeScore(number, score);
                }
                /*
                if (!minmax.isMinMaxHeap())
                {
                    Console.WriteLine("Error: "+line);
                }*/
            }
        }
    }

    class MinMaxHeap
    {

        public bool isMinHeap(int idx)
        {
            int ch1 = idx * 2;
            int ch2 = ch1 + 1;
            int last = minHeap[0];
            if (ch1 > last) return true;
            if (ch2 <= last)
            {
                if (scores[minHeap[idx]] > scores[minHeap[ch2]]) return false;
                if (scores[minHeap[idx]] == scores[minHeap[ch2]] &&
                    minHeap[idx] < minHeap[ch2])
                    return false;

                if (!isMinHeap(ch2)) return false;
            }
            if (scores[minHeap[idx]] > scores[minHeap[ch1]]) return false;
            if (scores[minHeap[idx]] == scores[minHeap[ch1]] &&
                minHeap[idx] < minHeap[ch1])
                return false;

            return isMinHeap(ch1);
        }
        public bool isMaxHeap(int idx)
        {
            int ch1 = idx * 2;
            int ch2 = ch1 + 1;
            int last = maxHeap[0];
            if (ch1 > last) return true;
            if (ch2 <= last)
            {
                if (scores[maxHeap[idx]] < scores[maxHeap[ch2]]) return false;
                if (scores[maxHeap[idx]] == scores[maxHeap[ch2]] &&
                    maxHeap[idx] > maxHeap[ch2])
                    return false;
                if (!isMaxHeap(ch2)) return false;
            }
            if (scores[maxHeap[idx]] < scores[maxHeap[ch1]]) return false;
            if (scores[maxHeap[idx]] == scores[maxHeap[ch1]] &&
                maxHeap[idx] > maxHeap[ch1])
                return false;
            return isMaxHeap(ch1);
        }
        public bool isMinMaxHeap()
        {
            return isMinHeap(1) &&
                isMaxHeap(1);
        }





        static int INF =  100000000;

        delegate bool Compare(int a, int b);

        static Compare lessThan = (int a, int b) => a < b;
        static Compare greaterThan = (int a, int b) => a > b;

        int[] scores;
        int[] minHeap;
        int[] maxHeap;
        int[] playerMinIdx;
        int[] playerMaxIdx;

        void heapify(int[] heap, Compare cmp1, Compare cmp2, int [] player)
        {
            for (int i = heap[0] / 2; i > 0; i--)
                Down(i, heap, player, cmp1, cmp2);
        }

        public MinMaxHeap(int n)
        {
            scores = new int[n + 1];
            minHeap = new int[n + 1];
            maxHeap = new int[n + 1];
            playerMinIdx = new int[n + 1];
            playerMaxIdx = new int[n + 1];
            minHeap[0] = maxHeap[0] = 0;            
        }

        int getChild(int[] heap, int i, Compare cmp1, Compare cmp2, ref bool noChild)
        {
            noChild = false;
            int last = heap[0];
            int lch = i * 2;
            int rch = i * 2 + 1;
            if (lch > last) { noChild = true;  return -1; } // no childs
            if (rch > last) return lch;
            if (cmp1(scores[heap[lch]], scores[heap[rch]])) return lch;
            if (scores[heap[lch]] == scores[heap[rch]] && cmp2(heap[lch], heap[rch])) return lch;
            return rch;
        }

        public int getMax()
        {
            bool noChild = false;
            int r = maxHeap[1];
            int otherHeapIdx = playerMinIdx[maxHeap[1]]; // remove this one from minHeap
            maxHeap[1] = maxHeap[maxHeap[0]--];
            minHeap[otherHeapIdx] = minHeap[minHeap[0]--];
            playerMinIdx[minHeap[minHeap[0]+1]] = otherHeapIdx;
            if (minHeap[0]>1 && (scores[minHeap[otherHeapIdx]] < scores[minHeap[otherHeapIdx/2]] ||
                scores[minHeap[otherHeapIdx]] == scores[minHeap[otherHeapIdx / 2]] && minHeap[otherHeapIdx] > minHeap[otherHeapIdx/2])
                )
            {
                Up(otherHeapIdx, minHeap, playerMinIdx, lessThan, greaterThan);
            }
            else if (minHeap[0] > 1)
            {
                int minCh = getChild(minHeap, otherHeapIdx, lessThan, greaterThan, ref noChild);
                if(!noChild) // if there is a child...
                    if (scores[minHeap[minCh]] < scores[minHeap[otherHeapIdx]])
                {
                    Down(otherHeapIdx, minHeap, playerMinIdx, lessThan, greaterThan);
                }
            }
            Down(1, maxHeap, playerMaxIdx, greaterThan, lessThan);
            return r;
        }

        public int getMin()
        {
            bool noChild = false;
            int r = minHeap[1];
            int otherHeapIdx = playerMaxIdx[minHeap[1]]; // remove this one from minHeap
            minHeap[1] = minHeap[minHeap[0]--];
            maxHeap[otherHeapIdx] = maxHeap[maxHeap[0]--];
            playerMaxIdx[maxHeap[maxHeap[0]+1]] = otherHeapIdx;
            if (maxHeap[0] > 1 && (scores[maxHeap[otherHeapIdx]] > scores[maxHeap[otherHeapIdx / 2]] ||
                scores[maxHeap[otherHeapIdx]] == scores[maxHeap[otherHeapIdx / 2]] && maxHeap[otherHeapIdx] < maxHeap[otherHeapIdx / 2]
                ))
            {
                Up(otherHeapIdx, maxHeap, playerMaxIdx, greaterThan, lessThan);
            }
            else if (maxHeap[0] > 1)
            {
                int maxCh = getChild(maxHeap, otherHeapIdx, greaterThan, lessThan, ref noChild);
                if (! noChild) // if there is a child...
                    if (scores[maxHeap[maxCh]] > scores[maxHeap[otherHeapIdx]])
                    {
                        Down(otherHeapIdx, maxHeap, playerMaxIdx, greaterThan, lessThan);
                    }
            }
            Down(1, minHeap, playerMinIdx, lessThan, greaterThan);
            return r;
        }

        public void add(int number, int score)
        {
            scores[number] = score;
            minHeap[++minHeap[0]] = number;
            maxHeap[++maxHeap[0]] = number;
            playerMinIdx[number] = minHeap[0];
            playerMaxIdx[number] = maxHeap[0];
            Up(minHeap[0], minHeap, playerMinIdx, lessThan, greaterThan);
            Up(maxHeap[0], maxHeap, playerMaxIdx, greaterThan, lessThan);
        }

        public void changeScore(int number, int score)
        {
            // heapify:
            /*
            scores[number] = score;
            heapify(minHeap, lessThan, greaterThan, playerMinIdx);
            heapify(maxHeap, greaterThan, lessThan, playerMaxIdx);*/
            /**/
            int prevScore = scores[number];
            if (score == prevScore) return;
            //scores[number] = score;
            int idx = playerMaxIdx[number];

           // bool r;
            //r = isMinMaxHeap();

            scores[number] = INF;
            Up(idx, maxHeap, playerMaxIdx, greaterThan, lessThan);
            maxHeap[1] = maxHeap[maxHeap[0]--];
            Down(1, maxHeap, playerMaxIdx, greaterThan, lessThan);

            idx = playerMinIdx[number];
            scores[number] = -INF;
            Up(idx, minHeap, playerMinIdx, lessThan, greaterThan);
            minHeap[1] = minHeap[minHeap[0]--];
            Down(1, minHeap, playerMinIdx, lessThan, greaterThan);

            //r = isMinMaxHeap();

            //getMax(); // remove the element
            //r = isMinMaxHeap();
            add(number, score);
            //r = isMinMaxHeap();
            /**/

            /**

            int prevScore = scores[number];
            if (score == prevScore) return;
            int idx = playerMaxIdx[number];
            scores[number] = score;

            if (score > prevScore) // go up
            {
                Up(idx, maxHeap, playerMaxIdx, greaterThan, lessThan);
                Down(idx/2, maxHeap, playerMaxIdx, greaterThan, lessThan);
            }
            else
            {
                Down(idx, maxHeap, playerMaxIdx, greaterThan, lessThan);
                Up(idx*2, maxHeap, playerMaxIdx, greaterThan, lessThan);
                Up(idx * 2+1, maxHeap, playerMaxIdx, greaterThan, lessThan);
            }
            idx = playerMinIdx[number];
            if (score < prevScore) // go up
            {
                Up(idx, minHeap, playerMinIdx, lessThan, greaterThan);
                Down(idx/2, minHeap, playerMinIdx, lessThan, greaterThan);
            }
            else
            {
                Down(idx, minHeap, playerMinIdx, lessThan, greaterThan);
                Up(idx*2, minHeap, playerMinIdx, lessThan, greaterThan);
                Up(idx * 2+1, minHeap, playerMinIdx, lessThan, greaterThan);
            }/**/
        }

        void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        void Up(int i, int [] heap, int [] idx, Compare cmp1, Compare cmp2)
        {
            if (i == 1) return;
            if (i > heap[0]) return;
            int parent = i/2;
            if (cmp1(scores[heap[i]], scores[heap[parent]]) || 
                scores[heap[i]]==scores[heap[parent]] && cmp2(heap[i],heap[parent]))
            {
                Swap(ref heap[parent], ref heap[i]);
                Swap(ref idx[heap[parent]], ref idx[heap[i]]);
                Up(parent, heap, idx, cmp1, cmp2);
            }
        }

        void Down(int i, int [] heap, int [] idx, Compare cmp1, Compare cmp2)
        {
            if (i == 0) return;
            bool noChild = false;
            int child = getChild(heap, i, cmp1, cmp2, ref noChild);
            if (!noChild) // if there is a child
            {
                if (cmp1(scores[heap[child]], scores[heap[i]]) ||
                    scores[heap[child]]==scores[heap[i]] && cmp2(heap[child],heap[i])) // compare with child
                {
                    Swap(ref heap[i], ref heap[child]);
                    Swap(ref idx[heap[i]], ref idx[heap[child]]);
                    Down(child, heap, idx, cmp1, cmp2);
                }
            }
        }
    }
}
