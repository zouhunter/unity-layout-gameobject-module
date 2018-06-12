using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LayoutGameObject
{
    public class LayoutUtil
    {
        public static Vector3[] CalcutePoints(LayoutItem item)
        {
            if(item.enableClip)
            {
                return CalcutePoints(item.count.x, item.count.y, item.count.z, item.span.x, item.span.y, item.span.z, item.style, item.bounds);
            }
            else
            {
                return CalcutePoints(item.count.x, item.count.y, item.count.z, item.span.x, item.span.y, item.span.z, item.style,null);
            }
        }

        public static Vector3[] CalcutePoints(int xCount, int yCount, int zCount, float xSpan, float ySpan, float zSpan, LayoutStyle style,List<Bounds> bounds)
        {
            switch (style)
            {
                case LayoutStyle.Normal:
                    return CalcuteNormalPoints(xCount, yCount, zCount, xSpan, ySpan, zSpan, bounds);
                case LayoutStyle.Cross:
                    return CalcuteCrossPoints(xCount, yCount, zCount, xSpan, ySpan, zSpan, bounds);
                default:
                    break;
            }
            return null;
        }

        public static Vector3[] CalcuteCrossPoints(int xCount, int yCount, int zCount, float xSpan, float ySpan, float zSpan,List<Bounds> bounds = null)
        {
            var list = new List<Vector3>();
            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    for (int z = 0; z < zCount; z++)
                    {
                        var xpos = y % 2 == 0 ? xSpan * x : xSpan * x + xSpan / 2f;
                        var ypos = z % 2 == 0 ? ySpan * y : ySpan * y + ySpan / 2f;
                        var zpos = x % 2 == 0 ? zSpan * z : zSpan * z + zSpan / 2f;
                        var point = new Vector3(xpos, ypos, zpos);
                        if (bounds == null || IsInBounds(point, bounds))
                        {
                            list.Add(point);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static Vector3[] CalcuteNormalPoints(int xCount, int yCount, int zCount, float xSpan, float ySpan, float zSpan, List<Bounds> bounds = null)
        {
            var list = new List<Vector3>();
            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    for (int z = 0; z < zCount; z++)
                    {
                        var point = new Vector3(x * xSpan, y * ySpan, z * zSpan);
                        if (bounds == null || IsInBounds(point, bounds))
                        {
                            list.Add(point);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        private static bool IsInBounds(Vector3 point,List<Bounds> bounds)
        {
            var activeBounds = bounds.Where(x => !x.reverse);
            var disableBounds = bounds.Where(x=> x.reverse);
            var contentBounds = from bound in bounds
                                where bound.Contains(point)
                                select bound;
            //没有激活区域
            if (activeBounds.Count() == 0)
            {
                if (contentBounds == null || contentBounds.Count() == 0)
                {
                    //不在禁用区域内
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //没有禁用区域
            else if (disableBounds.Count() == 0)
            {
                if (contentBounds == null || contentBounds.Count() == 0)
                {
                    //不在显示区域内
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (activeBounds.Where(bound => bound.Contains(point)).Count() == 0)
            {
                //不在显示区域内（不显示）
                return false;
            }

            if(disableBounds.Where(bound => bound.Contains(point)).Count() > 0)
            {
                //在禁用区域内（不显示）
                return false;
            }

            return true;
        }
    }
}