using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentID.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentID.Lib.Tests
{
    [TestClass()]
    public class MinHashTests
    {

        [TestMethod()]
        public void GetShinglesTest()
        {
            MinHash minHash = new MinHash(10);
            string input = "Hello world I am a string";
            List<string> result = minHash.GetShingles(input);
            List<string> expected = new List<string>() { "hello world i", "world i am", "i am a", "am a string", "a string", "string" };
            Assert.AreEqual(string.Join(" ", expected), string.Join(" ", result));
        }

        [TestMethod()]
        public void GetMinHashTest_ForSameInputs()
        {
            MinHash minHash = new MinHash(10);
            List<string> inputOne = new List<string>() { "hello world i", "world i am", "i am a", "am a string", "a string", "string" };
            List<string> inputTwo = new List<string>() { "hello world i", "world i am", "i am a", "am a string", "a string", "string" };
            List<uint> result = minHash.GetMinHash(inputOne);
            List<uint> expected = minHash.GetMinHash(inputTwo);
            Assert.AreEqual(string.Join(" ", expected), string.Join(" ", result));
        }

        [TestMethod()]
        public void GetMinHashTest_ForDifferentInputs()
        {
            MinHash minHash = new MinHash(10);
            List<string> inputOne = new List<string>() { "hello world i", "world i am", "i am a", "am a string", "a string", "string" };
            List<string> inputTwo = new List<string>() { "not the same", "the same", "same" };
            List<uint> result = minHash.GetMinHash(inputOne);
            List<uint> expected = minHash.GetMinHash(inputTwo);
            Assert.AreNotEqual(string.Join(" ", expected), string.Join(" ", result));
        }

        [TestMethod()]
        public void SimilarityTest()
        {
            MinHash minHash = new MinHash(10);
            List<uint> firstInput = new List<uint>() { 1, 2, 3 };
            List<uint> secondInput = new List<uint>() { 1, 2, 3 };
            double result = minHash.Similarity(firstInput, secondInput);
            double expected = 1.0;
            Assert.AreEqual(expected, result);
        }
    }
}