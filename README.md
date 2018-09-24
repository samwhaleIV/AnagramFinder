# AnagramFinder
Main feature: Find anagrams and subanagrams blazingly fast at a cost almost as high as Google Chrome's memory usage.
Recommendations: Sort your alphabet by the letter frequency of your dictionary, your wordset.

```c#

string[] words = File.ReadAllLines("words_alpha.txt");
string alphabet = "etaoinshrdlcumwfgypbvkjxqz";
MappedWords mappedWords = new MappedWords(alphabet,words);

$"Largest word: {mappedWords.GetLargestWord()}";

$"Largest anagram pair: {string.Join(", ",mappedWords.GetLargestAnagramPair())}";

$"Largest anagram set: {string.Join(", ",mappedWords.GetLargestAnagramSet())}";

List<string> possibleWords = mappedWords.GetAnagrams("Anagram");

```
