export interface SnackbarConfig {
    message: string;
    icon?: string;
    closeLabel?: string;
    color?: "error" | "neutral" | "primary" | "success" | "warning";
    gravity?: "bottom" | "top";
    position?: "center" | "left" | "right";
    time?: number;
    type?: "bold" | "subtle";
}