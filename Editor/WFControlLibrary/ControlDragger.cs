using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.Control;

namespace WFControlLibrary
{
    /// <summary>
    /// Все обработчики событий будут отписаны после вызова StopDrag()
    /// </summary>
    internal static class ControlDragger
    {
        public static event Action<Control[]> OnStartDrag;
        public static event Action<Control[]> OnDrag;
        public static event Action<Control[], Point> OnStopDrag;

        static ControlDragger() { updater.Interval = interval; updater.Tick += Drag; }

        public static int fps = 1000 / interval;

        public static List<Control> TargetList = new List<Control>();
        private static Dictionary<Control, Point> targetStartPosList = new Dictionary<Control, Point>();

        private static Timer updater = new Timer();
        private static int interval => 5;

        private static Point startMousePos = new Point(0, 0);
        private static Point translateVector => MousePosition.Sub(startMousePos);

        public static void StartDrag()
        {
            foreach (var item in TargetList)
            {
                targetStartPosList.Add(item, item.Location);
            }
            startMousePos = MousePosition;
            updater.Start();
            OnStartDrag?.Invoke(TargetList.ToArray());
        }
        public static void StartDrag(params Control[] targetList)
        {
            TargetList.Clear();
            foreach (var item in targetList)
            {
                TargetList.Add(item);
            }
            StartDrag();
        }

        private static void Drag(object sender, EventArgs e)
        {
            foreach (var item in TargetList)
            {
                item.Location = targetStartPosList[item].Add(translateVector);
            }
            OnDrag?.Invoke(TargetList.ToArray());
        }

        public static void StopDrag()
        {
            OnStopDrag?.Invoke(TargetList.ToArray(), translateVector);
            TargetList.Clear();
            targetStartPosList.Clear();
            updater.Stop();
            OnStopDrag = null;
            OnStartDrag = null;
            OnDrag = null;
        }
    }
}
