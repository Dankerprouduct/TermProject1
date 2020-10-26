using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TermProject1.AI
{
    public class Pathfinding
    {
        public Dictionary<Point, Point> cameFrom
            = new Dictionary<Point, Point>();
        public Dictionary<Point, double> costSoFar
            = new Dictionary<Point, double>();

        public List<Point> Path = new List<Point>();
        public Pathfinding(WeightedGraph<Point> graph, Point start, Point goal)
        {
            var frontier = new PriorityQueue<Point>();
            frontier.Enqueue(start, 0);

            //Path.Add(start);
            cameFrom[start] = start;
            costSoFar[start] = 0;
            var c = new Point(); 
            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    c = current;
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    double newCost = costSoFar[current]
                                     + graph.Cost(current, next);
                    if (!costSoFar.ContainsKey(next)
                        || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        double priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                        c = next; 
                    }
                }
            }

            var n = c;
            while (n != start)
            {
                Path.Add(n);
                try
                {
                    n = cameFrom[n];
                }
                catch (Exception e)
                {
                    break;
                }
            }

            Path.Reverse();
        }

        

        static public double Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.X - b.Y);
        }

    }

    public class PriorityQueue<T>
    {
        // I'm using an unsorted array for this example, but ideally this
        // would be a binary heap. There's an open issue for adding a binary
        // heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
        //
        // Until then, find a binary heap class:
        // * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
        // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
        // * http://xfleury.github.io/graphsearch.html
        // * http://stackoverflow.com/questions/102398/priority-queue-in-net

        private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, double priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

    public interface WeightedGraph<L>
    {
        double Cost(Point a, Point b);
        IEnumerable<Point> Neighbors(Point id);
    }

    public class SquareGrid : WeightedGraph<Point>
    {
        // Implementation notes: I made the fields public for convenience,
        // but in a real project you'll probably want to follow standard
        // style and make them private.

        public static readonly Point[] DIRS = new[]
        {
            new Point(1, 0),
            new Point(0, -1),
            new Point(-1, 0),
            new Point(0, 1)
        };

        public int width, height;
        public HashSet<Point> walls = new HashSet<Point>();
        public HashSet<Point> forests = new HashSet<Point>();

        public SquareGrid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public bool InBounds(Point id)
        {
            return 0 <= id.X && id.X < width
                             && 0 <= id.Y && id.Y < height;
        }

        public bool Passable(Point id)
        {
            return !walls.Contains(id);
        }

        public double Cost(Point a, Point b)
        {
            return forests.Contains(b) ? 5 : 1;
        }

        public IEnumerable<Point> Neighbors(Point id)
        {
            foreach (var dir in DIRS)
            {
                Point next = new Point(id.X + dir.X, id.Y + dir.Y);
                if (InBounds(next) && Passable(next))
                {
                    yield return next;
                }
            }
        }
    }
}
