using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    // -------------------------------- Request Keys ---------------------------------------- //

    public static readonly string s_brandNamesReqKey = "brandNames";
    public static readonly string s_brandChecksumsReqKey = "brandChecksums";
    public static readonly string s_glassesNamesReqKey = "glassesNames";
    public static readonly string s_glassesChecksumsReqKey = "glassesChecksums";
    public static readonly string s_platformReqKey = "platform";

    // -------------------------------- Request Keys ---------------------------------------- //


    public static readonly string s_brandThumbName = "BrandThumb";








    public static readonly string s_managerObjName = "_manager";
    public static string s_mainCanvasName = "MainCanvas";
    public static string s_eventSystemName = "EventSystem";

    public static string s_manufacturerButtonName = "ManufacturerBtn_";
    public static string s_manufacturerScrollRectName = "ManufacturerScrollRect";
    public static string s_manufacturerScrollContentParentName = "ManufacturerScrollContentParent";

    public static string s_glassesButtonName = "GlassesBtn_";
    public static string s_glassesScrollRectName = "GlassesScrollRect";
    public static string s_glassesScrollContentParentName = "GlassesScrollContentParent";

    public static readonly string s_glassesSliderName = "GlassesSlider";

    public static string s_customSingleLineInputUIElementName = "SingleLine";

    public static string s_canvasUIName = "Canvas";


    public static float s_manufacturerButtonWidth = 140;
    public static float s_manufacturerButtonHeight = 140;

    public static int s_screenHeight = Screen.height / 2;
    public static int s_screenWidth = Screen.width / 2;

    public static float s_filterButtonWidth = 140;
    public static float s_filterButtonHeight = 65;


    public static float s_distanceBetweenFilterButtons = 17;
    public static float s_distanceBetweenBottomSideAndFilterButton = 100;
    public static float s_distanceBetweenLeftSideAndFirstFilterButton = 30;
    public static float s_distanceBetweenRightSideAndLastFilterButton = 30;

    public static Vector2 s_canvasCenterPoint = new Vector2(0, 0);


    // Layers
    public static string s_UILayerName = "UI";

    public static readonly int s_glassesCount = 20;

    public static readonly int s_glassesBackgroundSize = 150;
    public static readonly string s_glassesBackgroundName = "GlassesBackground_";

    //
    public static readonly string s_UIPrefabsReourceFolderName = "UIPrefabs";
    public static readonly string s_searchInputFieldPrefabName = "SearchInputField";
    public static readonly string s_searchButtonPrefabName = "SearchButton";

    public static readonly string s_manufacturersReourceFolderName = "Manufacturers";
    public static readonly string s_oneManufacturerFolderResourceName = "manufacturer_";
    public static readonly string s_manufacturerTextureResourceName = "manufacturer_";

    public static readonly string s_3DModelResourceFolderName = "3D_Model";
    //public static readonly string s_3DModelResourceName = "Glasses";

    public static readonly string s_searchCustomInputName = "SearchCustomInput";
    public static readonly string s_screentouchPanelName = "ScreenTouchPanel";
    public static readonly string s_bottomButtonsPanelName = "BottomButtonsPanel";
    public static readonly string s_bottomRightFitPanelName = "RightFitPanel";

    public static readonly string s_manufacturerSRCenterPointName = "ManufacturerScrollRectCenterPointX";
    public static readonly string s_manufacturerSRLeftPointName = "ManufacturerScrollRectLeftPoint";
    public static readonly string s_manufacturerSRRightPointName = "ManufacturerScrollRectRightPoint";

    public static readonly string s_glassesSRCenterPointName = "GlassesScrollRectCenterPointX";
    public static readonly string s_glassesSRLeftPointName = "GlassesScrollRectLeftPoint";
    public static readonly string s_glassesSRRightPointName = "GlassesScrollRectRightPoint";

    public static readonly string s_cameraPreviewMeshName = "CameraPreviewMesh";

    public static readonly string s_glassesPointerName = "GlassesPointer";
    public static readonly string s_redRectangleName = "RedRectangle";

    public static readonly string s_oneGlassesItemResourceName = "glasses_";

    //public static string s_leftBindObjectName = "LeftBindPoint";
    //public static string s_rightBindObjectName = "RightBindPoint";

   // public static string s_leftTempleEndPointName = "TempleEndPointLeft";
   // public static string s_rightTempleEndPointName = "TempleEndPointRight";

    /// /// //////////////////////
    /// /// //////////////////////
    /// /// //////////////////////

    public static readonly string s_glassesPointLName = "GlassesPointL";
    public static readonly string s_glassesPointRName = "GlassesPointR";
    //
    public static readonly string s_templeRotPointLName = "TempleRotPointL";
    public static readonly string s_templeRotPointRName = "TempleRotPointR";
    //
    public static readonly string s_earpieceStartPointLName = "EarpieceStartPointL";
    public static readonly string s_earpieceStartPointRName = "EarpieceStartPointR";
    //
    public static readonly string s_earpieceEndPointLName = "EarpieceEndPointL";
    public static readonly string s_earpieceEndPointRName = "EarpieceEndPointR";
    //
    public static readonly string s_lensCenterPointLName = "LensCenterPointL";
    public static readonly string s_lensCenterPointRName = "LensCenterPointR";
    //
    public static readonly string s_frameBottomPointLName = "FrameBottomPointL";
    public static readonly string s_frameBottomPointRName = "FrameBottomPointR";
    //
    public static readonly string s_frameTopPointLName = "FrameTopPointL";
    public static readonly string s_frameTopPointRName = "FrameTopPointR";
    //////////
    //public static readonly string s_templeLeftName = "TempleLeft";
    //public static readonly string s_templeRightName = "TempleRight";

    public static readonly string s_greenPointsParentObjectName = "GreenPointsParent";

    public static readonly string s_glassesUnzippedFolderName = "UnzippedGlassesFolder";

    public static readonly string s_glassesNetworkingFolderName = "GlassesNetworkingFolder";

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && UNITY_ANDROID
    //public static readonly string s_glassesZipSavingPath = "C:/Users/Garik/Desktop/GarikFiles/TempFolderForUnityGlassesNetworking";
    //public static readonly string s_glassesUnZippingFolderPath = "C:/Users/Garik/Desktop/GarikFiles/TempFolderForUnityGlassesNetworking/GlassesUnzipFolder";

    //public static readonly string s_glassesZipSavingPath =  Application.dataPath + "/StreamingAssets" + "/GlassesNetworkingFolder";
    //public static readonly string s_glassesUnZippingFolderPath = Application.dataPath + "/StreamingAssets" + "/GlassesNetworkingFolder" + "/GlassesUnzippingFolder";


#endif

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
    public static readonly string s_glassesZipSavePath = Application.temporaryCachePath;
    public static readonly string s_glassesUnZipFolderPath = Application.temporaryCachePath + "/GlassesUnzipFolder";
#endif

    public static readonly string s_downloadedGlassesZipFileName = "JustDownloadedGlassesZip";

}