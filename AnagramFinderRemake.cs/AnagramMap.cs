using System;
using System.Collections.Generic;

namespace AnagramFinderRemake {
	internal sealed class LOD {
		private LOD child;
		private byte[] value;
		internal byte[] Value {
			get {
				return value;
			}
		}
		internal LOD Child {
			get {
				return child;
			}
		}
		internal LOD(byte[] value) {
			this.value=value;
		}
		private LOD(byte[] value,LOD child) {
			this.child=child;
			this.value=value;
		}
		internal void Add(byte[] value) {
			child=new LOD(this.value,child);
			this.value=value;
		}
		internal IEnumerable<byte[]> Enumerate() {
			LOD node = this;
			while(node!=null) {
				yield return node.value;
				node=node.child;
			}
		}
		internal int GetCount() {
			int count = 1;
			LOD node = child;
			while(node!=null) {
				count++;
				node=node.child;
			}
			return count;
		}
	}
	public sealed class AnagramMap {

		private sealed class ByteArrayComparer:IEqualityComparer<byte[]> {
			public bool Equals(byte[] x,byte[] y) {
				if(x.Length != y.Length) {
					return false;
				}
				for(byte i = 0;i<x.Length;i++) {
					if(x[i] != y[i]) {
						return false;
					}
				}
				return true;
			}
			public int GetHashCode(byte[] obj) {
				int hashCode = 17;
				for(byte i = 0;i < obj.Length;i++) {
					unchecked {
						hashCode = hashCode * 23 + obj[i];
					}
				}
				return hashCode;
			}
		}

		//This is the key field used in GetAlphabetMap and GetWord
		private Dictionary<char,byte> alphabet;
		private Dictionary<byte,char> alphabetInverse;

		//This method is a string replacement, not a frequency map!
		private byte[] GetAlphabetMap(string word) {
			var alphabetMap = new byte[word.Length];
			for(byte characterIndex = 0;characterIndex<word.Length;characterIndex++) {
				char letter = word[characterIndex];
				if(alphabet.ContainsKey(letter)) {
					alphabetMap[characterIndex] = alphabet[letter];
				}
			}
			return alphabetMap;
		}
		
		//Used in conjunction with GetAlphabetMap, not freqeuency mapping!
		private string GetWord(byte[] alphabetMap) {
			var characterMap = new char[alphabetMap.Length];
			for(byte alphabetMapIndex = 0;alphabetMapIndex<alphabetMap.Length;alphabetMapIndex++) {
				characterMap[alphabetMapIndex] = alphabetInverse[alphabetMap[alphabetMapIndex]];
			}
			return string.Concat(characterMap);
		}

		private byte[] GetFrequencyMap(string word) {
			byte[] frequencies = new byte[alphabet.Count];
			foreach(char letter in word) {
				if(alphabet.ContainsKey(letter)) {
					frequencies[alphabet[letter]]++;
				}
			}
			return frequencies;
		}

		private byte[] GetFrequencyMap(byte[] alphabetMap) {
			byte[] frequencyMap = new byte[alphabetInverse.Count];
			foreach(byte letter in alphabetMap) {
				if(alphabetInverse.ContainsKey(letter)) {
					frequencyMap[letter]++;
				}
			}
			return frequencyMap;
		}

		private Dictionary<byte[],LOD> map;

		//Filter words to lower case before instantiating
		public AnagramMap(string alphabet,IEnumerable<string> words) {

			this.alphabet =        new Dictionary<char,byte>();
			this.alphabetInverse = new Dictionary<byte,char>();

			for(byte alphabetIndex = 0;alphabetIndex<alphabet.Length;alphabetIndex++) {
				this.alphabet.Add(
					alphabet[alphabetIndex],
					alphabetIndex
				);
				this.alphabetInverse.Add(
					alphabetIndex,
					alphabet[alphabetIndex]
				);
			}
			map = new Dictionary<byte[],LOD>(
				new ByteArrayComparer()
			);
			foreach(var word in words) {
				var alphabetMap = GetAlphabetMap(word);
				var frequencyMap = GetFrequencyMap(alphabetMap);
				if(map.ContainsKey(frequencyMap)) {
					map[frequencyMap].Add(alphabetMap);
				} else {
					map[frequencyMap] = new LOD(alphabetMap);
				}
			}
		}

		private static bool GreaterOrEqual(byte[] frequencyMap1,byte[] frequencyMap2) {
			for(byte frequencyMapIndex = 0;frequencyMapIndex<frequencyMap1.Length;frequencyMapIndex++) {
				if(frequencyMap1[frequencyMapIndex]<frequencyMap2[frequencyMapIndex]) {
					return false;
				}
			}
			return true;
		}

#region Public methods
		//No need to filter characters out of word for these methods, just ensure that they are in lower case

		public IEnumerable<string> GetAnagrams(string word) {
			var key = GetFrequencyMap(word);
			if(map.ContainsKey(key)) {
				foreach(var alphabetMap in map[key].Enumerate()) {
					yield return GetWord(alphabetMap);
				}
			}
		}

		public bool HasAnagram(string word) {
			if(map.ContainsKey(GetFrequencyMap(word))) {
				return true;
			} else {
				return false;
			}
		}
		public IEnumerable<string> GetSubAnagrams(string word) {
			var frequencyMap = GetFrequencyMap(word);
			foreach(var key in map.Keys) {
				if(GreaterOrEqual(frequencyMap,key)) {
					foreach(var value in map[key].Enumerate()) {
						yield return GetWord(value);
					}
				}
			}
		}
		public string GetLargestWord() {
			byte[] largestWord = null;
			int largestKeyTotal = 0;
			foreach(var value in map.Values) {
				var wordLength = value.Value.Length;
				if(wordLength >= largestKeyTotal) {
					largestWord = value.Value;
					largestKeyTotal = wordLength;
				}
			}
			if(largestWord != null) {
				return GetWord(largestWord);
			} else {
				return null;
			}
		}

		public IEnumerable<string> GetLargestAnagramSet() {
			List<byte[]> bucket = new List<byte[]>();
			foreach(var value in map.Values) {
				if(value.GetCount()>bucket.Count) {
					bucket=(List<byte[]>)value.Enumerate();
				}
			}
			foreach(var item in bucket) {
				yield return GetWord(item);
			}
		}

		public IEnumerable<string> GetLongestAnagramSet() {
			List<byte[]> largestSet = null;
			var largestLength = 0;
			foreach(var value in map.Values) {
				if(value.GetCount()>1) {
					var length = value.Value.Length;
					if(length > largestLength) {
						largestLength = length;
						largestSet = (List<byte[]>)value.Enumerate();
					}
				}
			}
			if(largestSet != null) {
				foreach(var item in largestSet) {
					yield return GetWord(item);
				}
			}
		}
		#endregion
	}
}
