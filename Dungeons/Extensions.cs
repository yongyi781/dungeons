﻿using System.Drawing;
using System.Windows.Forms;

namespace Dungeons
{
    public static class Extensions
    {
        public static bool IsOnScreen(this Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                var formRectangle = new Rectangle(form.Left, form.Top,
                                                         form.Width, form.Height);

                if (screen.WorkingArea.Contains(formRectangle))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
