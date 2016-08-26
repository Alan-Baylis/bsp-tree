using BspTree.Base;
using BspTree.Construct;
using BspTree.Import;
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

namespace BspTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Tree> _trees;
        public MainWindow()
        {
            InitializeComponent();
            this._trees = new List<Tree>();

            var polyhedra = ImportBuilder
                .Create(new[]
                {
                    new
                    {
                        points = new List<PointImport>(),
                        planes = new List<string[]>()
                    }
                })
                .ReadFrom("BspTree.example.json")
                .Embedded()
                .Read();

            foreach (var item in polyhedra)
            {
                var polyhedronPlanes = new List<Plane>();

                foreach (var plane in item.planes)
                {
                    polyhedronPlanes.Add(new Plane
                    {
                        Points = item.points.Where(x => plane.Contains(x.Label)).ToList()
                    });
                }

                var treeBuilder = new TreeBuilder(polyhedronPlanes);
                this._trees.Add(treeBuilder.Construct());
            }

            this.Draw();
        }

        private void Draw()
        {
            this.grid.Children.Clear();

            foreach (var item in this._trees)
            {
                this.Draw(item);
            }
        }

        private void Draw(Tree tree)
        {
            if (tree == null)
                return;

            this.Draw(tree.Left);

            var p = new Polygon();
            p.Points.Add(new System.Windows.Point(tree.Plane.Points[0].X, tree.Plane.Points[0].Y));
            p.Points.Add(new System.Windows.Point(tree.Plane.Points[1].X, tree.Plane.Points[1].Y));
            p.Points.Add(new System.Windows.Point(tree.Plane.Points[2].X, tree.Plane.Points[2].Y));

            p.Stroke = Brushes.Black;
            p.Fill = Brushes.Red;
            grid.Children.Add(p);

            this.Draw(tree.Right);
        }
    }
}
