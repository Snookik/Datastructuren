using System;
using System.IO;

// Naam: Petar Kostic
// Studentennummer: 4075897
namespace Datastructuren5
{
    struct klant
    {
        public int t;
        // tijdstap waarop de klant binnekomt
        public int p;
        // de hoeveelheid tijdstappen die het kost om te printen
        public int s; // de hoeveelheid tijdstappen die het Piet kost om de bouwplaat uit te snijden

        public int wachttijd;
        // Dit nog bijhouden:
        // helemaal klaar
        // binnen is gekomen bij piet = klaar is met printen



        public int p_remaining;
        public int s_trmaining;

        public klant(int input_t, int input_p, int input_s, int input_tp)
        {
            t = input_t;
            p = input_p;
            s = input_s;
            p_remaining = input_p;
            s_remaining = input_s;
            wachttijd = 0;

        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            // Declaraties queue classes
            Queue queueA = new Queue();
            Queue queueB = new Queue();
            Queue queueC = new Queue();

            // Piet stack
            Stack Piet = new Stack();

            // Eerste input
            string input = Console.ReadLine();

            // Array van structs met klanten
            klant[] klanten = new klant[2100000];
            int klanten_index = 1; 

            // Input Loop om array van klanten (structs) te vullen met de input.
            while (input != "sluit")
            {
                // Invoer
                string[] input_tps = input.Split(' ');        
                int input_t = int.Parse(input_tps[0]);          
                int input_p = int.Parse(input_tps[1]);          
                int input_s = int.Parse(input_tps[2]);
                int input_tp = input_t + input_p;

                klant k = new klant(input_t, input_p, input_s);      

                klanten[klanten_index] = k;
                klanten_index++;

                input = Console.ReadLine();
            }
            //------------------------------
            //        DE SIMULATIE
            //------------------------------
            // Juiste queue kiezen voor simulatie
            for (int i = 0; i < klanten.Length; i++)
            {
                if (klanten[i].t == i) // Als de tijd van binnenkomst gelijk is aan de actuele tijd -->
                {
                    // Klant kiest A
                    if (queueA.Length == Math.Min(queueA.Length, Math.Min(queueB.Length, queueC.Length)))
                    {
                        queueA.Enqueue(klanten[i]);
                    }
                    // Klant kiest B
                    else if (queueB.Length == Math.Min(queueA.Length, Math.Min(queueB.Length, queueC.Length)))
                    {
                        queueB.Enqueue(klanten[i]);
                    }
                    // Klant kiest C
                    else if (queueC.Length == Math.Min(queueA.Length, Math.Min(queueB.Length, queueC.Length)))
                    {
                        queueC.Enqueue(klanten[i]);
                    }
                }
                // Als een printer klaar is met een bouwplaat, dan komt hij op  stack
                // Als ze tegelijk klaar zijn dan A -> B -> C respectievelijk
                // Per queue voor de uitvoer checken of hij een nieuw record heeft bereikt voor langste wachttijd.

                if (queueA.q_Array[queueA.front].p_remaining == 0)
                { 
                    Piet.push(queueA.Dequeue);
                }

                if (queueB.q_Array[queueB.front].p_remaining == 0)
                {
                    Piet.push(queueB.Dequeue);
                }

                if (queueC.q_Array[queueB.front].p_remaining == 0)
                {
                    Piet.push(queueC.Dequeue);
                }
                // Counter, voor elke tijdstap gaat de printtijd omlaag
                queueA.q_Array[queueA.front].p_remaining--;
                queueB.q_Array[queueB.front].p_remaining--;
                queueC.q_Array[queueC.front].p_remaining--;
       
                // Snijden
                if (Piet.curKlant.s_remaining == 0)
                {
                    Piet.curKlant = Piet.pop();
                }
                Piet.curKlant.s_remaining--;


                // Uitvoer
                // klantnummer die het langst in de tij heeft gewacht (klant_index), 
                // gevolgd door een dubbele punt en een enkele spatie, 
                // met daarna het aantal tijdstappen dat de kalnt heeft gewacht

                // Op de tweede regel 
            }
        }
    }

    class Queue
    {
        public klant[] q_Array = new klant[700001];
        public klant tijdelijk;
        public int front, end = 0;
        public long Length;

        public void Enqueue(klant currentKlant)
        {
            q_Array[end] = currentKlant;
            if (end == Length)
                end = 1;
            else
            {
                end++;
                Length++;
            }   
        }

        public klant Dequeue()
        {
            tijdelijk = q_Array[front];
            if (front == q_Array.Length)
                front = 1;
            else
            {
                front++;
                Length--;
            }
            return tijdelijk;
        }
    }

    class Stack
    {
        klant[] stack_array;
        int top;
        int max;
        int Length;
        klant curKlant;

        bool stack_empty()
        {
            if (top == 0)
                return true;
            else
                return false;
        }

        void push(klant currentJob)
        {
            top++;
            stack_array[top] = currentJob;
            Length++;
        }

        klant pop()
        {
            if (stack_empty())
                Console.WriteLine("Stack is leeg");
            else
            {
                top--;
                Length--;
                return stack_array[top + 1];
            }
            return stack_array[top]; // wat moeten we hier nog returnen??
        }
    }
}

// Steeds als je een klant hebt die klaar is bijhouden: is zijn wachttijd groter dan 0?
    
// klant die op 1 binnenkomt
//
