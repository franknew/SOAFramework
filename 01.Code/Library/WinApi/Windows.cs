using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SOAFramework.Library.WinApi
{
    public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);

    public class Windows
    {
        #region const
        public const int WM_SETTEXT = 0x000C;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWNOACTIVATE = 4;
        //SendMessage参数
        public const int WM_KEYDOWN = 0X100;
        public const int WM_KEYUP = 0X101;
        public const int WM_SYSCHAR = 0X106;
        public const int WM_SYSKEYUP = 0X105;
        public const int WM_SYSKEYDOWN = 0X104;
        public const int WM_CHAR = 0X102;
        public const int BM_CLICK = 0xF5;

        public const int Mouse_Cliking = 0x0002;
        public const int Mouse_Clicked = 0x0004;
        #endregion

        #region api
        [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(uint dwErrCode);

        [DllImport("USER32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("USER32.DLL", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, UInt32 wMsg, int wParam, string lParam);

        [DllImport("USER32.DLL", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);

        [DllImport("USER32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// 获得鼠标位置
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("USER32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(out Point pt);

        /// <summary>
        /// 获得指定位置的窗体
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("USER32.dll", EntryPoint = "WindowFromPoint")]
        public static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("USER32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("USER32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("USER32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 获得当前激活窗体
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 获得窗体大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref Point lpPoints, uint cPoints);
        #endregion

        #region methods
        public static void SetText(IntPtr hWnd, string lParam)
        {
            SendMessage(hWnd, WM_SETTEXT, (int)IntPtr.Zero, lParam);
        }

        public static string GetText(IntPtr hWnd)
        {
            StringBuilder text = new StringBuilder(256);
            GetWindowText(hWnd, text, text.Capacity);
            return text.ToString();
        }

        public static IntPtr WindowFromMouse()
        {
            Point p = Point.Empty;
            GetCursorPos(out p);
            if (p == Point.Empty) return IntPtr.Zero;
            return WindowFromPoint(p);
        }

        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder builder = new StringBuilder(256);
            GetClassName(hWnd, builder, builder.Capacity);
            return builder.ToString();
        }

        public static Rectangle GetWindowRec(IntPtr hWnd)
        {
            RECT rec = new RECT();
            GetWindowRect(hWnd, ref rec);
            Rectangle rectangle = new Rectangle();
            if (rec.IsEmpty()) return rectangle;
            rectangle = new Rectangle(rec.Left, rec.Top, rec.Right - rec.Left, rec.Bottom - rec.Top);
            return rectangle;
        }

        public static void Click(IntPtr hWnd)
        {
            SendMessage(hWnd, BM_CLICK, 0, 0);
        }

        public static IntPtr FindForm(IntPtr hWnd)
        {
            var parentHandle = GetParent(hWnd);
            var type = GetControlType(parentHandle);
            if (parentHandle == IntPtr.Zero) return hWnd;
            parentHandle = FindForm(parentHandle);
            return parentHandle;
        }

        public static ControlTypeEnum GetControlType(IntPtr hWnd)
        {
            var className = GetClassName(hWnd);
            var classes = className.Split('.');
            if (classes.Length <= 1) return ControlTypeEnum.Form;
            string type = classes[1];
            ControlTypeEnum controlType = ControlTypeEnum.Form;
            switch (type.ToLower())
            {
                case "button":
                    controlType = ControlTypeEnum.Button;
                    break;
                case "edit":
                    controlType = ControlTypeEnum.TextBox;
                    break;
                case "window":
                    controlType = ControlTypeEnum.Form;
                    break;
                case "static":
                    controlType = ControlTypeEnum.Label;
                    break;
            }
            return controlType;
        }
        #endregion

        #region struct
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标

            public bool IsEmpty()
            {
                return Left == 0 & Top == 0 & Right == 0 & Bottom == 0;
            }
        }
        #endregion

    }
    #region enum
    public enum ControlTypeEnum
    {
        Form,
        Button,
        TextBox,
        Label
    }
    #endregion
}
