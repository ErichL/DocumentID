using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentID.Lib
{
    public class MinHash
    {
        public MinHash(int numHashFunctions)
        {
            NumHashFunctions = numHashFunctions;
            GenerateHashFunctions();
        }

        public int NumHashFunctions { get; }

        public delegate uint Hash(string toHash);
        
        public Hash[] HashFunctions { get; private set; }

        // TODO: Revise hashing functions
        private void GenerateHashFunctions()
        {
            HashFunctions = new Hash[NumHashFunctions];

            Random r = new Random(10);
            for (int i = 0; i < NumHashFunctions; i++)
            {
                uint a = 0;
                while (a % 1 == 1 || a <= 0)
                    a = (uint)r.Next();
                uint b = 0;
                while (b <= 0)
                    b = (uint)r.Next();
                HashFunctions[i] = s => StringHash(s, a, b);
            }
        }

        // Universal hash function
        private static uint StringHash(string s, uint a, uint b)
        {
            return a * (uint)s.GetHashCode() + b;
        }

        public List<string> GetShingles(string input)
        {
            List<string> shingles = new List<string>();
            string[] stringSeparators = new string[] { "\r\n", "\n", " ", ",", ";" };
            string[] words = input.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                shingles.Add(string.Join(" ", words, i, Math.Min(3, words.Length - i)).ToLower().Trim());
            }

            return shingles;
        }

        // Returns the list of min hashes for the given string
        public List<uint> GetMinHash(List<string> input)
        {
            uint[] minHashes = new uint[NumHashFunctions];
            for (int h = 0; h < NumHashFunctions; h++)
            {
                minHashes[h] = int.MaxValue;
            }
            foreach (string s in input)
            {
                for (int h = 0; h < NumHashFunctions; h++)
                {
                    uint hash = HashFunctions[h](s);
                    minHashes[h] = Math.Min(minHashes[h], hash);
                }
            }
            return minHashes.ToList();
        }

        // Calculates the similarity of two minhashes
        public double Similarity(List<uint> l1, List<uint> l2)
        {
            HashSet<uint> hs1 = new HashSet<uint>(l1);
            HashSet<uint> hs2 = new HashSet<uint>(l2);
            return (double)hs1.Intersect(hs2).Count() / (double)hs1.Union(hs2).Count();
        }
    }
}