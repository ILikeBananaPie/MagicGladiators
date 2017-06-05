using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MagicGladiators
{
    public class SaMM
    {
        //SaMM = Sound and Music Media

        private static SaMM _i;
        public static SaMM i
        {
            get
            {
                if (_i == null)
                {
                    _i = new SaMM();
                }
                return _i;
            }
        }

        //

        private SaMM()
        {
            
        }
}
