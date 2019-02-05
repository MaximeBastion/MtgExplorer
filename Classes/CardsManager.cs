using MtgExplorator.Models;
using Scryfall.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtgExplorator.Classes
{
/// <summary>
/// Classes that manages cards load and save, from local and/or API
/// </summary>
	public class CardsManager
	{

		public static void LoadAllCardsAsync() {
			//LoadCardsAsync(0, StorageManager.NUniqueCards - 1, output);
			LoadCardsAsync(0, 201);
		}

		/// <summary>
		/// Loads the n first cards. For each card, tries to get it from local stroage. If it fails, fetches it from API and saves it in local storage
		/// </summary>
		/// <param name="iStart">Index of the first card id</param>
		/// <param name="iStop">Index of the first card id</param>
		/// <param name="output">Collection of SimpleCards to store loaded cards to</param>
		public static async void LoadCardsAsync(int iStart, int iStop) {
			//int maxI = (n <= StorageManager.Ids.Count) ? n : StorageManager.Ids.Count;

			for (int i = iStart; i < iStop + 1; i++)
			{
				Guid id = StorageManager.Ids[i];
				SimpleCard cardLoaded =  StorageManager.LoadCardLocal(id);
				if (cardLoaded.IsError) {
					Card cardFound = await Task.Run(() => ApiManager.FetchApiAsync(id));
					cardLoaded = cardFound.SimpleCard;
					if (cardLoaded.Name.Equals("Totally Lost")) {
						// ERROR FETCH API
						System.Diagnostics.Debug.WriteLine("Failed to fetch " + cardLoaded.Name);
					} else {
						// API FETCH SUCCESS
						StorageManager.SaveCardLocal(cardLoaded);
						MainPage.CardsLoaded.Add(cardLoaded);
						System.Diagnostics.Debug.WriteLine("Fetched " + cardLoaded.Name + " from API");
					}
				} else {
					// LOCAL LOAD SUCCESS
					MainPage.CardsLoaded.Add(cardLoaded);
					System.Diagnostics.Debug.WriteLine("Fetched " + cardLoaded.Name + " from Local Storage");
				}
			}
			MainPage.OnLoadCardsCompletion();
		}

		/// <summary>
		/// Searches a card in the loaded cards, if it fails, searches via API
		/// </summary>
		/// <param name="cardName"></param>
		/// <param name="cardsLoaded"></param>
		/// <returns></returns>
		public static SimpleCard SearchCardByName(string cardName, ObservableCollection<SimpleCard> cardsLoaded) {
			System.Diagnostics.Debug.WriteLine("Searching card of name " + cardName);
			SimpleCard cardFound = SearchCardByNameInLoaded(cardName, cardsLoaded);
			if (!cardFound.Name.Equals("errorCard")) {
				return cardFound;
			}
			// Search among loaded cards failed
			return SearchCardByNameAPI(cardName);
		}

		public static SimpleCard SearchCardByNameInLoaded(string cardName, ObservableCollection<SimpleCard> cardsLoaded) {
			foreach (SimpleCard e in cardsLoaded)
			{
				if (e.Name.Equals(cardName))
				{
					System.Diagnostics.Debug.WriteLine("Found card in loaded cards");
					return e;
				}
			}
			return StorageManager.errorCard;
		}

		public static SimpleCard SearchCardByNameAPI(string cardName) {
			return ApiManager.SearchCardByName(cardName);
		}



	}
}
