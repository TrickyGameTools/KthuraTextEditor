// Lic:
// 	Kthura Text Editor
// 	
// 	
// 	
// 	
// 	(c) Jeroen P. Broks, 2018, All rights reserved
// 	
// 		This program is free software: you can redistribute it and/or modify
// 		it under the terms of the GNU General Public License as published by
// 		the Free Software Foundation, either version 3 of the License, or
// 		(at your option) any later version.
// 		
// 		This program is distributed in the hope that it will be useful,
// 		but WITHOUT ANY WARRANTY; without even the implied warranty of
// 		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// 		GNU General Public License for more details.
// 		You should have received a copy of the GNU General Public License
// 		along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 		
// 	Exceptions to the standard GNU license are available with Jeroen's written permission given prior 
// 	to the project the exceptions are needed for.
// Version: 18.09.19
// EndLic
﻿using System;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;

namespace KthuraTextEditor
{
    class KthuraLoadedFile{

    }

    class MainClass
    {
        static void initMKL(){
            MKL.Lic    ("Kthura Text Editor - Program.cs","GNU General Public License 3");
            MKL.Version("Kthura Text Editor - Program.cs","18.09.19");

        }

        static void InitGUI(){
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
            Application.Run();
        }

        public static void Main(string[] args)
        {
            initMKL();
        }
    }
}
