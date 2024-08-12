using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows;

namespace ReceiptManager
{
    public partial class StatisticsSumByDateWindow : Window
    {
        private readonly List<Receipt> receipts;

        public PlotModel SumByDateModel { get; set; }
        public StatisticsSumByDateWindow(List<Receipt> receipts)
        {
            InitializeComponent();
            this.receipts = receipts;

            DataContext = this;

            InitializePlot();
            LoadData();
        }

        private void InitializePlot()
        {
            SumByDateModel = new PlotModel { Title = "Статистика витрат за період" };
            SumByDateModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "MMM yyyy" });
            SumByDateModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Сума (грн)" });
        }

        private void LoadData()
        {
            var series = new LineSeries { Title = "Витрати", MarkerType = MarkerType.Circle };

            foreach (var item in receipts)
            {
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), (double)item.Amount));
            }

            SumByDateModel.Series.Add(series);
            SumByDateModel.InvalidatePlot(true);
        }
    }
}