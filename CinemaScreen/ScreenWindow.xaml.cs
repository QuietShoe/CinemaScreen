using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaScreen
{
    /// <summary>
    /// Interaction logic for Screen.xaml
    /// </summary>
    public partial class ScreenWindow : Window
    {
        private Position ScreenPos = new();
        private float Ratio = 0.8f;

        public ScreenWindow()
        {
            InitializeComponent();
            this.DataContext = ScreenPos;
            this.Topmost = true;
        }

        public void Update(Vector2 screenPos)
        {
            ScreenPos.PosX = String.Format("{0}", screenPos.X);
            ScreenPos.PosY = String.Format("{0}", screenPos.Y);

            this.Left = screenPos.X * Ratio;
            this.Top = screenPos.Y * Ratio;
        }
    }

    class Position : INotifyPropertyChanged
    {
        private string X = "0", Y = "0";
        public string PosX
        {
            get { return X; }
            set { X = value; OnPropertyChanged(); }
        }
        public string PosY
        {
            get { return Y; }
            set { Y = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
