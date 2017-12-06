using System.Drawing;

namespace TagsCloudVisualization.CloudShape
{
    internal interface ICloudShape 
    {
        Point Current { get;}
        bool MoveNext();
        void Reset();
    }
}