using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

using RF.WinApp.Infrastructure.CC;

namespace RF.WinApp
{
    public class ModalSpace : AdornerDecorator
    {
        public static ModalSpace GetSpace(Visual visual)
        {
            return RF.WinApp.Infrastructure.Behaviour.XamlHelper.FindAncestor<ModalSpace>(visual);
        }

        public static void ShowModal(ActionBlock modal, bool isShow)
        {
            var space = ModalSpace.GetSpace(modal);
            if (space != null)
                if (isShow)
                    space.AddModal(modal);
                else
                    space.RemoveModal(modal);
        }

        private Dictionary<ActionBlock, Adorner> modals = new Dictionary<ActionBlock, Adorner>();
        private Dictionary<ActionBlock, ZIndex> modalsPositions = new Dictionary<ActionBlock, ZIndex>();
        private ActionBlock currentModal;

        public void AddModal(ActionBlock modal)
        {
            if (!modals.ContainsKey(modal))
            {
                var adorner = new SmokeScreenAdorner(modal, modal.ModalScopeElement ?? modal);
                modals.Add(modal, adorner);
            }

            if (!modalsPositions.ContainsKey(modal))
                modalsPositions.Add(modal, GetGlobalZIndex(modal));

            if (currentModal != null && modalsPositions[currentModal] < modalsPositions[modal])
            {
                this.AdornerLayer.Remove(modals[currentModal]);
                currentModal.IsShaded = true;
                currentModal = null;
            }

            if (currentModal == null)
            {
                currentModal = modal;
                currentModal.IsShaded = false;
                this.AdornerLayer.Add(modals[currentModal]);
            }
        }

        public void RemoveModal(ActionBlock modal)
        {
            modal.IsShaded = true;
            var adorner = modals[modal];
            modals.Remove(modal);
            modalsPositions.Remove(modal);
            this.AdornerLayer.Remove(adorner);

            if (currentModal == modal)
            {
                currentModal = null;

                if (modalsPositions.Keys.Count() > 0)
                {
                    var maxPos = modalsPositions.Values.Max();
                    currentModal = modalsPositions.FirstOrDefault(kvp => kvp.Value == maxPos).Key;

                    if (currentModal != null)
                    {
                        currentModal.IsShaded = false;
                        this.AdornerLayer.Add(modals[currentModal]);
                    }
                }
            }
        }

        private ZIndex GetGlobalZIndex(ActionBlock modal)
        {
            var idx = new ZIndex();
            return GetZIndex(modal, Application.Current.MainWindow, idx);
        }

        private ZIndex GetZIndex(ActionBlock modal, DependencyObject parent, ZIndex parentIdx)
        {
            if (parent == modal)
                return parentIdx;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var newIdx = new ZIndex(parentIdx.Index);
                newIdx.Add(i);
                var ret = GetZIndex(modal, VisualTreeHelper.GetChild(parent, i), newIdx);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        private class ZIndex : IComparer, IComparable
        {
            internal Dictionary<int, int> Index;

            internal ZIndex(Dictionary<int, int> index)
            {
                this.Index = new Dictionary<int, int>(index);
            }

            internal ZIndex()
            {
                this.Index = new Dictionary<int, int>();
            }

            internal void Add(int pos)
            {
                if (this.Index.Count == 0)
                    this.Index.Add(0, pos);
                else
                    this.Index.Add(this.Index.Keys.Last() + 1, pos);
            }

            public static bool operator >(ZIndex x, ZIndex y)
            {
                if (x.Index.Count > y.Index.Count)
                {
                    if (y.Index.Count == 0)
                        return true;
                    var last = y.Index.Last();
                    if (x.Index.ContainsKey(last.Key) && x.Index[last.Key] >= last.Value)
                        return true;
                }
                else if (x.Index.Count <= y.Index.Count && x.Index.Count > 0)
                {
                    var last = x.Index.Last();
                    if (!y.Index.ContainsKey(last.Key) || (y.Index[last.Key] < last.Value) || (y.Index[last.Key] == last.Value && x.Index.Count == y.Index.Count))
                        return true;
                }

                return false;
            }

            public static bool operator <(ZIndex x, ZIndex y)
            {
                return y > x;
            }

            public override bool Equals(object obj)
            {
                var s = obj as ZIndex;

                if (s == null)
                    return false;

                return this.Index.Equals(s.Index);
            }

            public override int GetHashCode()
            {
                return Index.GetHashCode();
            }

            public int Compare(object x, object y)
            {
                var zx = x as ZIndex;
                var zy = y as ZIndex;
                if (zx > zy) return 1;
                if (zx < zy) return -1;
                return 0;
            }

            public int CompareTo(object obj)
            {
                return this.Compare(this, obj);
            }
        }
    }
}
