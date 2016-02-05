﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Bellona.Controller {
    class SendKey {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern bool PostMessage(IntPtr hWnd, uint Msg, string wParam, string lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        private static readonly uint WM_KEYDOWN = 0x0100;
        private static readonly uint WM_KEYUP = 0x0101;

        public static void Send(ConstController.WindowsVirtualKey Key) {
            IntPtr Handle = FindWindow(null, Program.PROCESS_WINDOW_TITLE);
            PostMessage(Handle, WM_KEYDOWN, (int)Key, 0);
            PostMessage(Handle, WM_KEYUP, (int)Key, 0);
        }

        public static void KeyDown(ConstController.WindowsVirtualKey Key, ref bool state) {
            IntPtr Handle = FindWindow(null, Program.PROCESS_WINDOW_TITLE);
            PostMessage(Handle, WM_KEYDOWN, (int)Key, 0);
            state = true;
        }
        public static void KeyUp(ConstController.WindowsVirtualKey Key, ref bool state) {
            IntPtr Handle = FindWindow(null, Program.PROCESS_WINDOW_TITLE);
            PostMessage(Handle, WM_KEYUP, (int)Key, 0);
            state = false;
        }
        public static void KeyDown(ConstController.WindowsVirtualKey Key, bool state, ref bool mystate) {
            if (state || mystate) {
                IntPtr Handle = FindWindow(null, Program.PROCESS_WINDOW_TITLE);
                PostMessage(Handle, WM_KEYDOWN, (int)Key, 0);
            }
            mystate = true;
        }
        public static void KeyUp(ConstController.WindowsVirtualKey Key, bool state, ref bool mystate) {
            if (state || mystate) {
                IntPtr Handle = FindWindow(null, Program.PROCESS_WINDOW_TITLE);
                PostMessage(Handle, WM_KEYUP, (int)Key, 0);
            }
            mystate = false;
        }
        public static Rect GetWoWPosition() {
            Rect result = new Rect();
            GetWindowRect(FindWindow(null, Program.PROCESS_WINDOW_TITLE), ref result);
            return result;
        }
    }
}
