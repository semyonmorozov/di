using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class UniquePositivePointsFromSpiral : IEnumerable, IEnumerator
    {
        private readonly Spiral spiral;
        private int index=-1;
        public UniquePositivePointsFromSpiral(Spiral spiral)
        {
            this.spiral = spiral;
        }
        public object Current => spiral.GetPoint(index);

        public bool MoveNext()
        {
            var oldPoint = Current;
            index++;
            while(oldPoint.Equals(Current)||!IsPositive((Point)Current))
                    index++;
            return true;
        }

        private bool IsPositive(Point point)
        {
            return point.X > 0 && point.Y > 0;
        }

        public void Reset()
        {
            index = -1;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }
    }
}
