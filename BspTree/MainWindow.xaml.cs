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

            this.Read();

            this.Draw();
        }

        private void Read()
        {
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

            //as we cannot be inside of scene
            //simply check from what side we are currently
            var lookVect = new Base.Point { X = 0, Y = 0, Z = 1 };
            if (LocalMath.ScalarProduct(tree.Plane.NormVect, lookVect) > 0)
            {
                this.Draw(tree.Inner);

                canvas.Children.Add(tree.Plane.GetPolygon());

                this.Draw(tree.Outer);
            }
            else
            {
                this.Draw(tree.Outer);

                canvas.Children.Add(tree.Plane.GetPolygon());

                this.Draw(tree.Inner);
            }
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
                    foreach (var item in this._trees)
                    {
                        item.MoveAlong(WorkAxis.Ox, position.X - this._lastTransitionPoint.Value.X);
                        item.MoveAlong(WorkAxis.Oy, position.Y - this._lastTransitionPoint.Value.Y);
                    }
                }
                else
                {
                    //moving from one grid side to other wiil cause single rotation
                    var length = this.canvas.ActualWidth;
                    var height = this.canvas.ActualHeight;

                    foreach (var item in this._trees)
                    {
                        item.Rotate(WorkAxis.Oy, 180 * (position.X - this._lastTransitionPoint.Value.X) / length);
                        item.Rotate(WorkAxis.Ox, 180 * (position.Y - this._lastTransitionPoint.Value.Y) / height);
                    }
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
            this.move = false;
            foreach (var item in this._trees)
            {
                this.move |= item.Contains(this._transitionPoint.Value);
            }
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var coeff = e.Delta > 0 ? 1.5 : 1.0 / 1.5;

            foreach (var item in this._trees)
            {
                item.Scale(coeff);
            }

            this.Draw();
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._transitionPoint = null;
            this.Draw();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                this.Read();

                this.Draw();
            }
        }
    }
}
