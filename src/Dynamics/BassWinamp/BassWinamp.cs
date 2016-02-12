﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// BassWinamp: Wraps bass_winamp.dll.
    /// </summary>
    /// <remarks>
    /// Adds Winamp Input plugins support.
    /// BassWinamp doesn't support Bass plugin system.
    /// </remarks>
    public static class BassWinamp
    {
        const string DllName = "bass_winamp";

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_AboutPlugin")]
        public static extern void AboutPlugin(int handle, IntPtr win);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_ConfigPlugin")]
        public static extern void ConfigPlugin(int handle, IntPtr win);

        [DllImport(DllName)]
        static extern IntPtr BASS_WINAMP_FindPlugins(string directory, WinampFindPluginFlags flags);

        public static IEnumerable<string> FindPlugins(string plugindirectory, WinampFindPluginFlags flags)
        {
            var ptr = BASS_WINAMP_FindPlugins(plugindirectory,
                                              flags & (WinampFindPluginFlags.Input | WinampFindPluginFlags.Recursive));

            if (ptr == IntPtr.Zero) return null;

            return Tags.ExtractMultiStringAnsi(ptr);
        }

        [DllImport(DllName)]
        static extern IntPtr BASS_WINAMP_GetExtentions(int handle);

        public static IEnumerable<string> GetExtentions(int handle)
        {
            var ptr = BASS_WINAMP_GetExtentions(handle);

            if (ptr == IntPtr.Zero) return null;

            return Tags.ExtractMultiStringAnsi(ptr);
        }

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_GetIsSeekable")]
        public static extern bool IsSeekable(int handle);

        [DllImport(DllName)]
        static extern bool BASS_WINAMP_GetFileInfoInt(string file, IntPtr title, out int lenms);

        public static bool GetFileInfo(string file, out string title, out int lenms)
        {
            var titleptr = Marshal.AllocHGlobal(255);

            bool Result = BASS_WINAMP_GetFileInfoInt(file, titleptr, out lenms);

            title = Marshal.PtrToStringAnsi(titleptr);

            Marshal.FreeHGlobal(titleptr);

            return Result;
        }

        [DllImport(DllName)]
        static extern IntPtr BASS_WINAMP_GetName(int handle);

        public static string GetName(int handle)
        {
            IntPtr ptr = BASS_WINAMP_GetName(handle);

            if (ptr == IntPtr.Zero) return null;

            return Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_GetUsesOutput")]
        public static extern bool GetUsesOutput(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_GetVersion")]
        public static extern int GetVersion(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_InfoDlg")]
        public static extern bool InfoDialog(string file, IntPtr win);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_LoadPlugin")]
        public static extern int LoadPlugin(string file);

        [DllImport(DllName, EntryPoint = "BASS_WINAMP_UnloadPlugin")]
        public static extern void UnloadPlugin(int handle);

        /// <remarks>BassWinamp supports only ASCII filenames</remarks>
        [DllImport(DllName, EntryPoint = "BASS_WINAMP_StreamCreate")]
        public static extern int CreateStream(string filename, BassFlags flags);
    }
}