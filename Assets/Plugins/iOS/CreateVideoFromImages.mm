//
//  CreateVideoFromImages.m
//  CreateVideoFromImages
//
//  Created by Hovhannes Sargsyan on 25.01.21.
//

#import <AVFoundation/AVFoundation.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <UIKit/UIKit.h>

//#if __IPHONE_OS_VERSION_MIN_REQUIRED < 80000
//#import <AssetsLibrary/AssetsLibrary.h>
//#endif
//#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 140000
//#import <PhotosUI/PhotosUI.h>
//#endif
//
//#ifdef UNITY_4_0 || UNITY_5_0
//#import "iPhone_View.h"
//#else
//extern UIViewController* UnityGetGLViewController();
//#endif

#define CHECK_IOS_VERSION( version )  ([[[UIDevice currentDevice] systemVersion] compare:version options:NSNumericSearch] != NSOrderedAscending)


@interface CreateVideoFromImages:NSObject

+(void)init:(unsigned int)height width:(unsigned int)width;
+(int)sampleFunc:(int)number;
+(void)_appendFrameFromImage:(const void* *)a_ImageBuffer a_BufferLength:(int)a_BufferLength a_FrameTimeInMilliseconds:(int)a_FrameTimeInMilliseconds a_startOrStop:(int)a_startOrStop;

@end


@class AVAsset, AVAssetWriter, AVAssetWriterInput, AVAssetWriterInputPixelBufferAdaptor,ATHSingleton, NSDate;

@implementation CreateVideoFromImages

static NSMutableArray *images = [NSMutableArray array];
static unsigned int height;
static unsigned int width;


+(int)sampleFunc:(int)number{
  return  2*number;
}

+(void)init:(unsigned int)height width:(unsigned int)width{
  height = height;
  width = width;
  images = [NSMutableArray array];
  
  NSLog(@"init  line 55");

}

+(void)_appendFrameFromImage:(const void* *)a_ImageBuffer a_BufferLength:(int)a_BufferLength a_FrameTimeInMilliseconds:(int)a_FrameTimeInMilliseconds a_startOrStop:(int)a_startOrStop {

  if (a_startOrStop == 1) {
    NSData * imageData = [[NSData alloc] initWithBytes:a_ImageBuffer length:a_BufferLength];
    
    NSRange range = NSMakeRange(4, [imageData length] - 4);
    NSData *refinedData = [imageData subdataWithRange:range];
    NSLog(@"refinedData  %@",refinedData);
    
    UIImage* image = [[UIImage alloc] initWithData:refinedData];
    [images addObject:image];
    
    NSLog(@"image height   %f",image.size.height);
    NSLog(@"image width   %f",image.size.width);

  }else{
    [self saveMovieToLibrary:images a_height:height a_width:width];
  }
}


+ (void)saveMovieToLibrary:(NSMutableArray*)images a_height:(unsigned int)a_height a_width:(unsigned int)a_width
{
    // You just need the height and width of the video here
    // For us, our input and output video was 640 height x 480 width
    // which is what we get from the iOS front camera
//    ATHSingleton *singleton = [ATHSingleton singletons];
    int height = a_height; //singleton.screenHeight;
    int width = a_width; // singleton.screenWidth;
  
  NSLog(@"saveMovieToLibrary  line 77");


    // You can save a .mov or a .mp4 file
    //NSString *fileNameOut = @"temp.mp4";
    NSString *fileNameOut = @"temp.mov";

    // We chose to save in the tmp/ directory on the device initially
    NSString *directoryOut = @"tmp/";
    NSString *outFile = [NSString stringWithFormat:@"%@%@",directoryOut,fileNameOut];
    NSString *path = [NSHomeDirectory() stringByAppendingPathComponent:[NSString stringWithFormat:outFile]];
    NSURL *videoTempURL = [NSURL fileURLWithPath:[NSString stringWithFormat:@"%@%@", NSTemporaryDirectory(), fileNameOut]];

    // WARNING: AVAssetWriter does not overwrite files for us, so remove the destination file if it already exists
    NSFileManager *fileManager = [NSFileManager defaultManager];
    [fileManager removeItemAtPath:[videoTempURL path]  error:NULL];


    // Create your own array of UIImages
//    NSMutableArray *images = [NSMutableArray array];
//    for (int i=0; i<singleton.numberOfScreenshots; i++)
//    {
//        // This was our routine that returned a UIImage. Just use your own.
//        UIImage *image =[self uiimageFromCopyOfPixelBuffersUsingIndex:i];
//        // We used a routine to write text onto every image
//        // so we could validate the images were actually being written when testing. This was it below.
//        image = [self writeToImage:image Text:[NSString stringWithFormat:@"%i",i ]];
//        [images addObject:image];
//    }

// If you just want to manually add a few images - here is code you can uncomment
  NSLog(@"saveMovieToLibrary  line 108");

    NSString *path1 = [NSHomeDirectory() stringByAppendingPathComponent:[NSString stringWithFormat:@"Documents/movie17.mp4"]];
//    NSArray *images = [[NSArray alloc] initWithObjects:
//                      [UIImage imageNamed:@"Image-1"],
//                      [UIImage imageNamed:@"Image-2"],
//                      [UIImage imageNamed:@"Image-3"],
//                      [UIImage imageNamed:@"Image-4"],
//                      [UIImage imageNamed:@"Image-5"],
//                      [UIImage imageNamed:@"Image-6"],
//                      [UIImage imageNamed:@"Image-7"],
//                      [UIImage imageNamed:@"Image-8"],
//                      [UIImage imageNamed:@"Image-9"],
//                      [UIImage imageNamed:@"Image-10"],
//                      [UIImage imageNamed:@"Image-1"],
//                      [UIImage imageNamed:@"Image-2"],
//                      [UIImage imageNamed:@"Image-3"],
//                      [UIImage imageNamed:@"Image-4"],
//                      [UIImage imageNamed:@"Image-5"],
//                      [UIImage imageNamed:@"Image-6"],
//                      [UIImage imageNamed:@"Image-7"],
//                      [UIImage imageNamed:@"Image-8"],
//                      [UIImage imageNamed:@"Image-9"],
//                      [UIImage imageNamed:@"Image-10"],
//                      [UIImage imageNamed:@"Image-1"],
//                      [UIImage imageNamed:@"Image-2"],
//                      [UIImage imageNamed:@"Image-3"],
//                      [UIImage imageNamed:@"Image-4"],
//                      [UIImage imageNamed:@"Image-5"],
//                      [UIImage imageNamed:@"Image-6"],
//                      [UIImage imageNamed:@"Image-7"],
//                      [UIImage imageNamed:@"Image-8"],
//                      [UIImage imageNamed:@"Image-9"],
//                      [UIImage imageNamed:@"Image-10"],
//                      [UIImage imageNamed:@"Image-1"],
//                      [UIImage imageNamed:@"Image-2"],
//                      [UIImage imageNamed:@"Image-3"],
//                      [UIImage imageNamed:@"Image-4"],
//                      [UIImage imageNamed:@"Image-5"],
//                      [UIImage imageNamed:@"Image-6"],
//                      [UIImage imageNamed:@"Image-7"],
//                      [UIImage imageNamed:@"Image-8"],
//                      [UIImage imageNamed:@"Image-9"],
//                      [UIImage imageNamed:@"Image-10"],
//                      [UIImage imageNamed:@"Image-1"],
//                      [UIImage imageNamed:@"Image-2"],
//                      [UIImage imageNamed:@"Image-3"],
//                      [UIImage imageNamed:@"Image-4"],
//                      [UIImage imageNamed:@"Image-5"],
//                      [UIImage imageNamed:@"Image-6"],
//                      [UIImage imageNamed:@"Image-7"],
//                      [UIImage imageNamed:@"Image-8"],
//                      [UIImage imageNamed:@"Image-9"],
//                      [UIImage imageNamed:@"Image-10"],nil];



    [self writeImageAsMovie:images toPath:path1 size:CGSizeMake(height, width)];
}


+(void)writeImageAsMovie:(NSArray *)array toPath:(NSString*)path size:(CGSize)size
{

  NSLog(@"writeImageAsMovie  line 172");

    NSError *error = nil;

    // FIRST, start up an AVAssetWriter instance to write your video
    // Give it a destination path (for us: tmp/temp.mov)
    AVAssetWriter *videoWriter = [[AVAssetWriter alloc] initWithURL:[NSURL fileURLWithPath:path]
                                                           fileType:AVFileTypeQuickTimeMovie
                                                              error:&error];


    NSParameterAssert(videoWriter);

    NSDictionary *videoSettings = [NSDictionary dictionaryWithObjectsAndKeys:
                                   AVVideoCodecTypeH264, AVVideoCodecKey,
                                   [NSNumber numberWithInt:size.width], AVVideoWidthKey,
                                   [NSNumber numberWithInt:size.height], AVVideoHeightKey,
                                   nil];

    AVAssetWriterInput* writerInput = [AVAssetWriterInput assetWriterInputWithMediaType:AVMediaTypeVideo
                                                                         outputSettings:videoSettings];

    AVAssetWriterInputPixelBufferAdaptor *adaptor = [AVAssetWriterInputPixelBufferAdaptor assetWriterInputPixelBufferAdaptorWithAssetWriterInput:writerInput sourcePixelBufferAttributes:nil];
    NSParameterAssert(writerInput);
    NSParameterAssert([videoWriter canAddInput:writerInput]);
    [videoWriter addInput:writerInput];


    //Start a SESSION of writing.
    // After you start a session, you will keep adding image frames
    // until you are complete - then you will tell it you are done.
    [videoWriter startWriting];
    // This starts your video at time = 0
    [videoWriter startSessionAtSourceTime:kCMTimeZero];

    CVPixelBufferRef buffer = NULL;

    // This was just our utility class to get screen sizes etc.
//    ATHSingleton *singleton = [ATHSingleton singletons];

  NSLog(@"writeImageAsMovie  line 212");

    int i = 0;
    while (1)
    {
        // Check if the writer is ready for more data, if not, just wait
        if(writerInput.readyForMoreMediaData){

            CMTime frameTime = CMTimeMake(1, 40);
            // CMTime = Value and Timescale.
            // Timescale = the number of tics per second you want
            // Value is the number of tics
            // For us - each frame we add will be 1/4th of a second
            // Apple recommend 600 tics per second for video because it is a
            // multiple of the standard video rates 24, 30, 60 fps etc.
            CMTime lastTime=CMTimeMake(i*1, 40);
            CMTime presentTime=CMTimeAdd(lastTime, frameTime);

            if (i == 0) {presentTime = CMTimeMake(0, 1);}
            // This ensures the first frame starts at 0.


            if (i >= [array count])
            {
                buffer = NULL;
            }
            else
            {
                // This command grabs the next UIImage and converts it to a CGImage
                buffer = [self pixelBufferFromCGImage:[[array objectAtIndex:i] CGImage]];
            }


            if (buffer)
            {
                // Give the CGImage to the AVAssetWriter to add to your video
                [adaptor appendPixelBuffer:buffer withPresentationTime:presentTime];
                i++;
            }
            else
            {

              //Finish the session:
              // This is important to be done exactly in this order
              [writerInput markAsFinished];
              // WARNING: finishWriting in the solution above is deprecated.
              // You now need to give a completion handler.
              [videoWriter finishWritingWithCompletionHandler:^{
                  NSLog(@"Finished writing...checking completion status...");
                  if (videoWriter.status != AVAssetWriterStatusFailed && videoWriter.status == AVAssetWriterStatusCompleted)
                  {
                      NSLog(@"Video writing succeeded.");

                      // Move video to camera roll
                      // NOTE: You cannot write directly to the camera roll.
                      // You must first write to an iOS directory then move it!
                      NSURL *videoTempURL = [NSURL fileURLWithPath:[NSString stringWithFormat:@"%@", path]];
//                      [self saveToCameraRoll:videoTempURL];

                  } else
                  {
                      NSLog(@"Video writing failed: %@", videoWriter.error);
                  }

              }]; // end videoWriter finishWriting Block

              CVPixelBufferPoolRelease(adaptor.pixelBufferPool);

              NSLog (@"Done");
              break;
          }
      }
  }

  NSLog(@"writeImageAsMovie  line 286");



}



+ (CVPixelBufferRef) pixelBufferFromCGImage: (CGImageRef) image
{
    // This again was just our utility class for the height & width of the
    // incoming video (640 height x 480 width)
//    ATHSingleton *singleton = [ATHSingleton singletons];
//  int height = height;//singleton.screenHeight;
//  int width = width;// singleton.screenWidth;

  NSLog(@"pixelBufferFromCGImage  line 302");

    NSDictionary *options = [NSDictionary dictionaryWithObjectsAndKeys:
                             [NSNumber numberWithBool:YES], kCVPixelBufferCGImageCompatibilityKey,
                             [NSNumber numberWithBool:YES], kCVPixelBufferCGBitmapContextCompatibilityKey,
                             nil];
    CVPixelBufferRef pxbuffer = NULL;

    CVReturn status = CVPixelBufferCreate(kCFAllocatorDefault, width,
                                          height, kCVPixelFormatType_32ARGB, (__bridge CFDictionaryRef) options,
                                          &pxbuffer);

    NSParameterAssert(status == kCVReturnSuccess && pxbuffer != NULL);

    CVPixelBufferLockBaseAddress(pxbuffer, 0);
    void *pxdata = CVPixelBufferGetBaseAddress(pxbuffer);
    NSParameterAssert(pxdata != NULL);

    CGColorSpaceRef rgbColorSpace = CGColorSpaceCreateDeviceRGB();

    CGContextRef context = CGBitmapContextCreate(pxdata, width,
                                                 height, 8, 4*width, rgbColorSpace,
                                                 kCGImageAlphaNoneSkipFirst);
    NSParameterAssert(context);
    CGContextConcatCTM(context, CGAffineTransformMakeRotation(0));
    CGContextDrawImage(context, CGRectMake(0, 0, CGImageGetWidth(image),
                                           CGImageGetHeight(image)), image);
    CGColorSpaceRelease(rgbColorSpace);
    CGContextRelease(context);

    CVPixelBufferUnlockBaseAddress(pxbuffer, 0);

  
  NSLog(@"pixelBufferFromCGImage  line 335");

    return pxbuffer;
}


//- (void) saveToCameraRoll:(NSURL *)srcURL
//{
//    NSLog(@"srcURL: %@", srcURL);
//
//    ALAssetsLibrary *library = [[ALAssetsLibrary alloc] init];
//    ALAssetsLibraryWriteVideoCompletionBlock videoWriteCompletionBlock =
//    ^(NSURL *newURL, NSError *error) {
//        if (error) {
//            NSLog( @"Error writing image with metadata to Photo Library: %@", error );
//        } else {
//            NSLog( @"Wrote image with metadata to Photo Library %@", newURL.absoluteString);
//            [[[ALAssetsLibrary alloc] init] writeVideoAtPathToSavedPhotosAlbum:newURL completionBlock:^(NSURL *assetURL, NSError *error) {
//                        if(assetURL) {
//                            NSLog(@"saved down");
//                        } else {
//                            NSLog(@"something wrong");
//                        }
//                    }];
//
//        }
//    };
//
//    if ([library videoAtPathIsCompatibleWithSavedPhotosAlbum:srcURL])
//    {
//        [library writeVideoAtPathToSavedPhotosAlbum:srcURL
//                                    completionBlock:videoWriteCompletionBlock];
//    }
//}



@end


extern "C" {
  int sampleFunc(int number){
    return 2*number;
  }
  
  void init(unsigned int height, unsigned int width){
    height = height;
    width = width;
    images = [NSMutableArray array];
    NSLog(@"init  line 384");

  }
  
  void _appendFrameFromImage(const Byte* a_ImageBuffer, int a_BufferLength, int a_FrameTimeInMilliseconds, int a_startOrStop) {

    NSLog(@"_appendFrameFromImage  line 372");

    if (a_startOrStop == 1) {
      NSLog(@"_appendFrameFromImage  line 396");
//
//      NSData * imageData = [[NSData alloc] initWithBytes:a_ImageBuffer length:a_BufferLength];
//      NSLog(@"_appendFrameFromImage  line 399");
//
//      const char bytes[] = "\x00\x00\x00\x01";
//      size_t length = (sizeof bytes) - 1; //string literals have implicit trailing '\0'
//      NSData *ByteHeader = [NSData dataWithBytes:bytes length:length];
//
//      NSMutableData* contactData = [ByteHeader mutableCopy];
//      [contactData appendData:imageData];
//      [contactData appendData:ByteHeader];
//
//      UIImage* image = [[UIImage alloc] initWithData:contactData];
//      NSLog(@"_appendFrameFromImage  line 402");
//
//      NSLog(@"image height %f", image.size.height);
//      NSLog(@"image height %f", image.size.width);
//
//      [images addObject:image];
//
//      NSLog(@"_appendFrameFromImage  line 406");
      
      NSData * imageData = [[NSData alloc] initWithBytes:a_ImageBuffer length:a_BufferLength];
      NSLog(@"image data  %@",imageData);
//      NSRange range = NSMakeRange(4, [imageData length] - 4);
//      NSData *refinedData = [imageData subdataWithRange:range];
//      NSLog(@"refinedData  %@",refinedData);
      
      UIImage* image = [[UIImage alloc] initWithData:imageData];
      
      NSLog(@"image height   %f",image.size.height);
      NSLog(@"image width   %f",image.size.width);
      
      [images addObject:image];
      
     

    }else{
      NSLog(@"_appendFrameFromImage line 379");

      [CreateVideoFromImages saveMovieToLibrary:images a_height:height a_width:width];
      
      NSLog(@"_appendFrameFromImage  line 383");

    }
    
  }
  
}
