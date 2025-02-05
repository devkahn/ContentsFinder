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
using CF.Models.DataModels;
using CF.Models.ViewModels;

namespace CF.Views
{
    /// <summary>
    /// ucMain.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucMain : UserControl
    {
        private vmMaterial _Material = null;
        public vmMaterial Material
        {
            get => _Material;
            set
            {
                _Material = value;
                this.DataContext = value;
                this.ucTextModifier.Material = value;
                this.ucMarkdownSetting.Material = value;
            }
        }


        public ucMain()
        {
            InitializeComponent();
            mMaterial newMaterial = new mMaterial();
            this.Material = new vmMaterial(newMaterial);
        }
    }
}
