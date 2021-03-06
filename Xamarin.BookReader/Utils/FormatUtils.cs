﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using Android.Text;

namespace Xamarin.BookReader.Utils
{
    public class FormatUtils
    {
        private const long ONE_MINUTE = 60000L;
        private const long ONE_HOUR = 3600000L;
        private const long ONE_DAY = 86400000L;
        private const long ONE_WEEK = 604800000L;

        private const string ONE_SECOND_AGO = "秒前";
        private const string ONE_MINUTE_AGO = "分钟前";
        private const string ONE_HOUR_AGO = "小时前";
        private const string ONE_DAY_AGO = "天前";
        private const string ONE_MONTH_AGO = "月前";
        private const string ONE_YEAR_AGO = "年前";

        private static SimpleDateFormat sdf = new SimpleDateFormat();
        public static string FORMAT_DATE_TIME = "yyyy-MM-dd HH:mm:ss.SSS";

        /**
         * 获取当前日期的指定格式的字符串
         *
         * @param format 指定的日期时间格式，若为null或""则使用指定的格式"yyyy-MM-dd HH:mm:ss.SSS"
         * @return
         */
        public static string getCurrentTimeString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                sdf.ApplyPattern(FORMAT_DATE_TIME);
            }
            else
            {
                sdf.ApplyPattern(format);
            }
            return sdf.Format(new Date());
        }

        /**
         * 根据时间字符串获取描述性时间，如3分钟前，1天前
         *
         * @param dateString 时间字符串
         * @return
         */
        public static string getDescriptionTimeFromDateString(string dateString)
        {
            if (TextUtils.IsEmpty(dateString))
                return "";
            sdf.ApplyPattern(FORMAT_DATE_TIME);
            try
            {
                return getDescriptionTimeFromDate(sdf.Parse(formatZhuiShuDateString(dateString)));
            }
            catch (Exception e)
            {
                LogUtils.e("getDescriptionTimeFromDateString: " + e);
            }
            return "";
        }

        /**
         * 格式化追书神器返回的时间字符串
         *
         * @param dateString 时间字符串
         * @return
         */
        public static string formatZhuiShuDateString(string dateString)
        {
            return dateString.Replace("T", " ").Replace("Z", "");
        }

        /**
         * 根据Date获取描述性时间，如3分钟前，1天前
         *
         * @param date
         * @return
         */
        public static string getDescriptionTimeFromDate(Date date)
        {
            long delta = new Date().Time - date.Time;
            if (delta < 1L * ONE_MINUTE)
            {
                long seconds = toSeconds(delta);
                return (seconds <= 0 ? 1 : seconds) + ONE_SECOND_AGO;
            }
            if (delta < 45L * ONE_MINUTE)
            {
                long minutes = toMinutes(delta);
                return (minutes <= 0 ? 1 : minutes) + ONE_MINUTE_AGO;
            }
            if (delta < 24L * ONE_HOUR)
            {
                long hours = toHours(delta);
                return (hours <= 0 ? 1 : hours) + ONE_HOUR_AGO;
            }
            if (delta < 48L * ONE_HOUR)
            {
                return "昨天";
            }
            if (delta < 30L * ONE_DAY)
            {
                long days = toDays(delta);
                return (days <= 0 ? 1 : days) + ONE_DAY_AGO;
            }
            if (delta < 12L * 4L * ONE_WEEK)
            {
                long months = toMonths(delta);
                return (months <= 0 ? 1 : months) + ONE_MONTH_AGO;
            }
            else
            {
                long years = toYears(delta);
                return (years <= 0 ? 1 : years) + ONE_YEAR_AGO;
            }
        }

        private static long toSeconds(long date)
        {
            return date / 1000L;
        }

        private static long toMinutes(long date)
        {
            return toSeconds(date) / 60L;
        }

        private static long toHours(long date)
        {
            return toMinutes(date) / 60L;
        }

        private static long toDays(long date)
        {
            return toHours(date) / 24L;
        }

        private static long toMonths(long date)
        {
            return toDays(date) / 30L;
        }

        private static long toYears(long date)
        {
            return toMonths(date) / 365L;
        }

        public static string formatWordCount(int wordCount)
        {
            if (wordCount / 10000 > 0)
            {
                return (int)((wordCount / 10000f) + 0.5) + "万字";
            }
            else if (wordCount / 1000 > 0)
            {
                return (int)((wordCount / 1000f) + 0.5) + "千字";
            }
            else
            {
                return wordCount + "字";
            }
        }
    }
}