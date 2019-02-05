using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MtgExplorator.Models
{
    public class SimpleCard
    {

		public string Name { get; set; }
		public Guid? Id { get; set; }
		public string ImageUri { get; set; }
		public BitmapImage BitmapImage { get { return new BitmapImage(new Uri(ImageUri)); } }
		public bool IsError { get { return Name.Equals("errorCard"); } }

		public SimpleCard(string name, Guid? id, string imageUri)
		{
			Name = name;
			Id = id;
			ImageUri = imageUri;
		}

	}
}
