// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.SampleApp.Data;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FluentEditor.ControlPalette
{
    public sealed partial class ControlPaletteTestContent : UserControl, INotifyPropertyChanged
    {
        public ControlPaletteTestContent()
        {
            this.InitializeComponent();
            this.DataGridDataProvider = new DataGridDataSource();
            _ = this.DataGridDataProvider.GetDataAsync().ContinueWith((result) => 
            {
                if (result.Status == System.Threading.Tasks.TaskStatus.Faulted)
                    throw new System.ApplicationException("Failed to Load Grid Data", result.Exception);

                _ = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {
                    this.DataGridDataView = this.DataGridDataProvider.GroupData().View;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataGridDataView)));
                });
            }) ;
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ControlPaletteTestContent), new PropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return GetValue(TitleProperty) as string; }
            set { SetValue(TitleProperty, value); }
        }

        public DataGridDataSource DataGridDataProvider { get; }

        public ICollectionView DataGridDataView { get; set; }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Windows.UI.Popups.MessageDialog("test", "title");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });

            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                // Adding a 3rd command will crash the app when running on Mobile !!!
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Maybe later") { Id = 2 });
            }

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();

        }
    }
}
