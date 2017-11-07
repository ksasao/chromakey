using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using ShaderEffectLibrary;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ChromaKey
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject); // gdi32.dllのDeleteObjectメソッドの使用を宣言する。
        DispatcherTimer dispatcherTimer;
        Bitmap bmp = new Bitmap(400, 720);
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(1000/60));
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            ColorKeyAlphaEffect effect = new ColorKeyAlphaEffect();

            System.Windows.Media.Brush brush = Effect.ImplicitInput;

            effect.Input = brush;
            imageControl.Effect = effect;

            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(new System.Drawing.Point(200, 200), new System.Drawing.Point(0, 0), bmp.Size);
            }
            IntPtr hbitmap = bmp.GetHbitmap();
            imageControl.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hbitmap);
        }
    }
}
