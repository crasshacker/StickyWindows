using System.Windows;
using System.ComponentModel;
using StickyWindows;
using StickyWindows.WPF;

namespace WpfTest {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
            Loaded += window1Loaded;
        }

        private void window1Loaded(object sender, RoutedEventArgs e) {
            var sw = this.CreateStickyWindow(StickyWindowType.Anchor);
        }


        private void newWindowButton_Click(object sender, RoutedEventArgs e) {
            var win2 = new Window2();
            win2.Show();
        }

        protected override void OnClosing(CancelEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}
