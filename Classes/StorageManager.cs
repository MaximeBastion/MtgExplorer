using MtgExplorator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MtgExplorator.Classes
{
	class StorageManager
	{

		private static List<Guid> _ids = new List<Guid>();
		public static List<Guid> Ids { get { return _ids; } set { _ids = value; } }

		public static SimpleCard errorCard;

		public static int NUniqueCards { get { return Ids.Count; } }

		public static void Init() {
			InitIds();
			errorCard = new SimpleCard("errorCard", new Guid(), "");
		}

		public static void InitIds()
		{
			string[] lines = System.IO.File.ReadAllLines("scryfallIds.txt");
			foreach (string line in lines)
			{
				if (line != string.Empty)
				{
					Ids.Add(new Guid(line));
				}
			}
		}

		public static ObservableCollection<SimpleCard> LoadAllCardsLocal() {
			ObservableCollection<SimpleCard> cardsLoaded = new ObservableCollection<SimpleCard>();
			foreach (Guid id in Ids)
			{
				SimpleCard cardLoaded = LoadCardLocal(id);
				if (!cardLoaded.Name.Equals("errorCard")) {
					cardsLoaded.Add(LoadCardLocal(id));
				}
			}
			return cardsLoaded;
		}

		public static SimpleCard LoadCardLocal(Guid id) {
			ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
			if (!localSettings.Values.ContainsKey(id + ".name") || 
			!localSettings.Values.ContainsKey(id + ".image")) {
				// HAS NOT BEEN SAVED LOCALLY
				return errorCard;
			}
			string name = localSettings.Values[id + ".name"].ToString(); //HERE
			string imageUri = localSettings.Values[id + ".image"].ToString();
			if (name != null && imageUri != null)
			{
				return new SimpleCard(name, id, imageUri);
			}
			return errorCard;
		}

		public static void SaveCardLocal(SimpleCard simpleCard) {
			ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
			localSettings.Values[simpleCard.Id + ".id"] = simpleCard.Id;
			localSettings.Values[simpleCard.Id + ".name"] = simpleCard.Name;
			localSettings.Values[simpleCard.Id + ".image"] = simpleCard.ImageUri;
		}

	}
}
