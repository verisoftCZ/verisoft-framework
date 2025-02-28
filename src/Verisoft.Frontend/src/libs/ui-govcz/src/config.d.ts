declare global {
    interface Window {
        GOV_DS_CONFIG?: {
            canValidateWcagOnRender?: boolean;
            iconsPath?: string;
        };
    }
}

window.GOV_DS_CONFIG ??= { canValidateWcagOnRender: true, iconsPath: '/assets/govcz/icons' };
