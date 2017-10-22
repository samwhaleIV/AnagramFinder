using System;
using System.IO;
using AnagramFinder;
using System.Collections.Generic;
using System.Linq;

namespace AnagramFinderTest {
	internal static class Program {
		private static void Main() {
			Console.Title="Anagram Finder Test";
			Console.Write("Mapping English words...");
			string[] words = File.ReadAllLines("words_alpha.txt");
			string alphabet = "etaoinshrdlcumwfgypbvkjxqz";
			MappedWords mappedWords = new MappedWords(alphabet,words);
			Console.Clear();
			Console.WriteLine($"Largest word: {mappedWords.GetLargestWord()}");
			Console.WriteLine($"Largest anagram pair: {string.Join(", ",mappedWords.GetLargestAnagramPair())}");
			Console.WriteLine($"Largest anagram set: {string.Join(", ",mappedWords.GetLargestAnagramSet())}{Environment.NewLine}");
			Start:
			Console.Write("Compared word: ");
			string letters = Console.ReadLine();
			Console.Clear();
			Console.Write($"Finding possible words to create from {letters}...");
			List<string> possibleWords = mappedWords.GetAnagrams(letters);
			possibleWords.Sort((x,y) => y.Length.CompareTo(x.Length));
			string printOut =
				$"There {(possibleWords.Count!=1 ? "are" : "is")} "+
				$"{possibleWords.Count} possible word{(possibleWords.Count!=1?"s":string.Empty)} to create from {(letters.Length!=1? "all the letters of" : "the letter")} "+
				$"{letters}.\n{string.Join(", ",possibleWords)}";
			Console.Clear();
			Console.Write(printOut);
			Console.ReadKey(true);
			Console.Clear();
			goto Start;
		}
	}
}
