using System.Windows;
using System.Windows.Controls;

namespace NursesScheduler.WPF.Controls
{
    /// <summary>
    /// Interaction logic for BulletedItem.xaml
    /// </summary>
    public partial class BulletedItem : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("BulletText", typeof(string), typeof(BulletedItem));

        public string BulletText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public BulletedItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
