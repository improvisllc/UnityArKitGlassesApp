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


@interface AudioVideoControlProvider : NSObject

+(void) startRecording;
+(void) stopRecording;
+(void) mergeVideoWithAudio:(NSString*)videoPath;
+(const char*) audioPath;
+(void)saveMedia:(NSURL*)videoUrl;
+(void)playAudio;
@end

@implementation AudioVideoControlProvider
static AVAudioRecorder *audioRecorder;
static AVAudioPlayer *audioPlayer;
static NSURL *soundFileURL;
static NSString *outputPath;
//static const char *kCallbackTarget = "ReplayKitBridge";
static int saveVideoAndAudio;
static int videoSaved;

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
  NSLog (@"successfully");

}

-(void)audioRecorderEncodeErrorDidOccur:
(AVAudioRecorder *)recorder
error:(NSError *)error
{
  NSLog(@"Encode Error occurred");
}

+ (void)mergeVideoWithAudio:(NSString*)videoPath{
  
    saveVideoAndAudio = 0;
//  NSLog(@"videoPathNSString  Meri  %@",videoPath);

  NSURL *audioUrl = soundFileURL;

  NSURL *videoUrl = [NSURL fileURLWithPath:videoPath];
  
  
//  [self cropVideoOrAudio:videoUrl audioUrl:soundFileURL];
    
    
    
    AVAsset *aAudioAsset = [AVAsset assetWithURL:audioUrl];
    AVAsset *aVideoAsset = [AVAsset assetWithURL:videoUrl];
    
    CMTime videoTime = [aVideoAsset duration];
    NSUInteger videoTotalSeconds = CMTimeGetSeconds(videoTime);
    
    CMTime audioTime = [aAudioAsset duration];
    NSLog(@"audioTime.value  %lld",(audioTime.value*1000)/audioTime.timescale);
    
    double audioMiliseconds =(audioTime.value*1000)/audioTime.timescale;
    double videoMiliseconds = (videoTime.value*1000)/videoTime.timescale;
    NSLog(@"audioMiliseconds  %f   videoMiliseconds  %f",audioMiliseconds,videoMiliseconds);

//    AVAssetExportSession *exportSession;
//    NSURL *outputURL;
//    CMTimeRange range;
    
//    if (audioMiliseconds > videoMiliseconds){
//      NSArray *compatiblePresets = [AVAssetExportSession exportPresetsCompatibleWithAsset:aAudioAsset];
//      if ([compatiblePresets containsObject:AVAssetExportPresetMediumQuality]) {
//        outputURL = audioUrl;
//        if ([[NSFileManager defaultManager] fileExistsAtPath:outputURL.path]) {
////          NSLog(@"output crop audio %@",outputURL.path);
//
//            [[NSFileManager defaultManager] removeItemAtPath:outputURL.path error:nil];
//        }
//
//        exportSession = [[AVAssetExportSession alloc] initWithAsset:aAudioAsset presetName:AVAssetExportPresetHighestQuality];
//
//        double different = audioMiliseconds - videoMiliseconds;
////        NSLog(@"(audioTime.value*1000)/(5*audioTime.timescale)  %f    %f   %f",different/1000,  audioMiliseconds/1000,videoMiliseconds/1000);
//
//        CMTime start = CMTimeMakeWithSeconds(different/1000, aAudioAsset.duration.timescale);
//        CMTime duration = CMTimeMakeWithSeconds(audioMiliseconds/1000, aAudioAsset.duration.timescale);
//        range = CMTimeRangeMake(start, duration);
//
//      }
//    }else if (audioMiliseconds < videoMiliseconds){
//
//      NSArray *compatiblePresets = [AVAssetExportSession exportPresetsCompatibleWithAsset:aVideoAsset];
//      if ([compatiblePresets containsObject:AVAssetExportPresetMediumQuality]) {
//
//        outputURL = videoUrl;
//
//        if ([[NSFileManager defaultManager] fileExistsAtPath:outputURL.path]) {
////          NSLog(@"output crop video %@",outputURL.path);
//
//            [[NSFileManager defaultManager] removeItemAtPath:outputURL.path error:nil];
//        }
//
//        exportSession = [[AVAssetExportSession alloc] initWithAsset:aVideoAsset presetName:AVAssetExportPresetHighestQuality];
//
//        double different = videoMiliseconds - audioMiliseconds;
////        NSLog(@"(audioTime.value*1000)/(5*audioTime.timescale)  %f    %f   %f",different/1000,  audioMiliseconds/1000,videoMiliseconds/1000);
//
//        CMTime start = CMTimeMakeWithSeconds(different/1000, aVideoAsset.duration.timescale);
//        CMTime duration = CMTimeMakeWithSeconds(videoMiliseconds/1000, aVideoAsset.duration.timescale);
//        range = CMTimeRangeMake(start, duration);
//      }
//    }
//
//    exportSession.outputURL = outputURL;
//    exportSession.outputFileType = AVFileTypeMPEG4;
//    exportSession.timeRange = range;
//
//    [exportSession exportAsynchronouslyWithCompletionHandler:^{
//        switch (exportSession.status) {
//            case AVAssetExportSessionStatusFailed:
//                saveVideoAndAudio = 0;
//                break;
//            case AVAssetExportSessionStatusCancelled:
//                saveVideoAndAudio = 0;
//                break;
//            default:{
//              NSLog(@"~~~~~ SUCCES ~~~~~~~~  crop audio");
                
                AVMutableComposition *mixComposition = [AVMutableComposition new];
                NSMutableArray<AVMutableCompositionTrack *> *mutableCompositionVideoTrack = [NSMutableArray new];
                NSMutableArray<AVMutableCompositionTrack *> *mutableCompositionAudioTrack = [NSMutableArray new];
                AVMutableVideoCompositionInstruction *totalVideoCompositionInstruction = [AVMutableVideoCompositionInstruction new];
                
//                AVAsset *aVideoAsset = [AVAsset assetWithURL:videoUrl];
//                AVAsset *aAudioAsset = [AVAsset assetWithURL:audioUrl];
                
                AVMutableCompositionTrack *videoTrack = [mixComposition addMutableTrackWithMediaType:AVMediaTypeVideo preferredTrackID:kCMPersistentTrackID_Invalid];
                AVMutableCompositionTrack *audioTrack = [mixComposition addMutableTrackWithMediaType:AVMediaTypeAudio preferredTrackID:kCMPersistentTrackID_Invalid];
                if (videoTrack && audioTrack) {
                    [mutableCompositionVideoTrack addObject:videoTrack];
                    [mutableCompositionAudioTrack addObject:audioTrack];
                    
//                  NSLog(@"video track  %@",videoTrack);
//                  NSLog(@"audio track  %@",audioTrack);
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
//                NSLog(@"output  %@",outputPath);
                if ([[NSFileManager defaultManager] fileExistsAtPath:outputPath]) {
//                  NSLog(@"output  %@",outputPath);

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
//                            NSLog(@"~~~~~ Failure ~~~~~~~~ %@",exportSession.error);
                            saveVideoAndAudio = 0;
                            break;
                        case AVAssetExportSessionStatusCancelled:
//                            NSLog(@"~~~~~ Failure ~~~~~~~~");
                            saveVideoAndAudio = 0;
                            break;
                      default: {
//                          NSLog(@"~~~~~ SUCCES ~~~~~~~~");
//                          [self saveMedia:outputURL];
                          saveVideoAndAudio = 1;
                          break;
                      }
                    }
                }];
              

  //            [self cropVideo:outputURL];
//          }
//        }
//    }];
  
}

+(int) isMergedVideoWithAudio{
    return saveVideoAndAudio;
}


+(void)cropVideoOrAudio:(NSURL*)videoUrl audioUrl:(NSURL*)audioUrl{

  AVAsset *aAudioAsset = [AVAsset assetWithURL:audioUrl];
  AVAsset *aVideoAsset = [AVAsset assetWithURL:videoUrl];
  
  CMTime videoTime = [aVideoAsset duration];
//  NSUInteger videoTotalSeconds = CMTimeGetSeconds(videoTime);
  
  CMTime audioTime = [aAudioAsset duration];
//  NSLog(@"audioTime.value  %lld",(audioTime.value*1000)/audioTime.timescale);
  
  double audioMiliseconds =(audioTime.value*1000)/audioTime.timescale;
  double videoMiliseconds = (videoTime.value*1000)/videoTime.timescale;
//  NSLog(@"audioMiliseconds  %f   videoMiliseconds  %f",audioMiliseconds,videoMiliseconds);

  AVAssetExportSession *exportSession;
  NSURL *outputURL;
  CMTimeRange range;
  
  if(audioMiliseconds == videoMiliseconds) {
    return;
  }else if (audioMiliseconds > videoMiliseconds){
    NSArray *compatiblePresets = [AVAssetExportSession exportPresetsCompatibleWithAsset:aAudioAsset];
    if ([compatiblePresets containsObject:AVAssetExportPresetMediumQuality]) {
      outputURL = audioUrl;
      if ([[NSFileManager defaultManager] fileExistsAtPath:outputURL.path]) {
//        NSLog(@"output crop audio %@",outputURL.path);

          [[NSFileManager defaultManager] removeItemAtPath:outputURL.path error:nil];
      }
      
      exportSession = [[AVAssetExportSession alloc] initWithAsset:aAudioAsset presetName:AVAssetExportPresetHighestQuality];
      
      double different = audioMiliseconds - videoMiliseconds;
//      NSLog(@"(audioTime.value*1000)/(5*audioTime.timescale)  %f    %f   %f",different/1000,  audioMiliseconds/1000,videoMiliseconds/1000);
      
      CMTime start = CMTimeMakeWithSeconds(different/1000, aAudioAsset.duration.timescale);
      CMTime duration = CMTimeMakeWithSeconds(audioMiliseconds/1000, aAudioAsset.duration.timescale);
      range = CMTimeRangeMake(start, duration);

    }
  }else if (audioMiliseconds < videoMiliseconds){
  
    NSArray *compatiblePresets = [AVAssetExportSession exportPresetsCompatibleWithAsset:aVideoAsset];
    if ([compatiblePresets containsObject:AVAssetExportPresetMediumQuality]) {
      
      outputURL = videoUrl;

      if ([[NSFileManager defaultManager] fileExistsAtPath:outputURL.path]) {
//        NSLog(@"output crop video %@",outputURL.path);

          [[NSFileManager defaultManager] removeItemAtPath:outputURL.path error:nil];
      }
      
      exportSession = [[AVAssetExportSession alloc] initWithAsset:aVideoAsset presetName:AVAssetExportPresetHighestQuality];
      
      double different = videoMiliseconds - audioMiliseconds;
//      NSLog(@"(audioTime.value*1000)/(5*audioTime.timescale)  %f    %f   %f",different/1000,  audioMiliseconds/1000,videoMiliseconds/1000);
      
      CMTime start = CMTimeMakeWithSeconds(different/1000, aVideoAsset.duration.timescale);
      CMTime duration = CMTimeMakeWithSeconds(videoMiliseconds/1000, aVideoAsset.duration.timescale);
      range = CMTimeRangeMake(start, duration);
    }
  }
  
  exportSession.outputURL = outputURL;
  exportSession.outputFileType = AVFileTypeMPEG4;
  exportSession.timeRange = range;
  
  [exportSession exportAsynchronouslyWithCompletionHandler:^{
      switch (exportSession.status) {
          case AVAssetExportSessionStatusFailed:
//                failure(exportSession.error);
//          NSLog(@"~~~~~ Failure ~~~~~~~~ crop audio  %@",exportSession.error);
              break;
          case AVAssetExportSessionStatusCancelled:
//                failure(exportSession.error);
//          NSLog(@"~~~~~ Failure ~~~~~~~~  crop audio");

              break;
          default:{
//            NSLog(@"~~~~~ SUCCES ~~~~~~~~  crop audio");
//            [self cropVideo:outputURL];
        }
      }
  }];
    
}

+(const char*) audioPath{
  const char* currentPath = [soundFileURL.path UTF8String];
//  NSLog(@"video path  currentn path  %s",currentPath);
  return currentPath;
}


+(void)saveMedia:(NSURL*)videoUrl{
//  NSLog(@"source will be : %@", videoUrl.absoluteString);
  videoSaved = 0;
  NSURL *sourceURL = videoUrl;


  if([[NSFileManager defaultManager] fileExistsAtPath:[videoUrl absoluteString]]) {
      [[[ALAssetsLibrary alloc] init] writeVideoAtPathToSavedPhotosAlbum:videoUrl completionBlock:^(NSURL *assetURL, NSError *error) {

          if(assetURL) {
              NSLog(@"saved down");
            videoSaved = 1;
          } else {
            videoSaved = 0;
              NSLog(@"something wrong");
          }
      }];
  }else {
    
    NSURLSessionTask *download = [[NSURLSession sharedSession] downloadTaskWithURL:sourceURL completionHandler:^(NSURL *location, NSURLResponse *response, NSError *error) {
        if(error) {
          videoSaved = 0;
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
              videoSaved = 1;
                NSLog(@"saved down");
//                [[NSFileManager defaultManager] removeItemAtURL:tempURL error:nil];
            } else {
              videoSaved = 0;
                NSLog(@"something wrong %@", error.localizedDescription);
//                [[NSFileManager defaultManager] removeItemAtURL:tempURL error:nil];
            }
        }];
    }];
    [download resume];
  }
}


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

+(int)isvideoSaved{
    return  videoSaved;
}



@end

extern "C"{
  void startAudioRecording(){
//    NSLog(@"Start recording MEri");
    [AudioVideoControlProvider startRecording];
  }
  
  void stopAudioRecording(){
//    NSLog(@"Stop recording MEri");
    [AudioVideoControlProvider stopRecording];
  }
  
  int mergeVideoWithAudio(char* videoPath){
    NSString* videoPathNSString;
    int saveVideoAndAudio;
    if (videoPath != NULL){
      videoPathNSString = [NSString stringWithUTF8String:videoPath];
//      NSLog(@"videoPathNSString  Meri  %@",videoPathNSString);
      [AudioVideoControlProvider mergeVideoWithAudio:videoPathNSString];
    }
    else {
      saveVideoAndAudio = 0;
      videoPathNSString = [NSString stringWithUTF8String:""];
    }
//    NSLog(@"videoPathNSString  Meri  %@",videoPathNSString);
    return  saveVideoAndAudio;
  }


int isMergedVideoWithAudio(){
    return  [AudioVideoControlProvider isMergedVideoWithAudio];
}
  
  
  char* getAudioPath(){
//    NSLog(@"getAudio Path  Meri  ");
    const char* currentAudioPath = [AudioVideoControlProvider audioPath];
//    NSLog(@"currentpath  Meri  %s",currentAudioPath);
    
    
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
//          NSLog(@"currentpath  Meri  %s",res);

            return res;
        }
  }
  
  void saveVideo(char* videoPath){
    NSString* videoPathNSString;
    if (videoPath != NULL){
      videoPathNSString = [NSString stringWithUTF8String:videoPath];
//      NSLog(@ /"videoPathNSString  Meri  %@",videoPathNSString);
      NSURL *videoUrl = [NSURL fileURLWithPath:videoPathNSString];
      [AudioVideoControlProvider saveMedia:videoUrl];
    }
    else {
      videoPathNSString = [NSString stringWithUTF8String:""];
    }
    
  }
  
  void playAudio(){
    [AudioVideoControlProvider playAudio];
  }

  void changeSpeakerConfigurationToDefault(){
    [[AVAudioSession sharedInstance] setCategory:AVAudioSessionCategoryPlayAndRecord
                                       withOptions:AVAudioSessionCategoryOptionDefaultToSpeaker
                                             error:nil];
  }

  int isVideoSaved(){
    return [AudioVideoControlProvider isvideoSaved];
  }

}
