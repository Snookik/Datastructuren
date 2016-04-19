using System;
using System.IO;

namespace Datastruc6R
{
    public class HashTable
    {
        private const int Size = 40013;
        private int size;
        private Item[] buckets;

        public HashTable()
        {
            this.size = Size;
            this.buckets = new Item[size];
        }

        public HashTable(int capacity)
        {
            this.size = capacity;
            this.buckets = new Item[size];
        }

        public void AddItem(string key, int value)
        {
            // Plaats op de array bepalen
            int index = HashFunction(key);

            // Als de plaats op de bucket leeg is
            if (buckets[index] == null)
            {
                // Dan voegen we een nieuwe Node toe op die plek
                buckets[index] = new Item(key, value);
            }
            // Zo niet, dan moeten we gaan chainen
            else
            {
                Item newNode = new Item(key, value);
                newNode.Next = buckets[index];
                buckets[index] = newNode;
            }
        }

        public int Get(string key)
        {
            // Plaats op de array bepalen
            int index = HashFunction(key);

            // Als deze plek niet leeg is
            if (buckets[index] != null)
            {
                // Doorloop de lijst
                for (Item n = buckets[index]; n != null; n = n.Next)
                {
                    // Als we het element met de key zijn tegengekomen
                    if (n.Key == key)
                    {
                        // Return de punten hiervan
                        return n.Value;
                    }
                }
            }
            // Zo niet, return 'bestaat niet'
            return -1;
        }

        public void AddPoints(string key, int value)
        {
            // Plaats op de array bepalen
            int index = HashFunction(key);

            // Als deze plek niet leeg is
            if (buckets[index] != null)
            {
                // Doorloop de lijst
                for (Item n = buckets[index]; n != null; n = n.Next)
                {
                    // Als we het element met de key zijn tegengekomen
                    if (n.Key == key)
                    {
                        n.Value = n.Value + value;
                    }
                }
            }
        }

        // Chained-Hash-Delete(T,x) uit de slides implementatie
        public void Delete(string key)
        {
            Item delPtr;
            Item P1;
            Item P2;
            // Plaats op de array bepalen
            int index = HashFunction(key);

            // Als deze plek leeg is
            if (buckets[index].Key != key)
            {
                Console.WriteLine(-1);
            }
            else if (buckets[index].Key == key && buckets[index].Next == null)
            {
                buckets[index] = buckets[index].Next;
            }
            else if (buckets[index].Key == key)
            {
                delPtr = buckets[index];
                buckets[index] = buckets[index].Next;
            }
            else
            {
                P1 = buckets[index].Next;
                P2 = buckets[index];

                while (P1 != null && P1.Key != key)
                {
                    P2 = P1;
                    P1 = P1.Next;
                }
                if (P1 == null)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    delPtr = P1;
                    P1 = P1.Next;
                    P2.Next = P1;
                }
            }

            
        }

        // Chained-Hash-Search(T,x) uit slides implementatie
        public bool Contains(string key)
        {
            // Plaats op de array bepalen
            int index = HashFunction(key);
            // Als deze plek niet leeg is
            if (buckets[index] != null)
            {
                // Doorloop de lijst
                for (Item n = buckets[index]; n != null; n = n.Next)
                {
                    if (n.Key == key)
                    {
                        // Dan returnen we true
                        return true;
                    }
                }
            }
            // Zo niet, dan returnen we false
            return false;
        }

        protected virtual int HashFunction(string key)
        {
            // Oude hash
            // return Math.Abs(key.GetHashCode() + 1 +
            //     (((key.GetHashCode() >> 5) + 1) % (size))) % size;

            int hash = 0;
            int index;

            // Convert de string naar ASCII waardes
            for (int i = 0; i < key.Length; i++)
            {
                // Als de eerstvolgende Char een nummer is dan voeren we deze berekening uit
                if (char.IsNumber(key[i]))
                {
                    hash = hash + ((int)key[i] - (int)'0');
                }
                // Zo niet, dan voeren we deze berekening uit
                else
                    hash = (hash + (int)key[i]) - (int)'a' + 1;
            }
            // Index returns location where we want our information to be stored in the hash table
            index = hash % Size;

            return index;
        }

        private class Item
        {
            public string Key { get; set; }
            public int Value { get; set; }
            public Item Next { get; set; }
            public Item(string key, int value)
            {
                this.Key = key;
                this.Value = value;
                this.Next = null;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Chaining for testing
            // capacity kan dan op 1 worden gezet, dus ->           
            // HashTable ht = new HashTable(1);

            HashTable Hashtable = new HashTable();

            for (string input = Console.ReadLine(); input != null || input != ""; input = Console.ReadLine())
            {
                // NIEUWE SPELER
                if (input[0] == 'N')
                {
                    string[] inputArr = input.Split(' ');
                    if (Hashtable.Contains(inputArr[1]) == false)
                        Hashtable.AddItem(inputArr[1], 0);
                    else
                        // Er bestaat al iemand met die naam
                        Console.WriteLine(-1);
                }

                // WEGLATEN VAN EEN SPELER
                if (input[0] == 'D')
                {
                    string[] inputArr = input.Split(' ');
                    if (Hashtable.Contains(inputArr[1]) == true)
                        Hashtable.Delete(inputArr[1]);
                    else
                        Console.WriteLine(-1);
                }

                // OPHOGEN PUNTEN VAN EEN SPELER
                if (input[0] == 'X')
                {
                    string[] inputArr = input.Split(' ');
                    int pluspunten = int.Parse(inputArr[2]);
                    if (Hashtable.Contains(inputArr[1]) == true)
                    {
                        Hashtable.AddPoints(inputArr[1], pluspunten);
                    }
                    else
                        Console.WriteLine(-1);
                }

                // PUNTENAANTAL OPVRAGEN
                if (input[0] == 'Q')
                {
                    string[] inputArr = input.Split(' ');
                    Console.WriteLine(Hashtable.Get(inputArr[1]));
                }
            }
            Console.ReadLine();
        }
    }
}
