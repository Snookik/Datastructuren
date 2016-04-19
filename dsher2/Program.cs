using System;
using System.IO;

namespace SkipList
{
    class Program
    {
        static void Main(string[] args)
        {
            SkipList S = new SkipList(20, 1000000);

         //   Console.SetIn(File.OpenText("..\\..\\..\\..\\opg8-vb\\SampleTestCases\\ranktest.in"));
            //Console.SetIn(File.OpenText("..\\..\\..\\..\\SampleTestCases\\Successor.in"));

            char[] sep = { ' ' };
            while (true)
            {
                string line = Console.ReadLine();
                if (line == null) break;
                string[] parts = line.Split(sep);
                if (parts[0] == "T")
                {
                    int number = Convert.ToInt32(parts[1]);
                    int score = Convert.ToInt32(parts[2]);
                    S.insert(score, number);
                }
                else if (parts[0] == "G")
                {
                    int id = Convert.ToInt32(parts[1]);
                    S.PrintScoreTable(id);
                }
                else if (parts[0] == "R")
                {
                    int number = Convert.ToInt32(parts[1]);
                    Console.WriteLine(S.getRank(number));
                }
            }
          //  S.printAll();
        }
    }

    class SkipList
    {
        int size;
        SkipListNode head;
        Random rnd;
        double [] prob;
        SkipListNode[] position;
        int nElements;

        class SkipListNode
        {
            public int v;
            public int k;
            public SkipListNode[] next;
            public int[] distanceToNext;
            public int size;
            public SkipListNode prev;

            public SkipListNode(int size)
            {
                this.size = size;
                next = new SkipListNode[size];
                distanceToNext = new int[size];
                prev = null;
            }

            public SkipListNode(int size, int k, int v)
            {
                prev = null;
                this.size = size;
                this.v = v;
                this.k = k;
                next = new SkipListNode[size];
                distanceToNext = new int[size];
            }
        }


        public SkipList(int size, int nPositions)
        {
            nElements = 0;
            position = new SkipListNode[nPositions + 1];
            this.size = size;
            rnd = new Random(666);
            head = new SkipListNode(size);
            prob = new double[size];
            prev = new SkipListNode[size];
            double currentProb = 1.0;
            for (int i = size - 1; i >= 0; i--)
            {
                prob[i] = currentProb;
                currentProb /= 2.0;
            }
        }

        int getRandomSize()
        {
            double u = rnd.NextDouble();
            for (int i = 0; i < size-1; i++)
            {
                if (u < prob[i]) return size-i;
            }
            return 1;
        }

        void insert(int k, int n, int lvl)
        {
            int s = getRandomSize();
            SkipListNode newNode = new SkipListNode(s, k, n);
            position[n] = newNode;

            for (int i = 0; i < s; i++)
            {
                int stprev = size - prev[size - s+i].size;
                int myst = size - s;
                newNode.next[i] = prev[size - s+i].next[myst - stprev+i];
                prev[size - s+i].next[myst - stprev+i] = newNode;
            }
            newNode.prev = prev[size - 1];

            if(newNode.next[s - 1] != null)
             newNode.next[s - 1].prev = newNode;
  
            int myIdx = newNode.prev == head ? 1 : getIndex(newNode.prev.k)+1;
            for (int i = 0; i < s; i++)
            {
                int stprev = size - prev[size - s + i].size;
                int myst = size - s;
                int prevDist = prev[size - s + i].distanceToNext[myst - stprev + i];
                //int distToThis = distance(prev[size - s + i], newNode);
                    int idxPrev;
                    if (prev[size - s + i] == head) idxPrev = 0;
                    else
                    {
                        int kp = prev[size - s + i].k;
                        idxPrev = getIndex(kp);
                    }
                    int distanceToMe = myIdx - idxPrev;
                if (newNode.next[i] == null) // this is the last node on this level
                {
                    //if(prev[size-s+i]==head) idx 
                    //int kp = prev[size - s + i].k;
                    //int idx = getIndex(kp);
                    newNode.distanceToNext[i] =  nElements - myIdx;
                    prev[size - s + i].distanceToNext[myst - stprev + i] = distanceToMe;
                }
                else
                {
                    int prevDistance = prev[size - s + i].distanceToNext[myst - stprev + i];
                    newNode.distanceToNext[i] = prevDistance - distanceToMe + 1;
                    prev[size - s + i].distanceToNext[myst - stprev + i] = distanceToMe;
//                    int idxPrev
                }
            }
            for (int i = 0; i < size-s; i++)
            {
                int stprev = size - prev[i].size;
                //int myst = size - s;
                prev[i].distanceToNext[i - stprev]++;
            }
        }

        public int getIndex(int k) // K is in the skip list 
        {
            int idx = 0;
            int lvl = 0;
            SkipListNode n = head;
            int internLvl = 0;
            while (true)
            {
                if (n.next[internLvl] == null)
                {
                    lvl++;
                    internLvl++;
                    if (lvl == size) // last one... insert here
                    {
                        Console.WriteLine("Shouldn't happen!!!("+k+")");
                       // insert(k, v, internLvl);
                        break;
                    }
                }
                else if (n.next[internLvl].k == k)
                {
                    return idx+n.distanceToNext[internLvl];
                }
                else if (n.next[internLvl].k > k) // advance
                {
                    idx += n.distanceToNext[internLvl];
                    int newStart = n.size - n.next[internLvl].size;
                    n = n.next[internLvl];
                    internLvl = internLvl - newStart;// correct the level for this node
                }
                else // go to next level or insert here!
                {
                    lvl++;
                    internLvl++;
                    if (lvl == size) // insert here!
                    {
                        Console.WriteLine("Shouldn't happen V2.0!!!(" + k + ")");
                        break;
                    }
                }
            }
            return idx;
        }

        int distance(SkipListNode a, SkipListNode b)
        {

            return 0;
        }

        SkipListNode[] prev; // previous array

        public void insert(int k, int v)
        {
            nElements++;
            int i = 0;
            for(;i<size;i++)
            {
                if(head.next[i]!=null) break;
            }
            if (i == size) // first node
            {
                head.next[i - 1] = new SkipListNode(1, k, v);
                position[v] = head.next[i - 1];
                head.next[i - 1].distanceToNext[0] = 0;
                for (int j = 0; j < size; j++)
                    head.distanceToNext[j] = 1;
            }
            else // find a spot to insert it
            {
                int lvl = 0;
                int internLvl = 0;
                SkipListNode n = head;
                while (true)
                {
                    if (n.next[internLvl] == null)
                    {
                        prev[lvl] = n;
                        lvl++;
                        internLvl++;
                        if (lvl == size) // last one... insert here
                        {
                            insert(k, v, internLvl);
                            break;
                        }
                    }
                    else if (n.next[internLvl].k == k) // same score...
                    {
                        // we don't insert, but we mark it
                        n = n.next[internLvl];
                        position[v] = n;
                        // update all distances
                        for (i = 0; i < n.size; i++)
                            n.distanceToNext[i]++;

                        for ( i = 0; i < size - n.size; i++)
                        {
                            int stprev = size - prev[i].size;
                            prev[i].distanceToNext[i - stprev]++;
                        }

                        break;
                    }
                    else if (n.next[internLvl].k > k) // advance
                    {
                        prev[lvl] = n;
                        int newStart = n.size - n.next[internLvl].size;
                        n = n.next[internLvl];
                        internLvl = internLvl - newStart;// correct the level for this node
                    }
                    else // go to next level or insert here!
                    {
                        prev[lvl] = n;
                        lvl++;
                        internLvl++;
                        if (lvl == size) // insert here!
                        {
                            insert(k, v, internLvl);
                            break;
                        }
                    }
                }
            }
        }

        public void PrintScoreTable(int id)
        {
            int i;
            SkipListNode n = position[id];
            SkipListNode tmp = n;
            for (i = 0; i < 5; i++)
            {
                if (tmp.prev == head || tmp.prev == null) 
                    break;
                tmp = tmp.prev;
                //if (tmp.next[tmp.size - 1] == null) break;
                //tmp = tmp.next[tmp.size - 1];
            }
            while (tmp != n)
            {
                Console.WriteLine(tmp.v);
                tmp = tmp.next[tmp.size-1];
            }
            Console.WriteLine(tmp.v); // print the one with the requested id
            tmp = tmp.next[tmp.size - 1];
            for (i = 0; i < 4; i++)
            {
                if (tmp == null) break;
                Console.WriteLine(tmp.v);
                tmp = tmp.next[tmp.size - 1];
            }

        }

        public void printAll()
        {
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine("Level " + i);
                for (SkipListNode n = head.next[i]; n != null;)
                {
                    int myst = size - n.size;
                    Console.Write(n.k + " ");
                    n = n.next[i-myst];
                }
                Console.WriteLine();
            }
        }

        public void printLvl(int lvl)
        {
            //for (int i = 0; i < size; i++)
            {
                Console.WriteLine("Level " + lvl);
                for (SkipListNode n = head.next[lvl]; n != null; )
                {
                    //int prevst = size - n.size;
                    int myst = size - n.size;
                    Console.Write(n.k + " ");
                    n = n.next[lvl - myst];
                }
                Console.WriteLine();
            }
        }

        public int getRank(int number)
        {
            return getIndex(position[number].k);
        }

        /*
        SkipListNode find(int k)
        {
            return null;
        }*/
    }
        
}
