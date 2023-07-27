using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YGOSmallWorldSearcher
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly int OUTPUT_SIZE = 20;

		private SmallWorldSearcher SWSearcher;
		private CardData WaypointFrom { get; set; }
		private CardData WaypointTo { get; set; }
		private List<CardData> WaypointList { get; set; }
		
		private List<Grid> CardDataFramePool { get; set; }

		public MainWindow()
		{
			SWSearcher = new();

			CardDataFramePool = new();
			FramePoolInitialize();
			InitializeComponent();
		}

		public void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ClearValue(SizeToContentProperty);
			MainGrid.ClearValue(WidthProperty);
			MainGrid.ClearValue(HeightProperty);
		}

		private void CardNameFrom_GotFocus(object sender, RoutedEventArgs e)
		{
			CardNameFrom.Dispatcher.BeginInvoke(new Action(() => { CardNameFrom.Select(CardNameFrom.Text.Length, 1); }));
			CardNameFromPopup.IsOpen = true;
		}

		private void CardNameTo_GotFocus(object sender, RoutedEventArgs e)
		{
			CardNameTo.Dispatcher.BeginInvoke(new Action(() => { CardNameTo.Select(CardNameTo.Text.Length, 1); }));
			CardNameToPopup.IsOpen = true;
		}

		private void CardNameFrom_LostFocus(object sender, RoutedEventArgs e)
		{
			CardNameFromPopup.IsOpen = false;
		}

		private void CardNameTo_LostFocus(object sender, RoutedEventArgs e)
		{
			CardNameToPopup.IsOpen = false;
		}

		private void CardNameFrom_TextChanged(object sender, TextChangedEventArgs e)
		{
			CardNameFromList.Items.Clear();
			if(CardNameFrom.Text != "")
			{
				var searchResult = SWSearcher.CardNameSearch(CardNameFrom.Text);

				int listSize = Math.Min(10, searchResult.Count);
				for(int i = 0; i < listSize; ++i)
					CardNameFromList.Items.Add(searchResult[i]);
			}
		}

		private void CardNameTo_TextChanged(object sender, TextChangedEventArgs e)
		{
			CardNameToList.Items.Clear();
			if(CardNameTo.Text != "")
			{
				var searchResult = SWSearcher.CardNameSearch(CardNameTo.Text);

				int listSize = Math.Min(10, searchResult.Count);
				for(int i = 0; i < listSize; ++i)
					CardNameToList.Items.Add(searchResult[i]);
			}
		}

		private void CardNameFromList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if((sender as ListBox).HasItems)
				CardNameFrom.Text = (sender as ListBox).SelectedValue.ToString();
		}

		private void CardNameToList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if((sender as ListBox).HasItems)
				CardNameTo.Text = (sender as ListBox).SelectedValue.ToString();
		}

		private void WaypointButton_Click(object sender, RoutedEventArgs e)
		{
			string name1 = CardNameFrom.Text;
			string name2 = CardNameTo.Text;
			WaypointFrom = SWSearcher.GetCardData(name1);
			WaypointTo = SWSearcher.GetCardData(name2);
			WaypointList = SWSearcher.GetWaypoints(name1, name2);
			PrintCardData(0);
		}

		private void FramePoolInitialize()
		{
			for(int i = 0; i < OUTPUT_SIZE; ++i)
			{
				Grid grid = new()
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					Width = 270,
					Height = 500
				};
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(390) });
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

				Image cardImage = new Image()
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Label label = new Label()
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};

				grid.Children.Add(cardImage);
				grid.Children.Add(label);

				CardDataFramePool.Add(grid);
			}
		}

		private void PrintCardData(int page)
		{
			
		}

		
	}

}
