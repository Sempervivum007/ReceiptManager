using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ReceiptManager
{
    public partial class MainWindow : Window
    {
        private readonly JsonDatabase _database;

        public List<Receipt> _receipts;

        public MainWindow()
        {
            InitializeComponent();

            _database = new JsonDatabase("receipts.json");

            _receipts = _database.ReadReceipts();

            ShowReceipts_Click(null, null);
        }

        private void AddReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            AddReceiptWindow addReceiptWindow = new AddReceiptWindow(_database, null);
            addReceiptWindow.ShowDialog();

            ShowReceipts_Click(null, null);
        }

        private void ShowReceipts_Click(object sender, RoutedEventArgs e)
        {
            _receipts = _database.ReadReceipts();

            ReceiptsDataGrid.ItemsSource = _receipts;

            var totalAmountField = _receipts.Sum(r => r.Amount);
            totalAmountTextBlock.Text = $"Сума: {totalAmountField:C}.  Кількість: {_receipts.Count} ";
        }

        private void DeleteReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsDataGrid.SelectedItem != null &&
                ReceiptsDataGrid.SelectedItem is Receipt selectedReceipt)
            {
                try
                {
                    var result = MessageBox.Show("Видалити обраний чек?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        _receipts.Remove(selectedReceipt);
                        _database.SaveReceipts(_receipts);

                        MessageBox.Show("Чек успішно Видалено", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowReceipts_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Скасовано", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Не вдалося видалити чек", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsDataGrid.SelectedItem is Receipt selectedReceipt)
            {
                try
                {
                    var result = MessageBox.Show("Редагувати обраний чек?", "Підтвердження редагування", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        AddReceiptWindow addReceiptWindow = new AddReceiptWindow(_database, selectedReceipt);
                        addReceiptWindow.ShowDialog();

                        ShowReceipts_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Редагування скасовано", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Не вдалося змінити чек", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GetStatistics_Click(object sender, RoutedEventArgs e)
        {
            GetStatisticsWindow getStatisticsWindow = new GetStatisticsWindow(_database);
            getStatisticsWindow.ShowDialog();

            List<Receipt> filteredReceipts = getStatisticsWindow.FilteredReceipt;

            if (filteredReceipts != null)
            {
                ReceiptsDataGrid.ItemsSource = filteredReceipts;

                var totalAmountField = filteredReceipts.Sum(r => r.Amount);
                totalAmountTextBlock.Text = $"Сума: {totalAmountField:C}.  Кількість: {filteredReceipts.Count} ";

               

                if (getStatisticsWindow.IsSumByDate)
                {
                    filteredReceipts = filteredReceipts.OrderBy(x => x.Date).ToList();
                    StatisticsSumByDateWindow statisticsSumByDate = new StatisticsSumByDateWindow(filteredReceipts);
                    statisticsSumByDate.ShowDialog();
                }
                else
                {
                    List<StatModel> dataForStat =
                        filteredReceipts.GroupBy(r => r.Type)
                        .Select(g => new StatModel
                        {
                            Type = g.Key,
                            Count = g.Count(),
                            TotalAmount = g.Sum(r => r.Amount)
                        }).ToList();

                    if (getStatisticsWindow.IsTypeBySum)
                    {
                        StatisticsTypeBySumWindow statisticsTypeBySum = new StatisticsTypeBySumWindow(dataForStat);
                        statisticsTypeBySum.ShowDialog();
                    }
                    else if (getStatisticsWindow.IsTypeByAmount)
                    {
                        StatisticsTypeByCountWindow statisticsTypeByCount = new StatisticsTypeByCountWindow(dataForStat);
                        statisticsTypeByCount.ShowDialog();
                    }
                }
            }
        }
    }
}