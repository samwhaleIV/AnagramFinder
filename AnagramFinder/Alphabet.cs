using System.Collections.Generic;

namespace AnagramFinder {
	internal sealed class Alphabet {
		internal Alphabet(string letters) {
			indexedLetters=new Dictionary<char,int>();
			for(int i = 0;i<letters.Length;i+=1) {
				indexedLetters.Add(letters[i],i);
			}
		}
		private readonly Dictionary<char,int> indexedLetters;
		internal byte[] GetFrequencyMap(string word) {
			byte[] frequencies = new byte[indexedLetters.Keys.Count];
			foreach(char letter in word) {
				if(indexedLetters.ContainsKey(letter)) {
					frequencies[indexedLetters[letter]]++;
				}
			}
			return frequencies;
		}
	}
}
