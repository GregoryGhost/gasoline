using System;
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
            var editableCollectionView = 
                itemsControl.Items as IEditableCollectionView;

            if (!editableCollectionView.CanAddNew)
            {
                MessageBox.Show("Вы не можете добавить запись в список.");
                return;
            }

            // Create a window that prompts the user to enter a new
            // item to sell.
            var win = new ChangeItem {
                DataContext = editableCollectionView.AddNew() };

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
                MessageBox.Show("Выделите запись.");
                return;
            }

            var editableCollectionView =
                itemsControl.Items as IEditableCollectionView;

            // Create a window that prompts the user to edit an item.
            editableCollectionView.EditItem(itemsControl.SelectedItem);
            var win = new ChangeItem {
                DataContext = itemsControl.SelectedItem };

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
                MessageBox.Show("Выделите запись.");
                return;
            }

            var editableCollectionView =
                itemsControl.Items as IEditableCollectionView;

            if (!editableCollectionView.CanRemove)
            {
                MessageBox.Show("Вы не можете удалить запись из списка.");
                return;
            }

            var res = MessageBox.Show(
                "Вы уверены что хотите удалить запись? " + item.Name,
                "Remove Item",
                MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                editableCollectionView.Remove(itemsControl.SelectedItem);
            }
        }

        private void SaveItems_Click(object sender, RoutedEventArgs e)
        {
            var t = (Vehicles)this.Resources[nameof(Vehicles)];
            if (t == null)
            {
                MessageBox.Show(
                    "Ошибка подключения к БД",
                    "Ошибка записи",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                _saveData = false;
            }
            else
            {
                if (_path == string.Empty)
                {
                    _path = _defaultPath;
                }

                if(CheckEmptyRecords(t)) { return; }

                if (t.Save(_path))
                {
                    MessageBox.Show(
                        $"Записи были успешно записаны в файл {_path}",
                        "Сохранение",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    _saveData = true;
                }
                else
                {
                    _saveData = false;
                    MessageBox.Show(
                        $"Записи БД не были сохранены в файл {_path}.\n" +
                        $"Записи содержать неверные данные.",
                        "Ошибка записи",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

        }

        private bool CheckEmptyRecords(Vehicles t)
        {
            var result = false;

            if (t.Count == 0)
            {
                MessageBox.Show(
                    $"Отсутствуют записи для сохранения",
                    "Сохранение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);

                _saveData = true;

                result = true;
            }

            return result;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var t = (Vehicles)this.Resources[nameof(Vehicles)];

            if (_saveData == false)
            {
                var r = MessageBox.Show(
                    "Данные не были сохранены.\n" +
                    "Сохранить ?",
                    "Открытие нового файла",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                switch (r)
                {
                    case MessageBoxResult.Yes:
                        SaveItems_Click(sender, e);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }

            t.Clear();

            if (t == null)
            {
                MessageBox.Show(
                    "Ошибка подключения к БД",
                    "Ошибка открытия",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                _saveData = false;
            }
            else
            {
                var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    FileName = "Document",
                    DefaultExt = ".json",
                    Filter = "Text documents (.json)|*.json"
                };

                var result = dlg.ShowDialog();
                if (result == true)
                {
                    _path = dlg.FileName;
                }

                if (_path == string.Empty)
                {
                    _path = _defaultPath;
                }

                if (t.Open(_path))
                {
                    MessageBox.Show(
                        $"Записи были успешно загружены из файла {_path}",
                        "Загрузка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Записи БД не были загружены из файла {_path}.\n" +
                        $"Записи содержать неверные данные.",
                        "Ошибка загрузки",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        string _path = string.Empty;
        readonly string _defaultPath = "test.json";
        bool _saveData = false;
        private void New_Click(object sender, RoutedEventArgs e)
        {
            var t = (Vehicles)this.Resources[nameof(Vehicles)];

            if (_saveData == false)
            {
                var r = MessageBox.Show(
                    "Данные не были сохранены.\n" +
                    "Сохранить ?",
                    "Создание нового файла",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                switch (r)
                {
                    case MessageBoxResult.Yes:
                        SaveItems_Click(sender, e);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }

            t.Clear();

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Создать новый файл",
                FileName = "Document",
                DefaultExt = ".json",
                Filter = "Text documents (.json)|*.json"
            };

            var result = dlg.ShowDialog();

            if (result == true)
            {
                _path = dlg.FileName;
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var t = (Vehicles)this.Resources[nameof(Vehicles)];
            if (CheckEmptyRecords(t)) { return; }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Document",
                DefaultExt = ".json",
                Filter = "Text documents (.json)|*.json"
            };

            var result = dlg.ShowDialog();

            if (result == true)
            {
                _path = dlg.FileName;
            }

            SaveItems_Click(sender, e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var t = (Vehicles)this.Resources[nameof(Vehicles)];
            if (t.Count == 0) { return; }

            var result = MessageBox.Show(
                "Вы уверены что хотите выйти?\n" +
                "Есть несохраненные данные, сохранить их?",
                "Закрытие программы",
                MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (_path == string.Empty)
                    {
                        SaveAs_Click(sender, e);
                    }
                    else
                    {
                        SaveItems_Click(sender, e);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    return;
            }
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Exit_Click(sender, null);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var link = "https://github.com/GregoryGhost/gasoline";
            var result = MessageBox.Show(
                $"Разработчик: Кулаков Григорий.\n" +
                $"Репозиторий проекта: {link}\n" +
                $"Перейти на страницу проекта?",
                "О программе", MessageBoxButton.OKCancel,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                System.Diagnostics.Process.Start(link);
            }
        }
    }
}
