using Scryfall.API;
using Scryfall.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using MtgExplorator.Models;

namespace MtgExplorator.Classes
{
	public class ApiManager
	{

		public static ScryfallClient scryfall;
		public static Card errorCard;

		public static void Init()
		{
			scryfall = new ScryfallClient();
			errorCard = scryfall.Cards.GetById(new Guid("05f0b6ce-eb70-4f42-9360-c7d09f48a5c5"));
		}

		public static async Task<Card> FetchApiAsync(Guid id)
		{
			await Task.Delay(100); // To respect API's policy, make 10 requests/s max
			Card cardFound = await Task.Run(() => FetchCardAPI(id));
			return cardFound;
		}

		public static Card FetchCardAPI(Guid id)
		{
			Card cardFound = scryfall.Cards.GetById(id);
			if (cardFound == null)
			{
				return errorCard;
			}
			return cardFound;
		}

		public static SimpleCard SearchCardByName(string cardName)
		{
			SimpleCard cardFound = errorCard.SimpleCard;
			try
			{
				var cardList = scryfall.Cards.Search(cardName);
				if (cardList != null && cardList.Data != null && cardList.Data.Count > 0) {
					cardFound = cardList.Data[0].SimpleCard;
					if (!cardFound.IsError) {
						StorageManager.SaveCardLocal(cardFound);
						MainPage.CardsLoaded.Add(cardFound);
						return cardFound;
					}
				}
				
			}
			finally
			{ 
			}
			return cardFound;
		}
	}
}
