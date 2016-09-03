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
        private System.Windows.Point? _transitionPoint;
        private System.Windows.Point? _lastTransitionPoint;
        private bool move = false;
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
                var polyhedronPlanes = new List<PlaneImport>();

                foreach (var plane in item.planes)
                {
                    polyhedronPlanes.Add(new PlaneImport
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
            this.canvas.Children.Clear();

            foreach (var item in this._trees)
            {
                this.Draw(item);
            }

            if (this._transitionPoint != null)
            {
                var el = new Ellipse();
                el.Height = 5;
                el.Width = 5;
                el.Fill = Brushes.Black;
                el.Margin = new Thickness(this._transitionPoint.Value.X, this._transitionPoint.Value.Y, 0, 0);

                this.canvas.Children.Add(el);
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
            canvas.Children.Add(p);

            this.Draw(tree.Right);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(this.canvas);

                if (this._lastTransitionPoint == null)
                {
                    this._lastTransitionPoint = this._transitionPoint;
                }

                if (move)
                {
                    this._trees.First().MoveAlong(WorkAxis.Ox, position.X - this._lastTransitionPoint.Value.X);
                    this._trees.First().MoveAlong(WorkAxis.Oy, position.Y - this._lastTransitionPoint.Value.Y);
                }
                else
                {
                    //moving from one grid side to other wiil cause single rotation
                    var length = this.canvas.ActualWidth;
                    var height = this.canvas.ActualHeight;

                    this._trees.First().Rotate(WorkAxis.Oy, 180 * (position.X - this._lastTransitionPoint.Value.X) / length);
                    this._trees.First().Rotate(WorkAxis.Ox, 180 * (position.Y - this._lastTransitionPoint.Value.Y) / height);
                }

                this._lastTransitionPoint = position;
                this.Draw();

            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this._lastTransitionPoint = null;
            this._transitionPoint = e.GetPosition(this.canvas);

            //check if transition point is inside the figure
            //if so - move the figure
            //else rotate the scene
            this.move = this._trees.First().Contains(this._transitionPoint.Value);
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var coeff = e.Delta > 0 ? 1.5 : 1.0/1.5;

            this._trees.First().Scale(coeff);

            this.Draw();
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._transitionPoint = null;
            this.Draw();
        }
    }
}
