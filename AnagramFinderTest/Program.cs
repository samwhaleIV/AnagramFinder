using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace AnagramFinderTest {
	using static Console;
	internal static class Program {

		private static string[] words;
		private static string alphabet;
		private static AnagramFinder.MappedWords mappedWords;
		private static AnagramFinderRemake.AnagramMap mappedWordsRemake;

		private static void Main() {
			Title="Anagram Finder Test";

			words = File.ReadAllLines("words_alpha.txt");
			alphabet = "etaoinshrdlcumwfgypbvkjxqz";

			mappedWordsRemake = new AnagramFinderRemake.AnagramMap(alphabet,words);

			WriteLine(string.Join(", ",mappedWordsRemake.GetLongestAnagramSet()));

			Start:
			Write("Enter a word to find its sub anagrams: ");
			WriteLine(string.Join(", ",mappedWordsRemake.GetSubAnagrams(ReadLine())));
			goto Start;

		}

		private static void Test() {
			WriteLine(string.Concat(mappedWordsRemake.GetSubAnagrams("abcdefghijklmnopqrstuvwxyz")));
		}

		private static void TestRemake() {
			WriteLine(string.Concat(mappedWordsRemake.GetSubAnagrams("abcdefghijklmnopqrstuvwxyz")));
		}

		//https://stackoverflow.com/a/1622491/3967379
		private static string Benchmark(Action act,int iterations) {
			GC.Collect();
			act.Invoke(); // run once outside of loop to avoid initialization costs
			Stopwatch sw = Stopwatch.StartNew();
			for(int i = 0;i < iterations;i++) {
				act.Invoke();
			}
			sw.Stop();
			return (sw.ElapsedMilliseconds / iterations).ToString();
		}
	}
}
