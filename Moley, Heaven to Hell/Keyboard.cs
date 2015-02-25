using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Moley_Heaven_to_Hell
{
    static class Keyboard
    {
        private enum KeyStates { None = 0, Down = 1, }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            if ((retVal & 0x8000) == 0x8000)
            {
                state = KeyStates.Down;
            }

            return state;
        }

        public static bool IsKeyDown(Keys key)
        {
            return GetKeyState(key) == KeyStates.Down;
        }
    }
}