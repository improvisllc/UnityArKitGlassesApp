//
//  NativeUIProvider.m
//  CreateVideoFromImages
//
//  Created by Hovhannes Sargsyan on 10.02.21.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface NativeUIProvider : UIViewController

@end

@implementation NativeUIProvider
static int alertActionTag;
static UIViewController *presentedController;


+(void)showAlertView:(NSString*)title fisrtActionName:(NSString*)firstActionName secondActionName:(NSString*)secondActionName{

  alertActionTag = 0;
  UIAlertController* alert = [UIAlertController alertControllerWithTitle:title
                             message:@""
                             preferredStyle:UIAlertControllerStyleAlert];

  UIAlertAction* firstAction = [UIAlertAction actionWithTitle:firstActionName style:UIAlertActionStyleDefault
                                 handler:^(UIAlertAction * action) {
    alertActionTag = 1;
  }];
  
  UIAlertAction* secondAction = [UIAlertAction actionWithTitle:secondActionName       style:UIAlertActionStyleDefault
                                 handler:^(UIAlertAction * action) {
    alertActionTag = 2;
  }];

  [alert addAction:firstAction];
  [alert addAction:secondAction];

  [presentedController presentViewController:alert animated:YES completion:nil];
  
}

+(int) isTappetAction{
  return alertActionTag;
}

@end

//extern "C"{
  void showAlertView(char* title, char* firstActionName, char* secondActionName){
    NSString* titleObj = [NSString stringWithUTF8String:title];
    NSString* firstActionNameObj = [NSString stringWithUTF8String:firstActionName];
    NSString* secondActionNameObj = [NSString stringWithUTF8String:secondActionName];
    [NativeUIProvider showAlertView:titleObj fisrtActionName:firstActionNameObj secondActionName:secondActionNameObj];
  }
  
  int isTappedAction(){
    return [NativeUIProvider isTappetAction];
  }
//}
