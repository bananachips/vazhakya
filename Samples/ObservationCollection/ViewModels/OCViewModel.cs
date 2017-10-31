using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//if you have an observable collection bound to itemsource 
//then the source is automatically notified when you add or remove items.
// you do not need to inherit from INotifyPropertyChanged.

// Data grid creates columns of the properties of the item class.
// if the source is a string then length is the only property for it and hence it shows string

namespace ObservationCollection.ViewModels
{
	public class StringItem
	{
		public StringItem(string val) { X = val; Y = val + "hello"; }
		public string X { get; set; }
		public string Y { get; set; }
	}
	public class OCViewModel
	{
		private List<string> _stringList = new List<string>() { "string1", "string2" };
		private ObservableCollection<StringItem> _listOfClowns = new ObservableCollection<StringItem>();
		public string Name { get; set; }
		public ObservableCollection<StringItem> ClownList
		{
			get { return _listOfClowns; }
			set {  _listOfClowns = value; }
		}

		public List<string> StringList
		{
			get { return _stringList; }
			set { _stringList = value; }
		}
		public OCViewModel()
		{
			ClownList.Add(new StringItem("Meera"));
			ClownList.Add(new StringItem("Sachin"));
		}

		public void Add(string name)
		{
			if (Name != null && Name.Length >0)
			{
				ClownList.Add(new StringItem(Name));
			}
			else
			{
				ClownList.Add(new StringItem(name));
			}
		}
	}
}
