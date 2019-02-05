using MtgExplorator.Classes;
using MtgExplorator.Models;
using Scryfall.API;
using Scryfall.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MtgExplorator
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

		public ScryfallClient scryfall;
		public Card errorCard;
	

		private ObservableCollection<SimpleCard> _cardsDisplayed = new ObservableCollection<SimpleCard>();
		public ObservableCollection<SimpleCard> CardsDisplayed { get { return _cardsDisplayed; } set { _cardsDisplayed = value; } }

		public static ObservableCollection<SimpleCard> CardsLoaded { get; set; } = new ObservableCollection<SimpleCard>();

		public ObservableCollection<string> CardsNames 
		{ 
		get {
				ObservableCollection<string> output = new ObservableCollection<string>();
				foreach (SimpleCard e in CardsLoaded)
				{
					output.Add(e.Name);
				}
				return output;
			} 
		}

		public static int NCardsLoaded { get { return CardsLoaded.Count; } }

		public string TitleText { get { return TitleText; } set { contentTitle.Text = value; } }

		public static string LoadingCardsStateText 
		{
			get {return "Loaded cards : \n" + CardsLoaded.Count + "/" + StorageManager.NUniqueCards;  }
		}


		public MainPage()
        {
            this.InitializeComponent();

			// Instiantiate new Scryfall client
			scryfall = new ScryfallClient();

			errorCard = scryfall.Cards.Search("totallyLost").Data[0];


			// Get a random card
			/*var card = scryfall.Cards.GetRandom();
			Console.WriteLine($"{card.Name}\t\t{card.ManaCost}\n");
			Console.WriteLine($"{card.TypeLine}\n");
			Console.WriteLine($"{card.OracleText}");
			testText.Text = card.Name; */

			// Search
			/*
			var card = scryfall.Cards.Search("tarmogoyf").Data[0];
			testText.Text = card.Name;
			image.Source = card.BitmapImage;*/
			DataContext = this;
			/*
			Card card = scryfall.Cards.GetById(new Guid("05f0b6ce-eb70-4f42-9360-c7d09f48a5c5"));
			contentTitle.Text = card.Name;*/
			StorageManager.Init();
			ApiManager.Init();
			CardsManager.LoadCardsAsync(464, 466);
			//CardsManager.LoadAllCardsAsync(CardsDisplayed);
			//CardsDisplayed = StorageManager.LoadAllCardsLocal();
			System.Diagnostics.Debug.WriteLine("Init completed");
			loadingCardsStateText.Text = LoadingCardsStateText;
		}

		public static void OnLoadCardsCompletion()
		{
			System.Diagnostics.Debug.WriteLine("Finished to load cards.");
			System.Diagnostics.Debug.WriteLine(LoadingCardsStateText);
		}

		/// <summary>
		/// Called when the user submits a research. Searches in loaded cards if user chose a suggested elements, otherwise searches with the API
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			string userInput = args.QueryText;
			bool userChoseASuggestedElement = (args.ChosenSuggestion != null) ? true : false;
			SimpleCard cardFound = StorageManager.errorCard;

			// From loaded
			if (userChoseASuggestedElement) {
				DisplayAsFocus(CardsManager.SearchCardByNameInLoaded(userInput, CardsLoaded));
				return;
			}

			// From API 
			cardFound = CardsManager.SearchCardByNameAPI(userInput);
			if (!cardFound.IsError) {
				CardsLoaded.Add(cardFound);
				DisplayAsFocus(cardFound);
				System.Diagnostics.Debug.WriteLine("Search succeeded from API");
				return;
			}
			System.Diagnostics.Debug.WriteLine("Search failed");
		}

		private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			loadingCardsStateText.Text = LoadingCardsStateText;
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
				sender.ItemsSource = CardsNames;
		  }
		}

		public void DisplayAsFocus(SimpleCard simpleCard) {
			System.Diagnostics.Debug.WriteLine("Displaying as focus card");
			CardsDisplayed.Clear();
			CardsDisplayed.Add(simpleCard);
			TitleText = simpleCard.Name;
			contentTitle.Text = simpleCard.Name;
		}

	}
}
