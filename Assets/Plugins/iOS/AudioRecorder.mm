//
//  AudioRecorder.m
//  CreateVideoFromImages
//
//  Created by Hovhannes Sargsyan on 31.01.21.
//

#import <AVFoundation/AVFoundation.h>
#import <AVKit/AVKit.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <Photos/Photos.h>


@interface AudioRecorder : NSObject

+(void) startRecording;
+(void) stopRecording;
+(void) mergeVideoWithAudio:(NSString*)videoPath;
+(const char*) videoPath;
+(void)saveMedia:(NSURL*)videoUrl;
+(void)playAudio;
@end

@implementation AudioRecorder
static AVAudioRecorder *audioRecorder;
static AVAudioPlayer *audioPlayer;
static NSURL *soundFileURL;
static NSString *outputPath;

+(void) startRecording{
  NSArray *dirPaths;
  NSString *docsDir;

  dirPaths = NSSearchPathForDirectoriesInDomains(
       NSDocumentDirectory, NSUserDomainMask, YES);
  docsDir = dirPaths[0];

   NSString *soundFilePath = [docsDir
      stringByAppendingPathComponent:@"sound.caf"];

  soundFileURL = [NSURL fileURLWithPath:soundFilePath];

  NSDictionary *recordSettings = [NSDictionary
           dictionaryWithObjectsAndKeys:
           [NSNumber numberWithInt:AVAudioQualityMin],
           AVEncoderAudioQualityKey,
           [NSNumber numberWithInt:16],
           AVEncoderBitRateKey,
           [NSNumber numberWithInt: 2],
           AVNumberOfChannelsKey,
           [NSNumber numberWithFloat:44100.0],
           AVSampleRateKey,
           nil];

  NSError *error = nil;

  AVAudioSession *audioSession = [AVAudioSession sharedInstance];
  [audioSession setCategory:AVAudioSessionCategoryPlayAndRecord
       error:nil];

 audioRecorder = [[AVAudioRecorder alloc]
                 initWithURL:soundFileURL
                 settings:recordSettings
                 error:&error];

  if (error)
  {
    NSLog(@"error: %@", [error localizedDescription]);
  } else {
     [audioRecorder prepareToRecord];
  }
  
  
  if (!audioRecorder.recording)
 {
   [audioRecorder record];
 }

}

+(void)stopRecording{
  if (audioRecorder.recording)
  {
      [audioRecorder stop];
  }
}


-(void)audioPlayerDidFinishPlaying:
(AVAudioPlayer *)player successfully:(BOOL)flag
{
  NSLog(@"audio Player Did Finish Playing");
}

-(void)audioPlayerDecodeErrorDidOccur:
(AVAudioPlayer *)player
error:(NSError *)error
{
   NSLog(@"Decode Error occurred");
}

-(void)audioRecorderDidFinishRecording:
(AVAudioRecorder *)recorder
successfully:(BOOL)flag
{
  NSLog (@"audioRecorderDidFinishRecording:successfully:");

}

-(void)audioRecorderEncodeErrorDidOccur:
(AVAudioRecorder *)recorder
error:(NSError *)error
{
  NSLog(@"Encode Error occurred");
}

+ (void)mergeVideoWithAudio:(NSString*)videoPath{
  
  NSLog(@"videoPathNSString  Meri  %@",videoPath);

  
//  NSURL* audioUrl = [NSURL fileURLWithPath: @"/Users/improvis/Desktop/audio.caf"];
//  NSURL *audioUrl = [NSURL URLWithString:@"http://www.mfiles.co.uk/mp3-downloads/Toccata-and-Fugue-Dm.mp3"];
//  NSURL *audioUrl = [NSURL fileURLWithPath: soundFileURL];
  NSURL *audioUrl = soundFileURL;

  NSLog(@"recorderFilePath   %@",soundFileURL);
  //  NSURL* videoUrl = [[NSURL alloc]initWithString:@"/Users/improvis/Desktop/movie6.mp4"];
//  NSURL *videoUrl = [NSURL URLWithString:@"https://github.com/versluis/Movie-Player/blob/master/Movie%20Player/video.mov?raw=true"];
  NSURL *videoUrl = [NSURL fileURLWithPath:videoPath];
  
    AVMutableComposition *mixComposition = [AVMutableComposition new];
    NSMutableArray<AVMutableCompositionTrack *> *mutableCompositionVideoTrack = [NSMutableArray new];
    NSMutableArray<AVMutableCompositionTrack *> *mutableCompositionAudioTrack = [NSMutableArray new];
    AVMutableVideoCompositionInstruction *totalVideoCompositionInstruction = [AVMutableVideoCompositionInstruction new];
    
    AVAsset *aVideoAsset = [AVAsset assetWithURL:videoUrl];
    AVAsset *aAudioAsset = [AVAsset assetWithURL:audioUrl];
    
    AVMutableCompositionTrack *videoTrack = [mixComposition addMutableTrackWithMediaType:AVMediaTypeVideo preferredTrackID:kCMPersistentTrackID_Invalid];
    AVMutableCompositionTrack *audioTrack = [mixComposition addMutableTrackWithMediaType:AVMediaTypeAudio preferredTrackID:kCMPersistentTrackID_Invalid];
    if (videoTrack && audioTrack) {
        [mutableCompositionVideoTrack addObject:videoTrack];
        [mutableCompositionAudioTrack addObject:audioTrack];
        
      NSLog(@"video track  %@",videoTrack);
      NSLog(@"audio track  %@",audioTrack);
        AVAssetTrack *aVideoAssetTrack = [aVideoAsset tracksWithMediaType:AVMediaTypeVideo].firstObject;
        AVAssetTrack *aAudioAssetTrack = [aAudioAsset tracksWithMediaType:AVMediaTypeAudio].firstObject;
        
        if (aVideoAssetTrack && aAudioAssetTrack) {
            [mutableCompositionVideoTrack.firstObject insertTimeRange:CMTimeRangeMake(kCMTimeZero, aVideoAssetTrack.timeRange.duration) ofTrack:aVideoAssetTrack atTime:kCMTimeZero error:nil];
            
            CMTime videoDuration = aVideoAsset.duration;
            if (CMTimeCompare(videoDuration, aAudioAsset.duration) == -1) {
                [mutableCompositionAudioTrack.firstObject insertTimeRange:CMTimeRangeMake(kCMTimeZero, aVideoAssetTrack.timeRange.duration) ofTrack:aAudioAssetTrack atTime:kCMTimeZero error:nil];
            } else if (CMTimeCompare(videoDuration, aAudioAsset.duration) == 1) {
                CMTime currentDuration = kCMTimeZero;
                while (CMTimeCompare(currentDuration, videoDuration) == -1) {
                    // repeats audio
                    CMTime restTime = CMTimeSubtract(videoDuration, currentDuration);
                    CMTime maxTime = CMTimeMinimum(aAudioAsset.duration, restTime);
                    [mutableCompositionAudioTrack.firstObject insertTimeRange:CMTimeRangeMake(kCMTimeZero, maxTime) ofTrack:aAudioAssetTrack atTime:currentDuration error:nil];
                    currentDuration = CMTimeAdd(currentDuration, aAudioAsset.duration);
                }
            }
            videoTrack.preferredTransform = aVideoAssetTrack.preferredTransform;
            totalVideoCompositionInstruction.timeRange = CMTimeRangeMake(kCMTimeZero, aVideoAssetTrack.timeRange.duration);
        }
    }
    
//  NSString *outputPath = @"/Users/improvis/Desktop/screenCapture.mp4";
  
//    NSString *outputPath = [NSHomeDirectory() stringByAppendingPathComponent:@"Documents/videoWithAudio.mp4"];
    outputPath = videoPath;
    NSLog(@"output  %@",outputPath);
    if ([[NSFileManager defaultManager] fileExistsAtPath:outputPath]) {
      NSLog(@"output  %@",outputPath);

        [[NSFileManager defaultManager] removeItemAtPath:outputPath error:nil];
    }
    NSURL *outputURL = [NSURL fileURLWithPath:outputPath];
    
    AVAssetExportSession *exportSession = [[AVAssetExportSession alloc] initWithAsset:mixComposition presetName:AVAssetExportPresetHighestQuality];
    exportSession.outputURL = outputURL;
    exportSession.outputFileType = AVFileTypeMPEG4;
    exportSession.shouldOptimizeForNetworkUse = YES;
    
    // try to export the file and handle the status cases
    [exportSession exportAsynchronouslyWithCompletionHandler:^{
        switch (exportSession.status) {
            case AVAssetExportSessionStatusFailed:
//                failure(exportSession.error);
            NSLog(@"~~~~~ Failure ~~~~~~~~ %@",exportSession.error);

                break;
            case AVAssetExportSessionStatusCancelled:
//                failure(exportSession.error);
            NSLog(@"~~~~~ Failure ~~~~~~~~");

                break;
          default: {
              NSLog(@"~~~~~ SUCCES ~~~~~~~~");
              [self saveMedia:outputURL];
              break;
          }
        }
    }];
  

}
+(const char*) audioPath{
  const char* currentPath = [soundFileURL.path UTF8String];
  NSLog(@"video path  currentn path  %s",currentPath);
  return currentPath;
}


+(void)saveMedia:(NSURL*)videoUrl{
  NSLog(@"source will be : %@", videoUrl.absoluteString);
  NSURL *sourceURL = videoUrl;


  if([[NSFileManager defaultManager] fileExistsAtPath:[videoUrl absoluteString]]) {
      [[[ALAssetsLibrary alloc] init] writeVideoAtPathToSavedPhotosAlbum:videoUrl completionBlock:^(NSURL *assetURL, NSError *error) {

          if(assetURL) {
              NSLog(@"saved down");
          } else {
              NSLog(@"something wrong");
          }
      }];
  }else {
    
    NSURLSessionTask *download = [[NSURLSession sharedSession] downloadTaskWithURL:sourceURL completionHandler:^(NSURL *location, NSURLResponse *response, NSError *error) {
        if(error) {
            NSLog(@"error saving: %@", error.localizedDescription);
            return;
        }

        NSURL *documentsURL = [[[NSFileManager defaultManager] URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask] firstObject];
        NSURL *tempURL = [documentsURL URLByAppendingPathComponent:[sourceURL lastPathComponent]];

        [[NSFileManager defaultManager] moveItemAtURL:location toURL:tempURL error:nil];

        [[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
            PHAssetChangeRequest *changeRequest = [PHAssetChangeRequest creationRequestForAssetFromVideoAtFileURL:tempURL];

            NSLog(@"%@", changeRequest.description);
        } completionHandler:^(BOOL success, NSError *error) {
            if (success) {
                NSLog(@"saved down");
                [[NSFileManager defaultManager] removeItemAtURL:tempURL error:nil];
            } else {
                NSLog(@"something wrong %@", error.localizedDescription);
                [[NSFileManager defaultManager] removeItemAtURL:tempURL error:nil];
            }
        }];
    }];
    [download resume];
  }
}

//+ (void)saveMedia:(UIImage *)image video:(NSURL *)video_url {
//    if(image) {
//        if(!image) {
//            return;
//        }
//
//    [[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
//        PHAssetChangeRequest *changeRequest = [PHAssetChangeRequest creationRequestForAssetFromImage:image];
//        NSLog(@"%@", changeRequest.description);
//    } completionHandler:^(BOOL success, NSError *error) {
//        if (success) {
//            NSLog(@"saved down");
//        } else {
//            NSLog(@"something wrong");
//        }
//    }];
//} else if (video_url) {
//    if([video_url absoluteString].length < 1) {
//        return;
//    }

//    NSLog(@"source will be : %@", video_url.absoluteString);
//    NSURL *sourceURL = video_url;
//
//    if([[NSFileManager defaultManager] fileExistsAtPath:[video_url absoluteString]]) {
//        [[[ALAssetsLibrary alloc] init] writeVideoAtPathToSavedPhotosAlbum:video_url completionBlock:^(NSURL *assetURL, NSError *error) {
//
//            if(assetURL) {
//                NSLog(@"saved down");
//            } else {
//                NSLog(@"something wrong");
//            }
//        }];
//    }

//    } else {
//
//        NSURLSessionTask *download = [[NSURLSession sharedSession] downloadTaskWithURL:sourceURL completionHandler:^(NSURL *location, NSURLResponse *response, NSError *error) {
//            if(error) {
//                NSLog(@"error saving: %@", error.localizedDescription);
//                return;
//            }
//
//            NSURL *documentsURL = [[[NSFileManager defaultManager] URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask] firstObject];
//            NSURL *tempURL = [documentsURL URLByAppendingPathComponent:[sourceURL lastPathComponent]];
//
//            [[NSFileManager defaultManager] moveItemAtURL:location toURL:tempURL error:nil];
//
//            [[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
//                PHAssetChangeRequest *changeRequest = [PHAssetChangeRequest creationRequestForAssetFromVideoAtFileURL:tempURL];
//
//                NSLog(@"%@", changeRequest.description);
//            } completionHandler:^(BOOL success, NSError *error) {
//                if (success) {
//                    NSLog(@"saved down");
//                    [[NSFileManager defaultManager] removeItemAtURL:tempURL error:nil];
//                } else {
//                    NSLog(@"something wrong %@", error.localizedDescription);
//                    [[NSFileManager defaultManager] removeItemAtURL:tempURL error:nil];
//                }
//            }];
//        }];
//        [download resume];
//    }
////   }
//}
//
//

+(void)playAudio{
  
  [[AVAudioSession sharedInstance] setCategory:AVAudioSessionCategoryPlayAndRecord
                                   withOptions:AVAudioSessionCategoryOptionDefaultToSpeaker
                                         error:nil];
  
  if (!audioRecorder.recording)
      {
          NSError *error;
          audioPlayer = [[AVAudioPlayer alloc]
          initWithContentsOfURL:audioRecorder.url
          error:&error];
          audioPlayer.delegate = self;
          if (error)
                NSLog(@"Error: %@",
                [error localizedDescription]);
          else
                [audioPlayer play];
     }
}



@end

extern "C"{
  void startRecording(){
    NSLog(@"Start recording MEri");
    [AudioRecorder startRecording];
  }
  
  void stopRecording(){
    NSLog(@"Stop recording MEri");
    [AudioRecorder stopRecording];
  }
  
  void mergeVideoWithAudio(char* videoPath){
    NSString* videoPathNSString;
    if (videoPath != NULL){
      videoPathNSString = [NSString stringWithUTF8String:videoPath];
      NSLog(@"videoPathNSString  Meri  %@",videoPathNSString);
    }
    else {
      videoPathNSString = [NSString stringWithUTF8String:""];
    }
    NSLog(@"videoPathNSString  Meri  %@",videoPathNSString);
    [AudioRecorder mergeVideoWithAudio:videoPathNSString];
  }
  
  
  char* getAudioPath(){
    NSLog(@"getAudio Path  Meri  ");
    const char* currentAudioPath = [AudioRecorder audioPath];
    NSLog(@"currentpath  Meri  %s",currentAudioPath);
    
    
    if(!currentAudioPath)
          return NULL;
        int i;
        char* res = NULL;
        res = (char*) malloc(strlen(currentAudioPath)+1);
        if(!res){
            fprintf(stderr, "Memory Allocation Failed! Exiting...\n");
            exit(EXIT_FAILURE);
        } else{
            for (i = 0; currentAudioPath[i] != '\0'; i++) {
                res[i] = currentAudioPath[i];
            }
            res[i] = '\0';
          NSLog(@"currentpath  Meri  %s",res);

            return res;
        }
    
//    return  currentVideoPath; z
  }
  
  void saveVideo(char* videoPath){
    NSString* videoPathNSString;
    if (videoPath != NULL){
      videoPathNSString = [NSString stringWithUTF8String:videoPath];
      NSLog(@"videoPathNSString  Meri  %@",videoPathNSString);
      NSURL *videoUrl = [NSURL fileURLWithPath:videoPathNSString];
      [AudioRecorder saveMedia:videoUrl];
    }
    else {
      videoPathNSString = [NSString stringWithUTF8String:""];
    }
    
  }
  
  void playAudio(){
    [AudioRecorder playAudio];
  }

}
