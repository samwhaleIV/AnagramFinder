using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramFinder {
	internal sealed class ListOfDoom {
		private ListOfDoom child;
		private string value;
		internal string Value {
			get {
				return value;
			}
		}
		internal ListOfDoom Child {
			get {
				return child;
			}
		}
		internal ListOfDoom(string value) {
			this.value=value;
		}
		private ListOfDoom(string value,ListOfDoom child) {
			this.child=child;
			this.value=value;
		}
		internal void Add(string value) {
			child=new ListOfDoom(this.value,child);
			this.value=value;
		}
		internal List<string> ToList() {
			List<string> list = new List<string>();
			ListOfDoom node = this;
			while(node!=null) {
				list.Add(node.value);
				node=node.child;
			}
			return list;
		}
		internal int GetCount() {
			int count = 1;
			ListOfDoom node = child;
			while(node!=null) {
				count++;
				node=node.child;
			}
			return count;
		}
	}
}
