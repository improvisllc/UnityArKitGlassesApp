﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class NativeInfoProvider
{

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("FaceEngine")]
#endif
    unsafe public static extern int sampleFunc(int number);


#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("FaceEngine")]
#endif
    unsafe public static extern int init(int height, int width);

#if UNITY_IOS
    [DllImport("__Internal")]
#else
    [DllImport("FaceEngine")]
#endif
    unsafe public static extern void _appendFrameFromImage(byte[] a_ImageBuffer, int a_BufferLength, int a_FrameTimeInMilliseconds, int a_startOrStop);





}
