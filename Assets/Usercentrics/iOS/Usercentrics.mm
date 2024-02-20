#import <Foundation/Foundation.h>
#import <Usercentrics/Usercentrics.h>
#import <UsercentricsUI/UsercentricsUI-Swift.h>
#import <UIKit/UIKit.h>

extern UIViewController *UnityGetGLViewController();
extern char *toChar(NSString *value);

@interface UsercentricsHelper : NSObject
+(void)sendUnityMessageWithObj:(NSString *)obj andMethod:(NSString*)method andMsg:(NSString*)msg;
@end

@implementation UsercentricsHelper
+(void)sendUnityMessageWithObj:(NSString *)obj andMethod:(NSString*)method andMsg:(NSString*)msg {
    UnitySendMessage(toChar(obj), toChar(method), toChar(msg));
}
@end

#pragma mark - C interface
char *toChar(NSString *value) {
    char *string = (char *)[value UTF8String];
    if (string == nil)
        return NULL;
    char* copy = (char*)malloc(strlen(string) + 1);
    strcpy(copy, string);
    return copy;
}

NSString* _Nonnull CreateNSString(const char* string) {
    return [NSString stringWithUTF8String:string ?: ""];
}

extern "C" {

    void initCMP(const char* initialArgs) {
        [[UsercentricsUsercentricsUnityCompanion companion] doInitAppContext:nil rawUnityUserOptions:CreateNSString(initialArgs)];
    }

    void showFirstLayer() {
        UsercentricsUnityBanner *banner = [[UsercentricsUnityBanner alloc] init];
        [banner showFirstLayerWithHostView:UnityGetGLViewController()];
    }

    void showSecondLayer(bool showCloseButton) {
        UsercentricsUnityBanner *banner = [[UsercentricsUnityBanner alloc] init];
        [banner showSecondLayerWithHostView:UnityGetGLViewController() showCloseButton:showCloseButton];
    }

    char* getControllerId() {
        return toChar([[UsercentricsUsercentricsUnityCompanion companion] getControllerId]);
    }

    void getTCFData() {
        [[UsercentricsUsercentricsUnityCompanion companion] getTCFData];
    }

    char* getUSPData() {
        return toChar([[UsercentricsUsercentricsUnityCompanion companion] getUSPData]);
    }

    void restoreUserSession(const char* controllerId) {
        [[UsercentricsUsercentricsUnityCompanion companion] restoreUserSessionControllerId:CreateNSString(controllerId)];
    }

    void reset() {
        [[UsercentricsUsercentricsUnityCompanion companion] reset];
    }

    void subscribeOnConsentUpdated() {
        [[UsercentricsUsercentricsUnityCompanion companion] subscribeOnConsentUpdated];
    }

    void disposeOnConsentUpdatedSubscription() {
        [[UsercentricsUsercentricsUnityCompanion companion] disposeOnConsentUpdatedSubscription];
    }

    char* getFirstLayerSettings() {
        return toChar([[UsercentricsUsercentricsUnityCompanion companion] getFirstLayerSettings]);
    }

    void acceptAll() {
        [[UsercentricsUsercentricsUnityCompanion companion] acceptAllFirstLayerForTCF];
    }

    void denyAll() {
        [[UsercentricsUsercentricsUnityCompanion companion] denyAllFirstLayerForTCF];
    }

    void track(int eventType) {
        [[UsercentricsUsercentricsUnityCompanion companion] trackEventTypeEnumIndex:eventType];
    }

    void setCmpId(int cmpId) {
        [[UsercentricsUsercentricsUnityCompanion companion] setCmpIdCmpId:cmpId];
    }
}
