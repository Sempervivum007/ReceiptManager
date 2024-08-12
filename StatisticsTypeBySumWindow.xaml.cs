using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows;

namespace ReceiptManager
{
    public partial class StatisticsTypeBySumWindow : Window
    {
        private readonly List<StatModel> dataForStat;

        public PlotModel TypeBySumModel { get; set; }

        public StatisticsTypeBySumWindow(List<StatModel> dataForStat)
        {
            InitializeComponent();
            this.dataForStat = dataForStat;

            DataContext = this;

            InitializePlot();
            LoadData();
        }

        private void InitializePlot()
        {
            TypeBySumModel = new PlotModel { Title = "Статистика по Типам за сумами" };
        }

        private void LoadData()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            foreach (var item in dataForStat)
            {
                pieSeries.Slices.Add(new PieSlice(item.Type.ToString(), (double)item.TotalAmount));
            }

            TypeBySumModel.Series.Add(pieSeries);
        }
    }
}