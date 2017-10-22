using System.Collections.Generic;

namespace AnagramFinder {
	public sealed class MappedWords {
		private struct ListAndThenSome {
			internal readonly byte[] Frequencies;
			internal readonly ListOfDoom Bucket;
			internal ListAndThenSome(string word,byte[] frequencies) {
				Bucket=new ListOfDoom(word);
				Frequencies=frequencies;
			}
		}
		private readonly Dictionary<string,ListAndThenSome> wordMap;
		private readonly Alphabet alphabet;
		public MappedWords(string alphabet,IEnumerable<string> words) {
			this.alphabet=new Alphabet(alphabet);
			wordMap=new Dictionary<string,ListAndThenSome>();
			foreach(string word in words) {
				byte[] frequencies = this.alphabet.GetFrequencyMap(word);
				var key = string.Concat(frequencies);
				if(wordMap.ContainsKey(key)) {
					wordMap[key].Bucket.Add(word);
				} else {
					wordMap.Add(key,new ListAndThenSome(word,frequencies));
				}
			}
		}
		private static bool GreaterOrEqual(byte[] frequencyMap1,byte[] frequencyMap2) {
			for(int i = 0;i<frequencyMap1.Length;i++) {
				if(frequencyMap1[i]<frequencyMap2[i]) {
					return false;
				}
			}
			return true;
		}
		public List<string> GetAnagrams(string word) {
			List<string> anagrams = new List<string>();
			var key = string.Concat(alphabet.GetFrequencyMap(word));
			if(wordMap.ContainsKey(key)) {
				return wordMap[key].Bucket.ToList();
			} else {
				return anagrams;
			}
		}
		public bool HasAnagram(string word) {
			if(wordMap.ContainsKey(
					string.Concat(
						alphabet.GetFrequencyMap(
							word
						)
					)
				)
			) {
				return true;
			} else {
				return false;
			}
		}
		public List<string> GetSubAnagrams(string word) {
			List<string> subAnagrams = new List<string>();
			var frequencies = alphabet.GetFrequencyMap(word);
			foreach(ListAndThenSome value in wordMap.Values) {
				if(GreaterOrEqual(frequencies,value.Frequencies)) {
					subAnagrams.AddRange(value.Bucket.ToList());
				}
			}
			return subAnagrams;
		}
		public string GetLargestWord() {
			string largestWord = string.Empty;
			foreach(ListAndThenSome value in wordMap.Values) {
				string firstValue = value.Bucket.Value;
				if(firstValue.Length>=largestWord.Length) {
					largestWord=firstValue;
				}
			}
			return largestWord;
		}
		public List<string> GetLargestAnagramSet() {
			List<string> bucket = new List<string>();
			foreach(ListAndThenSome value in wordMap.Values) {
				if(value.Bucket.GetCount()>bucket.Count) {
					bucket=value.Bucket.ToList();
				}
			}
			return bucket;
		}
		public string[] GetLargestAnagramPair() {
			string[] largestPair = new string[2] {
				string.Empty,string.Empty
			};
			foreach(ListAndThenSome value in wordMap.Values) {
				if(
					value.Bucket.GetCount()>1 &&
					value.Bucket.Value.Length>largestPair[0].Length
				) {
					largestPair[0]=value.Bucket.Value;
					largestPair[1]=value.Bucket.Child.Value;
				}
			}
			return largestPair;
		}
	}
}
