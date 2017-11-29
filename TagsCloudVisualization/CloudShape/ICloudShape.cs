using System.Drawing;

namespace TagsCloudVisualization
{
    internal interface ICloudShape 
    {
        Point Current { get;}
        bool MoveNext();
        void Reset();
    }
}