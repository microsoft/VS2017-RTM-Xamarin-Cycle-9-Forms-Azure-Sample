using System;

using MyItems.Models;
using MyItems.ViewModels;

using Xamarin.Forms;

namespace MyItems.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

          
            BindingContext = viewModel = new ItemsViewModel();

            if (Device.OS == TargetPlatform.Windows)
            {
                ToolbarAdd.Icon = "plus.png";
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon = "refresh.png",
                    Command = viewModel.LoadItemsCommand
                });
            }

			if (Device.OS != TargetPlatform.Windows || Device.OS != TargetPlatform.WinPhone)
				UwpActivityIndicator.IsVisible = false;

        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
