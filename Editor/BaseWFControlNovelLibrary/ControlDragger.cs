using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Windows.Forms.Control;

namespace BaseWFControlNovelLibrary
{
    /// <summary>
    /// Все обработчики событий будут отписаны после вызова StopDrag()
    /// </summary>
    internal static class ControlDragger
    {
        public static event Action<Control[]> OnStartDrag;
        public static event Action<Control[]> OnDrag;
        public static event Action<Control[], Point> OnStopDrag;

        static ControlDragger() { updater.Interval = update_interval; updater.Tick += Drag; }

        public static int fps = 1000 / update_interval;

        public static List<Control> TargetList = new List<Control>();
        private static Dictionary<Control, Point> targetStartPosList = new Dictionary<Control, Point>();

        private static Timer updater = new Timer();
        private static int update_interval => 5;

        private static Point mouse_position_onDown = new Point(0, 0);
        private static Point mouse_move_vector => MousePosition.Sub(mouse_position_onDown);

        public static void StartDrag()
        {
            Parallel.ForEach(TargetList, item => targetStartPosList.Add(item, item.Location));
            mouse_position_onDown = MousePosition;
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
                try { item.Location = targetStartPosList[item].Add(mouse_move_vector); }
                catch { }
            }
            OnDrag?.Invoke(TargetList.ToArray());
        }
        public static void StopDrag()
        {
            OnStopDrag?.Invoke(TargetList.ToArray(), MousePosition.Sub(mouse_position_onDown));
            TargetList.Clear();
            targetStartPosList.Clear();
            updater.Stop();
            OnStopDrag = null;
            OnStartDrag = null;
            OnDrag = null;
        }
    }
}
