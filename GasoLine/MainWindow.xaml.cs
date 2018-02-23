using System.ComponentModel;
using System.Windows;

namespace GasoLine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var editableCollectionView = itemsControl.Items as IEditableCollectionView;

            if (!editableCollectionView.CanAddNew)
            {
                MessageBox.Show("You cannot add items to the list.");
                return;
            }

            // Create a window that prompts the user to enter a new
            // item to sell.
            var win = new ChangeItem { DataContext = editableCollectionView.AddNew() };

            //Create a new item to be added to the collection.

            // If the user submits the new item, commit the new
            // object to the collection.  If the user cancels 
            // adding the new item, discard the new item.
            if ((bool)win.ShowDialog())
            {
                editableCollectionView.CommitNew();
            }
            else
            {
                editableCollectionView.CancelNew();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (itemsControl.SelectedItem == null)
            {
                MessageBox.Show("No item is selected");
                return;
            }

            var editableCollectionView =
                itemsControl.Items as IEditableCollectionView;

            // Create a window that prompts the user to edit an item.
            editableCollectionView.EditItem(itemsControl.SelectedItem);
            var win = new ChangeItem { DataContext = itemsControl.SelectedItem };

            // If the user submits the new item, commit the changes.
            // If the user cancels the edits, discard the changes. 
            if ((bool)win.ShowDialog())
            {
                editableCollectionView.CommitEdit();
            }
            else
            {
                editableCollectionView.CancelEdit();
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = itemsControl.SelectedItem as VehicleViewModel;

            if (item == null)
            {
                MessageBox.Show("No Item is selected");
                return;
            }

            var editableCollectionView =
                itemsControl.Items as IEditableCollectionView;

            if (!editableCollectionView.CanRemove)
            {
                MessageBox.Show("You cannot remove items from the list.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to remove " + item.Name,
                "Remove Item", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                editableCollectionView.Remove(itemsControl.SelectedItem);
            }
        }
    }
}
