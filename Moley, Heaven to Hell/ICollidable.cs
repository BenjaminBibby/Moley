using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    interface ICollidable
    {
        RectangleF CollisionBox
        {
            get;
        }
    }
}
