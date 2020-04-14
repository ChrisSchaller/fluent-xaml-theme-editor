// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Toolkit.Uwp.SampleApp.Data;
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
    }
}
